namespace SQS.Web.Models
{
    public class MessageInfo
    {
        public string Body { get; set; }
        public string ReceiptHandle { get; set; } // This property can be used to delete a specific message
      
        public int MessageSize { get; set; }

    }
}
