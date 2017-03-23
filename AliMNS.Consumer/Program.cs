using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AliMNS.Consumer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Press any key to exit.");

#if NETCOREAPP1_1
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
          Console.OutputEncoding =Encoding.GetEncoding(936);
#endif 
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
                new MNSClient(AliConfig.Endpoint,
                    AliConfig.AccessKey, AliConfig.AccessKeySecret);

            while (true)
            {
                try
                {
                    MNSQueue queue = mqClient.GetQueue("toll-open");
                    MessageReceiveResponse message = queue.PopMessage();

                    if (!string.IsNullOrWhiteSpace(message.Code)) //有错误
                    {
                        Console.WriteLine(message.Message);
                        continue;
                    }

                    Console.WriteLine("收到消息Receive mesaage : {0}", message.MessageBody);
                    queue.DeleteMessage(message.ReceiptHandle);
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