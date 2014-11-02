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
            Console.WriteLine("Press any key to exit.");

            Task.Factory.StartNew(() => SendMessage());
            Task.Factory.StartNew(() => SendMessage());
            Task.Factory.StartNew(() => SendMessage());

            Console.ReadKey();
        }

        private static void SendMessage()
        {
            for (int i = 0; i < 20; i++)
            {
                var mqClient =
                    new MQSClient(string.Format("http://{0}.mqs-cn-hangzhou.aliyuncs.com", AliConfig.queueownerId),
                        AliConfig.accessKeyId, AliConfig.accessKeySecret);

                MQSQueue queue = mqClient.getQueue("testmq1");
                string message = string.Format("Hello World！ <from {0}, No.{1}>", Thread.CurrentThread.ManagedThreadId,
                    i);
                queue.sendMessage(message);

                Console.WriteLine(message);
            }
        }
    }
}