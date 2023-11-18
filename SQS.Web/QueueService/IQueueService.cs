using SQS.Web.Models;

namespace SQS.Web.QueueService
{
    public interface IQueueService
    {
        Task SendMessageToSqsQueue(string messageBody, string QueueURL);

        Task<List<MessageInfo>> ReadMessagesFromSqsQueue(string QueueURL, int numMessagesToRead);

        Task DeleteMessageFromSqsQueue(string QueueURL, string receiptHandle);

        Task<int> GetMessageCountInSqsQueue(string QueueURL);
    }
}
