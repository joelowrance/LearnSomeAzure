using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LearnSomeAzure.Core.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace LearnSomeAzure.ServiceBus.Receiver
{
    
    
    public static class Client
    {
        private static QueueClient _client;

        public static QueueClient Instance
        {
            get
            {
                if (_client == null)
                {
                    var connectionString =
                        "Endpoint=sb://lowrance.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=/faLtKBZOsvmB2m73wvBnvcm8dsRcmsHN2GCY7IbwxQ=";

                    var queueName = "testqueue";
            
                    _client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
                }

                return _client;
            }
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString =
                "Endpoint=sb://lowrance.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=/faLtKBZOsvmB2m73wvBnvcm8dsRcmsHN2GCY7IbwxQ=";

            
            var manager = new Manager(connectionString);;
            var subscriptionDescriptions = await manager.GetSubscriptionsForTopic("Orders");

            // Loop through the subscriptions and process the order messages.
            foreach (var subscriptionDescription in subscriptionDescriptions)
            {
                Console.WriteLine($"{subscriptionDescription.SubscriptionName} - {subscriptionDescription.TopicPath}");
            }

            
            var receiver = new SubscriptionReceiver(connectionString, "Orders", "LargeOrders");
            receiver.RegisterHandlers();
            Console.WriteLine("press enter when done");
            Console.ReadLine();
            await receiver.Close();


            // RegisterOnMessageHandlerAndReceiveMessages();
            // Console.ReadLine();
        }
        
        static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether the message pump should automatically complete the messages after returning from user callback.
                // False below indicates the complete operation is handled by the user callback as in ProcessMessagesAsync().
                AutoComplete = false
            };

            // Register the function that processes messages.
            Client.Instance.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }
        
        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message.
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            // Complete the message so that it is not received again.
            // This can be done only if the queue Client is created in ReceiveMode.PeekLock mode (which is the default).
            await Client.Instance.CompleteAsync(message.SystemProperties.LockToken);

            // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
            // If queueClient has already been closed, you can choose to not call CompleteAsync() or AbandonAsync() etc.
            // to avoid unnecessary exceptions.
        }
        
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}