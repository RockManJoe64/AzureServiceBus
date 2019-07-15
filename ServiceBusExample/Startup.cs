using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ServiceBusExample
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();

            //builder.Services.AddSingleton<QueueClient>((s) => {
            //    var connectionString = Environment.GetEnvironmentVariable("ServiceBusConnection");

            //    var queueName = "basicqueue";

            //    return new QueueClient(connectionString, queueName);
            //});
        }
    }
}
