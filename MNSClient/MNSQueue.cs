using System;
using System.Collections.Generic;

namespace AliMNS
{
    public class MNSQueue
    {
        private MQNSClient client;
        private string name;

        internal MNSQueue()
        {
        }

        internal void setName(string name)
        {
            this.name = name;
        }

        internal void setClient(MQNSClient client)
        {
            this.client = client;
        }

        public bool setAttribute(int visibilityTimeout = 60, int maximumMessageSize = 65536,
            int messageRetentionPeriod = 3600, int delaySeconds = 0, int pollingWaitSeconds = 0, bool overwrite = false)
        {
            var response = client.execute<NoContentResponse>(MQNSClient.Method.PUT,
                string.Format("/queues/{0}{1}", name, overwrite ? "?metaOverride=true" : ""), new Dictionary<string, string>(),
                new QueueAttributeSetRequest
                {
                    VisibilityTimeout = visibilityTimeout,
                    MaximumMessageSize = maximumMessageSize,
                    DelaySeconds = delaySeconds,
                    MessageRetentionPeriod = messageRetentionPeriod,
                    PollingWaitSeconds = pollingWaitSeconds
                });

            return response == null;
        }

        public QueueAttributeGetResponse getAttribute()
        {
            return client.execute<QueueAttributeGetResponse>(MQNSClient.Method.GET, name,
                new Dictionary<string, string>());
        }

        public bool CreateQueue(int visibilityTimeout = 60, int maximumMessageSize = 65536,
            int messageRetentionPeriod = 3600, int delaySeconds = 0, int pollingWaitSeconds = 0)
        {
            var response = setAttribute(visibilityTimeout, maximumMessageSize, messageRetentionPeriod, delaySeconds,
                pollingWaitSeconds, true);

            return response;
        }

        public MessageSendResponse sendMessage(string message, int delaySeconds = 0, int priority = 8)
        {
            return client.execute<MessageSendResponse>(MQNSClient.Method.POST, string.Format("/queues/{0}/{1}", name, "messages"),
                new Dictionary<string, string>(),
                new MessageSendRequest {MessageBody = message, DelaySeconds = delaySeconds, Priority = priority});
        }

        public MessageReceiveResponse popMessage()
        {
            return client.execute<MessageReceiveResponse>(MQNSClient.Method.GET,
                string.Format("/queues/{0}/{1}", name, "messages"), new Dictionary<string, string>());
        }

        public void popMessageAsync(Action<MessageReceiveResponse> callBack)
        {
            client.executeAsync(MQNSClient.Method.GET, string.Format("/queues/{0}/{1}", name, "messages"),
                new Dictionary<string, string>(), callBack);
        }

        public MessageReceiveResponse peekMessage()
        {
            return client.execute<MessageReceiveResponse>(MQNSClient.Method.GET,
                string.Format("/queues/{0}/{1}?peekonly=true", name, "messages"), new Dictionary<string, string>());
        }

        public MessageVisibilityChangeResponse changeVisibility(string receiptHandle, int visibilityTimeOut)
        {
            return client.execute<MessageVisibilityChangeResponse>(MQNSClient.Method.PUT,
                string.Format("/queues/{0}/{1}?ReceiptHandle={2}&VisibilityTimeout={3}", name, "messages", receiptHandle,
                    visibilityTimeOut), new Dictionary<string, string>());
        }

        public NoContentResponse deleteMessage(string receiptHandle)
        {
            return client.execute<NoContentResponse>(MQNSClient.Method.DELETE,
                string.Format("/queues/{0}/{1}?ReceiptHandle={2}", name, "messages", receiptHandle),
                new Dictionary<string, string>());
            //return response == null; 
        }
    }
}