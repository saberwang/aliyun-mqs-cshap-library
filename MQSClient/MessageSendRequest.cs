using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using RestSharp.Serializers;
namespace AMQS
{
    [SerializeAs(Name="Message")]
    public class MessageSendRequest : MQSRequest
    {
        public string MessageBody { set; get; }
        public int DelaySeconds { set; get; }
        public int Priority { set; get; }
    }
}
