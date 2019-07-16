# AzureServiceBus

This is an example of Azure Functions interacting with a ServiceBus queue

# Config Setup

You will have to add a `local.settings.json` file to the ServiceBusExample project like so:

```
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "ServiceBusConnection": "<YOUR-SERVICEBUS-ENDPOINT-URL>"
  }
}
```
