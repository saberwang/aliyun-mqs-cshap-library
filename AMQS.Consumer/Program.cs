using System;
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
                MQSQueue queue = mqClient.getQueue("testmq1");
                MessageReceiveResponse message = queue.popMessage();

                if (!string.IsNullOrWhiteSpace(message.Code)) //有错误
                {
                    Console.WriteLine(message.Message);
                    continue;
                }

                Console.WriteLine(message.MessageBody);
                queue.deleteMessage(message.ReceiptHandle);
            }
        }
    }
}