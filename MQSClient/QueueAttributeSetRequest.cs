using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp.Serializers;
namespace AMQS
{
    [SerializeAs(Name="Queue")]
    public class QueueAttributeSetRequest : MQSRequest
    {
        public int VisibilityTimeout { set; get; }
        public int MaximumMessageSize { set; get; }
        public int MessageRetentionPeriod { set; get; }
        public int DelaySeconds { set; get; }
        public int PollingWaitSeconds { set; get; }
    }
}
