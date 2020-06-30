using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;

namespace CogAI.QuickStart
{
    public partial class Hotel
    {
        [Key] [IsFilterable] public string HotelId { get; set; }

        [IsSearchable] [IsSortable] public string HotelName { get; set; }

        [IsSearchable]
        [Analyzer(AnalyzerName.AsString.EnMicrosoft)]
        public string Description { get; set; }

        [IsSearchable]
        [Analyzer(AnalyzerName.AsString.FrLucene)]
        [JsonProperty("Description_fr")]
        public string DescriptionFr { get; set; }

        [IsSearchable]
        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public string Category { get; set; }

        [IsSearchable]
        [IsFilterable]
        [IsFacetable]
        public string[] Tags { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public bool? ParkingIncluded { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public DateTimeOffset? LastRenovationDate { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public double? Rating { get; set; }

        public Address Address { get; set; }
    }

    public partial class Hotel
    {
        // This implementation of ToString() is only for the purposes of the sample console application.
        // You can override ToString() in your own model class if you want, but you don't need to in order
        // to use the Azure Cognitive Search .NET SDK.
        public override string ToString()
        {
            var builder = new StringBuilder();

            if (!string.IsNullOrEmpty(HotelId)) builder.AppendFormat("HotelId: {0}\n", HotelId);

            if (!string.IsNullOrEmpty(HotelName)) builder.AppendFormat("Name: {0}\n", HotelName);

            if (!string.IsNullOrEmpty(Description)) builder.AppendFormat("Description: {0}\n", Description);

            if (!string.IsNullOrEmpty(DescriptionFr))
                builder.AppendFormat("Description (French): {0}\n", DescriptionFr);

            if (!string.IsNullOrEmpty(Category)) builder.AppendFormat("Category: {0}\n", Category);

            if (Tags != null && Tags.Length > 0) builder.AppendFormat("Tags: [ {0} ]\n", string.Join(", ", Tags));

            if (ParkingIncluded.HasValue)
                builder.AppendFormat("Parking included: {0}\n", ParkingIncluded.Value ? "yes" : "no");

            if (LastRenovationDate.HasValue) builder.AppendFormat("Last renovated on: {0}\n", LastRenovationDate);

            if (Rating.HasValue) builder.AppendFormat("Rating: {0}\n", Rating);

            if (Address != null && !Address.IsEmpty) builder.AppendFormat("Address: \n{0}\n", Address);

            return builder.ToString();
        }
    }
}