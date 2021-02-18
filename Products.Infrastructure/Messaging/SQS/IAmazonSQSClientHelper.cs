using Amazon.SQS.Model;

namespace Products.Infraestructure.Messaging.SQS
{
    public interface IAmazonSqsClientHelper
    {
        SendMessageResponse SendMessageAsync(string queueName, string messageBody);
    }
}
