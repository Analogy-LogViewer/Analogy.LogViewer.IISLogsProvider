using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Analogy.LogViewer.IISLogsProvider.Types
{
    public class GeoLocation
    {
        [JsonPropertyName("query")] public string Query { get; set; }
        [JsonPropertyName("status")] public string Status { get; set; }
        [JsonPropertyName("country")] public string Country { get; set; }
        [JsonPropertyName("countryCode")] public string CountryCode { get; set; }
        [JsonPropertyName("region")] public string Region { get; set; }
        [JsonPropertyName("regionName")] public string RegionName { get; set; }
        [JsonPropertyName("city")] public string City { get; set; }
        [JsonPropertyName("zip")] public string Zip { get; set; }
        [JsonPropertyName("lat")] public float Lat { get; set; }
        [JsonPropertyName("lon")] public float Lon { get; set; }
        [JsonPropertyName("timezone")] public string Timezone { get; set; }
        [JsonPropertyName("isp")] public string Isp { get; set; }
        [JsonPropertyName("org")] public string Org { get; set; }
        [JsonPropertyName("as")] public string AsName { get; set; }
    }
}