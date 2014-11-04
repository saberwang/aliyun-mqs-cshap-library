using System;
using System.Threading;
using System.Threading.Tasks;
using AMQS.Consumer;

namespace AMQS.Productor
{
    internal class Program
    {
        private static void Main(string[] args)
        {
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
                    new MQSClient(string.Format("http://{0}.mqs-cn-hangzhou.aliyuncs.com", AliConfig.queueownerId),
                        AliConfig.accessKeyId, AliConfig.accessKeySecret);

                    MQSQueue queue = mqClient.getQueue("testmq1");
                    string message = string.Format("Hello World！ <from {0}, No.{1}>", Thread.CurrentThread.ManagedThreadId,
                        i);
                    queue.sendMessage(message);

                    Console.WriteLine("Send message : {0}", message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}