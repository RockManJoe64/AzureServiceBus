using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using ServiceBusCommon;

namespace ServiceBusExample
{
    public class PostMessageFunctions
    {
        private QueueClient queueClient;

        public PostMessageFunctions()
        {
            queueClient = CreateQueueClient();
        }

        [FunctionName("SendMessage")]
        public async Task<IActionResult> SendMessage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "sendmessage")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            log.LogInformation($"Received JSON {requestBody}");

            var person = JsonConvert.DeserializeObject<Person>(requestBody);

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var messageBody = JsonConvert.SerializeObject(person);

                    var message = new Message(Encoding.UTF8.GetBytes(messageBody))
                    {
                        ContentType = "application/json",
                        Label = "Person"
                    };

                    await queueClient.SendAsync(message);
                }
                catch (Exception exception)
                {
                    return new BadRequestObjectResult("An error occurred while sending message to post: " + exception.Message);
                }
            }

            return new OkObjectResult($"Hello, {person.FullName}, we sent you to the queue");
        }

        private QueueClient CreateQueueClient()
        {
            var connectionString = Environment.GetEnvironmentVariable("ServiceBusConnection");

            var queueName = "basicqueue";

            return new QueueClient(connectionString, queueName);
        }
    }
}
