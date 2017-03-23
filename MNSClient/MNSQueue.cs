using System;
using System.Collections.Generic;

namespace AliMNS
{
    public class MNSQueue
    {
        private MNSClient client;
        private string name;

        internal MNSQueue()
        {
        }

        internal void SetName(string name)
        {
            this.name = name;
        }

        internal void SetClient(MNSClient client)
        {
            this.client = client;
        }

        public bool SetAttribute(int visibilityTimeout = 30, int maximumMessageSize = 65536,
            int messageRetentionPeriod = 3600, int delaySeconds = 0, int pollingWaitSeconds = 0, bool overwrite = false)
        {
            var response = client.Execute<NoContentResponse>(MNSClient.Method.PUT,
                string.Format("/queues/{0}{1}", name, overwrite ? "?metaOverride=true" : ""), new Dictionary<string, string>(),
                new Queue
                {
                    VisibilityTimeout = visibilityTimeout,
                    MaximumMessageSize = maximumMessageSize,
                    DelaySeconds = delaySeconds,
                    MessageRetentionPeriod = messageRetentionPeriod,
                    PollingWaitSeconds = pollingWaitSeconds
                });

            return response == null;
        }

        public QueueAttributeGetResponse GetAttribute()
        {
            return client.Execute<QueueAttributeGetResponse>(MNSClient.Method.GET, name,
                new Dictionary<string, string>());
        }

        public bool CreateQueue(int visibilityTimeout = 60, int maximumMessageSize = 65536,
            int messageRetentionPeriod = 3600, int delaySeconds = 0, int pollingWaitSeconds = 0)
        {
            var response = SetAttribute(visibilityTimeout, maximumMessageSize, messageRetentionPeriod, delaySeconds,
                pollingWaitSeconds, true);

            return response;
        }

        public MessageSendResponse SendMessage(string message, int delaySeconds = 0, int priority = 8)
        {
            return client.Execute<MessageSendResponse>(MNSClient.Method.POST, string.Format("/queues/{0}/{1}", name, "messages"),
                new Dictionary<string, string>(),
                new Message {MessageBody = message, DelaySeconds = delaySeconds, Priority = priority});
        }

        public MessageReceiveResponse PopMessage()
        {
            return client.Execute<MessageReceiveResponse>(MNSClient.Method.GET,
                string.Format("/queues/{0}/{1}", name, "messages"), new Dictionary<string, string>());
        }

        public void PopMessageAsync(Action<MessageReceiveResponse> callBack)
        {
            client.ExecuteAsync(MNSClient.Method.GET, string.Format("/queues/{0}/{1}", name, "messages"),
                new Dictionary<string, string>(), callBack);
        }

        public MessageReceiveResponse PeekMessage()
        {
            return client.Execute<MessageReceiveResponse>(MNSClient.Method.GET,
                string.Format("/queues/{0}/{1}?peekonly=true", name, "messages"), new Dictionary<string, string>());
        }
        public MessageReceiveResponse ReceiveMessage(int waitseconds=10)
        {
            return client.Execute<MessageReceiveResponse>(MNSClient.Method.GET,
                string.Format("/queues/{0}/{1}?waitseconds={2}", name, "messages", waitseconds), new Dictionary<string, string>());
        }

        public MessageVisibilityChangeResponse ChangeVisibility(string receiptHandle, int visibilityTimeOut)
        {
            return client.Execute<MessageVisibilityChangeResponse>(MNSClient.Method.PUT,
                string.Format("/queues/{0}/{1}?ReceiptHandle={2}&VisibilityTimeout={3}", name, "messages", receiptHandle,
                    visibilityTimeOut), new Dictionary<string, string>());
        }

        public NoContentResponse DeleteMessage(string receiptHandle)
        {
            return client.Execute<NoContentResponse>(MNSClient.Method.DELETE,
                string.Format("/queues/{0}/{1}?ReceiptHandle={2}", name, "messages", receiptHandle),
                new Dictionary<string, string>());
            //return response == null; 
        }
    }
}