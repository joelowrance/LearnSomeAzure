using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace LearnSomeAzure.FunctionApp
{
    public static class MyTopic
    {
        [FunctionName("MyTopic")]
        public static async Task RunAsync([ServiceBusTrigger("mytopic", "mysubscription", Connection = "")]
            string mySbMsg, ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            
        }
    }
}