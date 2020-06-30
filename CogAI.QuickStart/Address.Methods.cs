using Microsoft.Azure.Search;
using Newtonsoft.Json;

namespace CogAI.QuickStart
{
    public partial class Address
    {
        [IsSearchable] public string StreetAddress { get; set; }

        [IsSearchable]
        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public string City { get; set; }

        [IsSearchable]
        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public string StateProvince { get; set; }

        [IsSearchable]
        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public string PostalCode { get; set; }

        [IsSearchable]
        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public string Country { get; set; }
    }

    public partial class Address
    {
        [JsonIgnore]
        public bool IsEmpty => string.IsNullOrEmpty(StreetAddress) &&
                               string.IsNullOrEmpty(City) &&
                               string.IsNullOrEmpty(StateProvince) &&
                               string.IsNullOrEmpty(PostalCode) &&
                               string.IsNullOrEmpty(Country);
        // This implementation of ToString() is only for the purposes of the sample console application.
        // You can override ToString() in your own model class if you want, but you don't need to in order
        // to use the Azure Cognitive Search .NET SDK.

        public override string ToString()
        {
            return IsEmpty ? string.Empty : $"{StreetAddress}\n{City}, {StateProvince} {PostalCode}\n{Country}";
        }
    }
}