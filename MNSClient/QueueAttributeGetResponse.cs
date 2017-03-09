using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AliMNS
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QueueAttributeGetResponse : MNSResponse
    {
        public int ActiveMessages { set; get; }
        public string CreateTime { set; get; }
        public int DelayMessages { set; get; }
        public int InactiveMessages { set; get; }
        public string LastModifyTime { set; get; }
        public int MaximumMessageSize { set; get; }
        public string MessageRetentionPeriod { set; get; }
        public int PollingWaitSeconds { set; get; }
        public string QueueName { set; get; }
        public string QueueStatus { set; get; }
        public int VisibilityTimeout { set; get; }
    }
}
