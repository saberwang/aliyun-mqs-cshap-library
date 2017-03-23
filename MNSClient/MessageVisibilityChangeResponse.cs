using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliMNS
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MessageVisibilityChangeResponse : MNSResponse
    {
        public string ReceiptHandle { set; get; }
        public long NextVisibleTime { set; get; }
    }
}
