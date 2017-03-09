using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AliMNS
{
    public class MessageReceiveResponse : MNSResponse
    {
        public string MessageId { set; get; }

        public string ReceiptHandle { set; get; }

        public string MessageBodyMD5{set;get;}

        public string MessageBody{set;get;}

        public long EnqueueTime { set; get; }

        public long NextVisibleTime { set; get; }

        public long FirstDequeueTime { set; get; }

        public long DequeueCount { set; get; }

        public int Priority { set; get; }
    }
}
