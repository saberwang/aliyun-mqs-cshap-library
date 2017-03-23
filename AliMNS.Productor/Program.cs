using System;
using System.Threading;
using System.Threading.Tasks;
 

namespace AliMNS.Productor
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AliConfig.AccessKey = args[0];
            AliConfig.AccessKeySecret = args[1];
            AliConfig.Endpoint = args[2];
            MNSQueue mqsQueue = new MNSClient( AliConfig.Endpoint,
                AliConfig.AccessKey, AliConfig.AccessKeySecret).GetQueue("toll-open");
            mqsQueue.CreateQueue(pollingWaitSeconds: 10);

            Start();
            
            var input = string.Empty;
            while (input != "quit")
            {
                if (input == "send")
                {
                    Start();
                }

                input = Console.ReadLine();
            }
        }

        static void Start()
        {
            Task.Factory.StartNew(() => SendMessage());
            Task.Factory.StartNew(() => SendMessage());
            Task.Factory.StartNew(() => SendMessage());
        }

        private static void SendMessage()
        {
            for (int i = 0; i < 20; i++)
            {
                try
                {
                    var mqClient =
                    new MNSClient( AliConfig.Endpoint,AliConfig.AccessKey, AliConfig.AccessKeySecret);
                    MNSQueue queue = mqClient.GetQueue("toll-open");
                    string message = string.Format("Hello World! <from {0}, No.{1}>", Thread.CurrentThread.ManagedThreadId,
                        i);
                   var result= queue.SendMessage(message);

                    Console.WriteLine("Send message : {0},return code {1}", message,result.Code);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}