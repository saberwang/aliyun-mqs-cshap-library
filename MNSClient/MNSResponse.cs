using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliMNS
{
    public abstract class MNSResponse
    {
        public string Code { set; get; } //错误码 

        public string Message { set; get; } //错误消息

        public string RequestId { set; get; } //请求id

        public string HostId { set; get; } 
    }
}
