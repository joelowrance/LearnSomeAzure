using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace LearnSomeAzure.FunctionApp
{
    public static class OnTime
    {
        [Microsoft.Azure.WebJobs.Disable]
        [FunctionName("OnTime")]
        public static async Task RunAsync([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");
            
        }
    }
}