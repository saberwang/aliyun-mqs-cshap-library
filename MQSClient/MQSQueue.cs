using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMQS
{
    public class MQSQueue
    {
        private string name;
        private MQSClient client;

        internal MQSQueue()
        { }

        internal void setName(string name)
        {
            this.name = name;
        }

        internal void setClient(MQSClient client)
        {
            this.client = client;
        }

        public bool setAttribute(int visibilityTimeout = 60, int maximumMessageSize = 65536, int messageRetentionPeriod = 3600, int delaySeconds = 0, int pollingWaitSeconds = 0)
        {
            var response = this.client.execute<NoContentResponse>(MQSClient.Method.PUT, string.Format("{0}?Metaoverride=true", this.name), new Dictionary<string, string>(), new QueueAttributeSetRequest() { VisibilityTimeout = visibilityTimeout, MaximumMessageSize = maximumMessageSize, DelaySeconds = delaySeconds, MessageRetentionPeriod = messageRetentionPeriod, PollingWaitSeconds = 0 });

            return response == null;
        }

        public QueueAttributeGetResponse getAttribute()
        {
            return this.client.execute<QueueAttributeGetResponse>(MQSClient.Method.GET, this.name, new Dictionary<string, string>());
        }

        public MessageSendResponse sendMessage(string message, int delaySeconds = 0, int priority = 8)
        {
            return this.client.execute<MessageSendResponse>(MQSClient.Method.POST, string.Format("{0}/{1}", this.name, "messages"), new Dictionary<string, string>(), new MessageSendRequest() { MessageBody = message, DelaySeconds = delaySeconds, Priority = priority });
        }

        public MessageReceiveResponse popMessage()
        {
            return this.client.execute<MessageReceiveResponse>(MQSClient.Method.GET, string.Format("{0}/{1}", this.name, "messages"), new Dictionary<string, string>());
        }

        public void popMessageAsync(Action<MessageReceiveResponse> callBack)
        {
            this.client.executeAsync<MessageReceiveResponse>(MQSClient.Method.GET, string.Format("{0}/{1}", this.name, "messages"), new Dictionary<string,string>(), callBack);
        }

        public MessageReceiveResponse peekMessage()
        {
            return this.client.execute<MessageReceiveResponse>(MQSClient.Method.GET, string.Format("{0}/{1}?peekonly=true", this.name, "messages"), new Dictionary<string, string>());
        }

        public MessageVisibilityChangeResponse changeVisibility(string receiptHandle, int visibilityTimeOut)
        {
            return this.client.execute<MessageVisibilityChangeResponse>(MQSClient.Method.PUT, string.Format("{0}/{1}?ReceiptHandle={2}&VisibilityTimeout={3}", this.name, "messages", receiptHandle, visibilityTimeOut), new Dictionary<string, string>());
        }

        public NoContentResponse deleteMessage(string receiptHandle)
        {
            return this.client.execute<NoContentResponse>(MQSClient.Method.DELETE, string.Format("{0}/{1}?ReceiptHandle={2}", this.name, "messages", receiptHandle), new Dictionary<string, string>());
            //return response == null; 
        }
    }
}
