using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace LearnSomeAzure.Core.ServiceBus
{
    public static class ServiceBusUtils
    {
        public static Message AsServiceBusMessage<T>(this T item)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(item);
            var bytes = new Message(Encoding.UTF8.GetBytes(json));
            return bytes;
        }
    }

    public class TopicSender
    {
        private readonly TopicClient _client;

        public TopicSender(string connectionString, string topicPath)
        {
            _client = new TopicClient(connectionString, topicPath);
        }

        public async Task SendOrderMessage(Order order)
        {
            var message = order.AsServiceBusMessage();
            message.UserProperties.Add(nameof(order.Region), order.Region);
            message.UserProperties.Add(nameof(order.Value), order.Value);
            message.UserProperties.Add(nameof(order.Items), order.Items);

            message.CorrelationId = order.Region;

            await _client.SendAsync(message);
        }

        public async Task Close()
        {
            await _client.CloseAsync();
        }
    }
    
    public class Manager
    {
        private ManagementClient _managementClient;


        public Manager(string connectionString)
        {
            _managementClient = new ManagementClient(connectionString);
        }
        
        
         public async Task<TopicDescription> CreateTopic(string topicPath)
        {
            Console.WriteLine($"Creating Topic { topicPath }");



            if (await _managementClient.TopicExistsAsync(topicPath))
            {
                await _managementClient.DeleteTopicAsync(topicPath);
            }

            return await _managementClient.CreateTopicAsync(topicPath);
        }
         
        public async Task<SubscriptionDescription> CreateSubscription(string topicPath, string subscriptionName)
        {
            Console.WriteLine($"Creating Subscription { topicPath }/{ subscriptionName }");

            return await _managementClient.CreateSubscriptionAsync(topicPath, subscriptionName);
        }

        public async Task<SubscriptionDescription> CreateSubscriptionWithSqlFilter(string topicPath, string subscriptionName, string sqlExpression)
        {
            Console.WriteLine($"Creating Subscription with SQL Filter{ topicPath }/{ subscriptionName } ({ sqlExpression })");



            var subscriptionDescription = new SubscriptionDescription(topicPath, subscriptionName);

            var ruleDescription = new RuleDescription("Default", new SqlFilter(sqlExpression));

            return await _managementClient.CreateSubscriptionAsync(subscriptionDescription, ruleDescription);
        }

        public async Task<SubscriptionDescription> CreateSubscriptionWithCorrelationFilter(string topicPath, string subscriptionName, string correlationId)
        {
            Console.WriteLine($"Creating Subscription with Correlation Filter{ topicPath }/{ subscriptionName } ({ correlationId })");

            

            var subscriptionDescription = new SubscriptionDescription(topicPath, subscriptionName);

            var ruleDescription = new RuleDescription("Default", new CorrelationFilter(correlationId));

            return await _managementClient.CreateSubscriptionAsync(subscriptionDescription, ruleDescription);
        }

        public async Task<IList<SubscriptionDescription>> GetSubscriptionsForTopic(string topicPath)
        {
            return await _managementClient.GetSubscriptionsAsync(topicPath);
        }
        
    }

    public class Client
    {
        
    }
}