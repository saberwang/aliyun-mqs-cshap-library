aliyun-mqs-cshap-library
========================

阿里云mqs开发包c#版


基于RestSharp开发，支持异步接受消息


var mqClient = new MQSClient("http://{queueownerId}.mqs-cn-hangzhou.aliyuncs.com", "{accessKeyId}", "{accessKeySecret}");

var queue = mqClient.getQueue("{queueName}");

//发送消息
queue.sendMessage("{messageBody}");

//接受消息-同步版

MessageReceiveResponse message = queue.popMessage();

//接受消息——异步版

queue.popMessageAsync(message => {
  
}）；

//删除消息
queue.deleteMessage("{receiptHandle}");

//peek message
MessageReceiveResponse message = queue.peekMessage();




提供的api还有设置队列属性，获取队列属性,改变消息可见时间
