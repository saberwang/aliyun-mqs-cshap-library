using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMQS
{
    public class MessageVisibilityChangeResponse : MQSResponse
    {
        public string ReceiptHandle { set; get; }
        public long NextVisibleTime { set; get; }
    }
}
