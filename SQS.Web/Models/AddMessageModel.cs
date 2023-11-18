using Autofac;
using SQS.Web.QueueService;
using System.ComponentModel.DataAnnotations;

namespace SQS.Web.Models
{
    public class AddMessageModel
    {
        [Required]
        public string Message { get; set; }

        public AddMessageModel()
        {
            
        }

        public void AddMessage(string message,string QueueURL)
        {

        }

    }
}
