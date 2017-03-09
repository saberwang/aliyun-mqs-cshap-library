using System;
using System.Threading;
using System.Threading.Tasks;

namespace AliMNS.Consumer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Press any key to exit.");
            AliConfig.AccessKey = args[0];
            AliConfig.AccessKeySecret = args[1];
            AliConfig.Endpoint = args[2];
            Task.Factory.StartNew(ReceiveMessage);
            Task.Factory.StartNew(ReceiveMessage);
            Task.Factory.StartNew(ReceiveMessage);

            Console.ReadKey();
        }

        private static void ReceiveMessage()
        {

            var mqClient =
                new MQNSClient(AliConfig.Endpoint ,
                    AliConfig.AccessKey, AliConfig.AccessKeySecret);

            while (true)
            {
                try
                {
                    MNSQueue queue = mqClient.getQueue("toll-open");
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