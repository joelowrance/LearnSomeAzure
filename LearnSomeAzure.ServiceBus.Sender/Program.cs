using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Azure.Cosmos;
using LearnSomeAzure.Core.ServiceBus;
using Microsoft.Azure.ServiceBus;

namespace LearnSomeAzure.ServiceBus.Sender
{
    
    
    class Program
    {
        static async Task SomethingElse()
        {
            var order = new Order()
            {
                Name = "UK Order",
                Value = 49.45,
                Region = "UK",
                Items = 3,
            };

            var endpointUrl = "";
            CosmosClient client = null;
            
            var key = "mWHXOfaiTKuG7neqqWN7zAhzsjVit9WnpCCzXOWCEwXlsXGcisE2aZWBC8DXmuKfi7oaJKMXuw7p1GXBxK3QAA==";
            try
            {
                endpointUrl = @"https://lowrancecosmos.documents.azure.com/";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            try
            {
                client = new CosmosClient(endpointUrl, key);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            CosmosContainer container = null;
            
            
            try
            {
                container = client.GetContainer("LearnAzure", "items");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            
            try
            {
                var response = await container.CreateItemAsync<Order>(order, new PartitionKey(order.OrderId.ToString()));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            
            
            
            //endpointUrl += ":";
            //endpointUrl += "//";
            //endpointUrl += "lowrancecosmos.documents.azure.com";
            //endpointUrl += "/";
            
             
            
            Console.ReadKey();
        }
        
        static async Task Main(string[] args)
        {
            
            
            Console.WriteLine("Hello World!");
            var connectionString =
                "Endpoint=sb://lowrance.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=/faLtKBZOsvmB2m73wvBnvcm8dsRcmsHN2GCY7IbwxQ=";
            
             var manager = new Manager(connectionString);
            //
            // await manager.CreateTopic("Orders");
            // await manager.CreateSubscription("Orders", "AllOrders");
            // await manager.CreateSubscriptionWithSqlFilter("Orders", "EuOrders", "region = 'EU'");
            // await manager.CreateSubscriptionWithSqlFilter("Orders", "LargeOrders", "items > 30");
            // await manager.CreateSubscriptionWithSqlFilter("Orders", "HighValueOrders", "value > 500");
            // await manager.CreateSubscriptionWithSqlFilter("Orders", "LoyaltyCardOrders", "loyalty = true AND region = 'USA'");
            // await manager.CreateSubscriptionWithCorrelationFilter("Orders", "UkOrders", "UK");

            var rule = await manager.CreateSubscriptionWithSqlFilter("Order", "fucl", "value = 300");
            

            var sender = new TopicSender(connectionString, "Orders");

            var x = 0;
            while (x < 10)
            {
                var orders = CreateTestOrders();
                foreach (var order in orders)
                {
                    await sender.SendOrderMessage(order);
                    
                }
                x++;
                
                Console.WriteLine($"Press enter to send more. x is {x}");
                Console.ReadLine();
            }

            await sender.Close();

            




            // var queueName = "testqueue";
            //
            //
            // var client = new QueueClient(connectionString, queueName);
            //
            //
            // var val = Console.ReadLine();
            // while (val != "x")
            // {
            //     var order = new Order("Name", "US");
            //
            //     var json = System.Text.Json.JsonSerializer.Serialize(order);
            //     var message = new Message(Encoding.UTF8.GetBytes(json));
            //     await client.SendAsync(message);
            //     Console.WriteLine("Sent another");
            //     
            //     val = Console.ReadLine();
            // }
        }
        
        static List<Order> CreateTestOrders()
        {
            var orders = new List<Order>();

            orders.Add(new Order()
            {
                Name = "Loyal Customer",
                Value = 19.99,
                Region = "USA",
                Items = 1,
            });
            orders.Add(new Order()
            {
                Name = "Large Order",
                Value = 49.99,
                Region = "USA",
                Items = 50,
            });
            orders.Add(new Order()
            {
                Name = "High Value",
                Value = 749.45,
                Region = "USA",
                Items = 45,
            });
            orders.Add(new Order()
            {
                Name = "Loyal Europe",
                Value = 49.45,
                Region = "EU",
                Items = 3,
            });
            orders.Add(new Order()
            {
                Name = "UK Order",
                Value = 49.45,
                Region = "UK",
                Items = 3,
            });

            // Feel free to add more orders if you like.


            return orders;
        }
    }
}