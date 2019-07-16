using System;
using System.Net;
using System.Net.Http;

using AutoFixture;
using FluentAssertions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using ServiceBusCommon;
using Xunit;

namespace ServiceBusExampleTests
{
    public class SendMessageTests
    {
        private const uint MAX_DATA_COUNT = 1024;

        private const string URL = "http://localhost:7071/api/sendmessage";

        private HttpClient httpClient;

        public SendMessageTests()
        {
            httpClient = new HttpClient();
        }

        [Fact]
        public async void SendSingleMessage()
        {
            var fixture = new Fixture();

            var person = new Person
            {
                FullName = fixture.Create<string>(),
                Location = fixture.Create<string>()
            };

            var response = await httpClient.PostAsJsonAsync(URL, person);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async void SendMultipleMessage()
        {
            var fixture = new Fixture();

            for (int index = 0; index < MAX_DATA_COUNT; index++)
            {
                var person = new Person
                {
                    FullName = fixture.Create<string>(),
                    Location = fixture.Create<string>()
                };

                //var response = await httpClient.PostAsJsonAsync(URL, person);

                //response.StatusCode.Should().Be(HttpStatusCode.OK);

                await httpClient.PostAsJsonAsync(URL, person);
            }
        }

        [Fact]
        public async void ClearDeadLetterQueue()
        {
            var connectionString = "Endpoint=sb://servicebusns-5003.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ZZCtmtN7XUHmleZwENlBkkaI/B96E5ytNF7OPD35E40=";

            var deadLetterQueueName = EntityNameHelper.FormatDeadLetterPath(ServiceBusExample.ServiceBusFunctions.QUEUE_NAME);

            var deadletterReceiver = new MessageReceiver(connectionString, deadLetterQueueName, ReceiveMode.PeekLock);

            var receivedMessage = await deadletterReceiver.ReceiveAsync(TimeSpan.FromSeconds(10));

            while (receivedMessage != null)
            {
                await deadletterReceiver.CompleteAsync(receivedMessage.SystemProperties.LockToken);

                receivedMessage = await deadletterReceiver.ReceiveAsync(TimeSpan.FromSeconds(10));
            }
        }
    }
}
