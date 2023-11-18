using Amazon.SQS;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SQS.Web.Models;
using SQS.Web.QueueService;
using System.Reflection;
using static System.Formats.Asn1.AsnWriter;

namespace SQS.Web.Controllers
{
    public class SQSController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SQSController> _logger;
        private readonly IQueueService _queueService;

        public SQSController(
            IConfiguration configuration,
            ILogger<SQSController> logger,
            IQueueService queueService
        )
        {
            _configuration = configuration;
            _logger = logger;
            _queueService = queueService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddMessageToQueue()
        {
            var model = new AddMessageModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMessageToQueue(AddMessageModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var queueUrl = _configuration["SQSConfig:QueueUrl"];
                   await _queueService.SendMessageToSqsQueue(model.Message, queueUrl);
                }
                catch (Exception ex)
                {
                    // Log the error using the ILogger
                    _logger.LogError(ex, "Error sending message to SQS.");
                    return Content("Failed to send message to SQS queue.");
                }
            }

            return Content("Message Added!");
        }

        public async Task<IActionResult> GetTenMessages()
        {
            try
            {
                var queueUrl = _configuration["SQSConfig:QueueUrl"];
                var messages = await _queueService.ReadMessagesFromSqsQueue(queueUrl, 10);

                return View(messages);
            }
            catch (Exception ex)
            {
                // Handle errors and return appropriate JSON response
                return Content("Problem");
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteReadMessagesFromQueue(string receiptHandle)
        {
            try
            {
                var queueUrl = _configuration["SQSConfig:QueueUrl"];
                await _queueService.DeleteMessageFromSqsQueue(queueUrl, receiptHandle);

                return Json(new { success = true, message = "Message deleted successfully" });
            }
            catch (Exception ex)
            {
                // Log the error using the ILogger
                _logger.LogError(ex, "Error deleting messages from SQS.");
                return Json(new { success = false, message = "An error occurred while deleting the message." });
            }
        }


        public async Task<IActionResult> GetMessageCount()
        {
            try
            {
                var queueUrl = _configuration["SQSConfig:QueueUrl"];
                var totalMessageCount = await _queueService.GetMessageCountInSqsQueue(queueUrl);

                return Content($"The total MessageCount = {totalMessageCount}");
            }
            catch (Exception ex)
            {
                // Log the error using the ILogger
                _logger.LogError(ex, "Error deleting messages from SQS.");
                return Content("Failed to delete messages from SQS queue.");
            }
        }


    }

}
