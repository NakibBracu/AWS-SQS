using Amazon.Runtime.Internal;
using Amazon.SQS;
using Amazon.SQS.Model;
using SQS.Web.Models;
namespace SQS.Web.QueueService
{
    public class QueueService:IQueueService
    {
        public async Task SendMessageToSqsQueue(string messageBody,string QueueURL)
        {
            var queueUrl = QueueURL;
            try
            {
                var config = new AmazonSQSConfig
                {
                    RegionEndpoint = Amazon.RegionEndpoint.USEast1
                };

                using (var client = new AmazonSQSClient(config))
                {
                    var request = new SendMessageRequest
                    {
                        QueueUrl = queueUrl,
                        MessageBody = messageBody
                    };

                    await client.SendMessageAsync(request);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or log errors as needed
                // You can also return an error view or message here if needed
                Console.WriteLine("Error sending message to SQS: " + ex.Message);
            }
        }


        public async Task<List<MessageInfo>> ReadMessagesFromSqsQueue(string QueueURL, int numMessagesToRead)
        {
            var queueUrl = QueueURL;
            try
            {
                var config = new AmazonSQSConfig
                {
                    RegionEndpoint = Amazon.RegionEndpoint.USEast1
                };

                using (var client = new AmazonSQSClient(config))
                {
                    var request = new ReceiveMessageRequest
                    {
                        QueueUrl = queueUrl,
                        MaxNumberOfMessages = numMessagesToRead
                    };

                    var response = await client.ReceiveMessageAsync(request);

                    if (response.Messages.Count > 0)
                    {
                        List<MessageInfo> messages = new List<MessageInfo>();
                        foreach (var message in response.Messages)
                        {
                            // Create a custom MessageInfo object to store message properties
                            var messageInfo = new MessageInfo
                            {
                                Body = message.Body,
                                ReceiptHandle = message.ReceiptHandle,        
                                MessageSize = message.Body.Length, 
                            };

                            messages.Add(messageInfo);
                        }

                        return messages;
                    }
                    else
                    {
                        // No messages available in the queue
                        return new List<MessageInfo>();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or log errors as needed
                Console.WriteLine("Error reading messages from SQS: " + ex.Message);
                return null;
            }
        }

     



        public async Task DeleteMessageFromSqsQueue(string QueueURL, string receiptHandle)
        {
            var queueUrl = QueueURL;
            try
            {
                var config = new AmazonSQSConfig
                {
                    RegionEndpoint = Amazon.RegionEndpoint.USEast1
                };

                using (var client = new AmazonSQSClient(config))
                {
                    var deleteRequest = new DeleteMessageRequest
                    {
                        QueueUrl = queueUrl,
                        ReceiptHandle = receiptHandle
                    };

                    await client.DeleteMessageAsync(deleteRequest);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or log errors as needed
                Console.WriteLine("Error deleting message from SQS: " + ex.Message);
            }
        }



        public async Task<int> GetMessageCountInSqsQueue(string QueueURL)
        {
            var queueUrl = QueueURL;
            try
            {
                var config = new AmazonSQSConfig
                {
                    RegionEndpoint = Amazon.RegionEndpoint.USEast1
                };

                using (var client = new AmazonSQSClient(config))
                {
                    var request = new GetQueueAttributesRequest
                    {
                        QueueUrl = queueUrl,
                        AttributeNames = new List<string> { "ApproximateNumberOfMessages" }
                    };

                    var response = await client.GetQueueAttributesAsync(request);
                    if (response.Attributes.TryGetValue("ApproximateNumberOfMessages", out string countString))
                    {
                        if (int.TryParse(countString, out int messageCount))
                        {
                            return messageCount;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or log errors as needed
                Console.WriteLine("Error getting message count from SQS: " + ex.Message);
            }

            // If there was an error or no count was retrieved, return -1 or another suitable default value.
            return -1;
        }

        
    }
}
