using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using RestSharp;
using System.Threading.Tasks;

namespace AliMNS
{
    public class MNSClient
    {
        private string url;
        private string accessKeyId;
        private string accessKeySecret;

        private string host;
        private string version = "2015-06-06";

        private RestClient restClient;

        static MNSClient()
        {
            System.Net.ServicePointManager.MaxServicePointIdleTime = 8000;
        }

        public MNSClient(string url, string accessKeyId, string accessKeySecret)
        {
            this.url = url;
            this.accessKeyId = accessKeyId;
            this.accessKeySecret = accessKeySecret;

            this.host = new Uri(url).Host;

            this.restClient = new RestClient(this.url);
        }

        public string Version
        {
            get {
                return this.version;
            }
            set {
                this.version = value;
            }
        }


        /// <summary>
        /// 同步执行api
        /// </summary>
        /// <typeparam name="TR"></typeparam>
        /// <param name="method"></param>
        /// <param name="resource"></param>
        /// <param name="headers"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        internal TR Execute<TR>(Method method, string resource, Dictionary<string, string> headers, MNSRequest input = null) where TR : MNSResponse, new()
        {
            //var restClient = new RestClient(this.url);
            var request = new RestRequest(resource, this.Map(method))
            {
                RequestFormat = DataFormat.Xml
            };
            this.RequestInit(method,request, headers, input);
          
            var task = new TaskCompletionSource<TR>();
           
            var responseasync = restClient.ExecuteAsync<TR>(request, response => {


                if (response.Data != null)
                {
                    task.SetResult(response.Data);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    task.SetResult( null);
                }
                else if (response.StatusCode == HttpStatusCode.Created)
                {
                    task.SetResult(null);
                }
                else
                {
               ;
                    task.SetException(new MNSRequestException(string.Format("{0}:{1}", response.StatusCode, response.ErrorMessage)));
                }

            });
            Task.WaitAll(task.Task);
            return task.Task.Result;

        }

        /// <summary>
        /// 异步执行api，主用用在popmessage上，当queue的长轮询等待时间大于0时可以非阻塞执行代码
        /// </summary>
        /// <typeparam name="TR"></typeparam>
        /// <param name="method"></param>
        /// <param name="resource"></param>
        /// <param name="headers"></param>
        /// <param name="callBack"></param>
        /// <param name="input"></param>
        internal void ExecuteAsync<TR>(Method method, string resource, Dictionary<string, string> headers, Action<TR> callBack, MNSRequest input = null) where TR : MNSResponse, new()
        {
            //var restClient = new RestClient(this.url);
            var request = new RestRequest(resource, this.Map(method))
            {
                RequestFormat = DataFormat.Xml
            };
            this.RequestInit(method,request, headers, input);

            restClient.ExecuteAsync<TR>(request, IRR => {
                callBack(IRR.Data);
            });
        }

        /// <summary>
        /// 初始化参数与头部信息 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="headers"></param>
        /// <param name="input"></param>
        private void RequestInit(Method method,RestRequest request, Dictionary<string, string> headers, MNSRequest input)
        {
            if (!headers.ContainsKey(HeaderConst.HOST))
            {
                headers[HeaderConst.HOST] = this.host;
            }
            if (!headers.ContainsKey(HeaderConst.DATE))
            {
                headers[HeaderConst.DATE] = DateTime.Now.ToUniversalTime().ToString("r");
                headers[HeaderConst.X_MNS_DATE] = DateTime.Now.ToUniversalTime().ToString("r");


            }
            if (!headers.ContainsKey(HeaderConst.MQVERSION))
            {
                headers[HeaderConst.MQVERSION] = this.version;
            }

            //如果request不为空，需指定content-type,否则签名无法通过
            if (input != null)
            {
                headers[HeaderConst.CONTENTTYPE] = "text/xml";
            }

            //headers[HeaderConst.CONTENTMD5]  =m
            headers[HeaderConst.AUTHORIZATION] = Authorization(method, headers, request.Resource);
            foreach (var kv in headers)
            {
                request.AddHeader(kv.Key, kv.Value);
            }

            if (input != null)
            {
                request.AddBody(input);
            }
            
        }

        public MNSQueue GetQueue(string name)
        {
            var queue = new MNSQueue();
            queue.SetClient(this);
            queue.SetName(name);
            return queue;
        }

        private RestSharp.Method Map(Method method)
        {
            switch (method)
            {
                case Method.POST:
                    return RestSharp.Method.POST;
                case Method.PUT:
                    return RestSharp.Method.PUT;
                case Method.DELETE:
                    return RestSharp.Method.DELETE;
                case Method.GET:
                    return RestSharp.Method.GET;
                default:
                    return RestSharp.Method.GET;
            }
        }

        /// <summary>
        /// 生成验证信息
        /// </summary>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        private string Authorization(Method method, Dictionary<string, string> headers, string resource)
        {
            return string.Format("MNS {0}:{1}", this.accessKeyId, this.Signature(method, headers, resource));
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        private string Signature(Method method, Dictionary<string, string> headers, string resource)
        {
            List<string> toSign = new List<string>
            {
                method.ToString(),
                headers.ContainsKey(HeaderConst.CONTENTMD5) ? headers[HeaderConst.CONTENTMD5] : string.Empty,
                headers.ContainsKey(HeaderConst.CONTENTTYPE) ? headers[HeaderConst.CONTENTTYPE] : string.Empty,
                headers.ContainsKey(HeaderConst.DATE) ? headers[HeaderConst.DATE] : DateTime.Now.ToUniversalTime().ToString("r")
            };
            foreach (KeyValuePair<string, string> header in headers.Where(kv => kv.Key.StartsWith("x-mns")).OrderBy(kv => kv.Key))
            {
                toSign.Add(string.Format("{0}:{1}", header.Key, header.Value));
            }

            toSign.Add(resource);

            HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(this.accessKeySecret));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(string.Join("\n", toSign)));
            return Convert.ToBase64String(hashBytes);
        }

        public enum Method
        {
            GET,
            PUT,
            POST,
            DELETE
        }

        public class HeaderConst
        {
            public const string AUTHORIZATION = "Authorization";
            public const string CONTENTTYPE = "Content-Type";
            public const string CONTENTMD5 = "Content-MD5";
            public const string MQVERSION = "x-mns-version";
            public const string HOST = "Host";
            public const string DATE = "Date";
            public const string KEEPALIVE = "Keep-Alive";
            public const string X_MNS_DATE = "x-mns-date";
        }
    }
}
