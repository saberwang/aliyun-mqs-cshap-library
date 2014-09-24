aliyun-mqs-cshap-library
========================

阿里云mqs开发包c#版


基于RestSharp开发，支持异步接受消息


var mqClient = new MQSClient("http://{queueownerId}.mqs-cn-hangzhou.aliyuncs.com", "{accessKeyId}", "{accessKeySecret}");
var queue = mqClient.getQueue("{queueName}");

//发送消息
