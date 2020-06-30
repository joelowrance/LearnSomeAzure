using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Cosmos;
using LearnSomeAzure.Core.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LearnSomeAzure.FunctionApp
{
    public static class My_Function
    {
        [FunctionName("My_Function")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req, ILogger log)
        {

            var order = new Order()
            {
                Name = "UK Order",
                Value = 49.45,
                Region = "UK",
                Items = 3,
            };
            
            
            var key = "mWHXOfaiTKuG7neqqWN7zAhzsjVit9WnpCCzXOWCEwXlsXGcisE2aZWBC8DXmuKfi7oaJKMXuw7p1GXBxK3QAA";
            var endpointUrl = @"https://lowrancecosmos.documents.azure.com/";
            //endpointUrl += ":";
            //endpointUrl += "//";
            //endpointUrl += "lowrancecosmos.documents.azure.com";
            //endpointUrl += "/";
            var client = new CosmosClient(endpointUrl, key);
            var container = client.GetContainer("LearnAzure", "items");
            var response = await container.CreateItemAsync(order, new PartitionKey(order.OrderId.ToString()));
                    
            
            log.LogInformation("C# HTTP trigger function processed a request.");

            return new OkObjectResult($"Hello, joe");
            //
             string name = req.Query["name"];
            //
             string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
             dynamic data = JsonConvert.DeserializeObject(requestBody);
             name = name ?? data?.name;
            //
             return name != null
                 ? (ActionResult) new OkObjectResult($"Hello, {name}")
                 : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
            
        }
    }
}