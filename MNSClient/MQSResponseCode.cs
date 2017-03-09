using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMQS
{
    public class MQSResponseCode
    {
        public const string  AccessDenied = "AccessDenied";
        public const string InvalidAccessKeyId = "InvalidAccessKeyId";
        public const string InternalError = "InternalError";
        public const string InvalidAuthorizationHeader = "InvalidAuthorizationHeader";
        public const string InvalidDateHeader = "InvalidDateHeader";
        public const string InvalidArgument = "InvalidArgument";
        public const string InvalidDegist = "InvalidDegist";
        public const string InvalidRequestURL = "InvalidRequestURL";
        public const string InvalidQueryString = "InvalidQueryString";
        public const string MalformedXML = "MalformedXML";
        public const string MissingAuthorizationHeader = "MissingAuthorizationHeader";
        public const string MissingDateHeader = "MissingDateHeader";
        public const string MissingVersionHeader = "MissingVersionHeader";
        public const string MissingReceiptHandle = "MissingReceiptHandle";
        public const string MissingVisibilityTimeout = "MissingVisibilityTimeout";
        public const string MessageNotExist = "MessageNotExist";
        public const string QueueAlreadyExist = "QueueAlreadyExist";
        public const string InvalidQueueName = "InvalidQueueName";
        public const string InvalidVersionHeader = "InvalidVersionHeader";
        public const string InvalidContentType = "InvalidContentType";
        public const string QueueNameLengthError = "QueueNameLengthError";
        public const string QueueNotExist = "QueueNotExist";
        public const string ReceiptHandleError = "ReceiptHandleError";
        public const string SignatureDoesNotMatch = "SignatureDoesNotMatch";
        public const string TimeExpired = "TimeExpired";
    }
}
