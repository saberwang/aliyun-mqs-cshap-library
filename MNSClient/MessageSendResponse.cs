using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliMNS
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MessageSendResponse : MNSResponse
    {
        public string MessageId { set; get; }

        public string MessageBodyMD5 { set; get; }
    }
}
