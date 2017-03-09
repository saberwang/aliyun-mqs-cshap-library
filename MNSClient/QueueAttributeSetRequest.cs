using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp.Serializers;
using System.Xml.Serialization;

namespace AliMNS
{
    
   
    [SerializeAs( Name = "Queue")]
    [XmlRoot("Queue")]
    
    public class Queue :  Attribute ,MNSRequest
    {
        public int VisibilityTimeout { set; get; }

        public int MaximumMessageSize { set; get; }

        public int MessageRetentionPeriod { set; get; }

        public int DelaySeconds { set; get; }

        public int PollingWaitSeconds { set; get; }
    }
}
