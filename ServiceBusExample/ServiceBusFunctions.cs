using System.Text;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using ServiceBusCommon;

namespace ServiceBusExample
{
    public class ServiceBusFunctions
    {
        public const string QUEUE_NAME = "basicqueue";

        [FunctionName("GetMessage")]
        public void GetMessage(
            [ServiceBusTrigger(QUEUE_NAME, Connection = "ServiceBusConnection")] Message queuedMessage, 
            ILogger log)
        {
            var jsonPayload = Encoding.UTF8.GetString(queuedMessage.Body);

            log.LogInformation($"C# ServiceBus queue trigger function processed message: {jsonPayload}");
        }
    }
}
