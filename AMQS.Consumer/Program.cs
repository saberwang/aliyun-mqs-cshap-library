using System;
using System.Threading;
using System.Threading.Tasks;

namespace AMQS.Consumer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Press any key to exit.");

            Task.Factory.StartNew(ReceiveMessage);
            Task.Factory.StartNew(ReceiveMessage);
            Task.Factory.StartNew(ReceiveMessage);

            Console.ReadKey();
        }

        private static void ReceiveMessage()
        {
            var mqClient =
                new MQSClient(string.Format("http://{0}.mqs-cn-hangzhou.aliyuncs.com", AliConfig.queueownerId),
                    AliConfig.accessKeyId, AliConfig.accessKeySecret);

            while (true)
            {
                try
                {
                    MQSQueue queue = mqClient.getQueue("testmq1");
                    MessageReceiveResponse message = queue.popMessage();

                    if (!string.IsNullOrWhiteSpace(message.Code)) //有错误
                    {
                        Console.WriteLine(message.Message);
                        continue;
                    }

                    Console.WriteLine("Receive mesaage : {0}", message.MessageBody);
                    queue.deleteMessage(message.ReceiptHandle);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(3000);
                }
            }
        }
    }
}