using System;
using System.Text.Json.Serialization;

namespace LearnSomeAzure.Core.ServiceBus
{
    public class Order
    {
        
        [JsonPropertyName("id")] 
        public string OrderId { get; set; } = Guid.NewGuid().ToString();
        
        [JsonPropertyName("OrderId")]
        public string Partition => OrderId;


        public DateTimeOffset? DateCreated { get; set; } = DateTimeOffset.Now;
        public string Name { get; set; }
        public string Region { get; set; }
        public double Value { get; set; }
        public int Items { get; set; }

        public Order(string name, string region, double value, int items)
        {
            Name = name;
            Region = region;
            Value = value;
            Items = items;
        }
        public Order()
        {
        }
    }
}