using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Cosmos;
using LearnSomeAzure.Core.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace LearnSomeAzure.FunctionApp
{
    public static class MyTopic
    {
       
        [FunctionName("MyTopic")]
        public static async Task RunAsync(
            [ServiceBusTrigger(
                "orders", 
                "EuOrders",
                Connection = "ServiceBus")]
            string mySbMsg, ILogger log)
        {
            try
            {
                log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
                var key = "mWHXOfaiTKuG7neqqWN7zAhzsjVit9WnpCCzXOWCEwXlsXGcisE2aZWBC8DXmuKfi7oaJKMXuw7p1GXBxK3QAA==";
                var endpointUrl = @"https://lowrancecosmos.documents.azure.com/";
                var client = new CosmosClient(endpointUrl, key);
                var container = client.GetContainer("LearnAzure", "items");
                var order = JsonSerializer.Deserialize<Order>(mySbMsg);
                log.LogInformation($"Posting item: {order.Partition}");
                var response =
                    await container.CreateItemAsync<Order>(order, new PartitionKey(order.OrderId.ToString()));
                log.LogInformation($"Added order to db: {response.ETag}");
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
            }
            
        }

        

        private static async Task OldVersion(ILogger log, string  mySbMsg)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

            var cxString = System.Environment.GetEnvironmentVariable("ServiceBus");
            log.LogInformation("Got cx string: " + cxString);
            
            var sender =
                new TopicSender(
                    "Endpoint=sb://lowrance.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=/faLtKBZOsvmB2m73wvBnvcm8dsRcmsHN2GCY7IbwxQ=",
                    "orders");
            
            var order = JsonSerializer.Deserialize<Order>(mySbMsg);
            order.Region = "US2";

            await sender.SendOrderMessage(order);
        }
    }
}