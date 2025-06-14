using Analogy.LogViewer.IISLogsProvider.Types;
using System;
using System.Collections.Generic;
using System.Linq;
#if NET
using System.Net.Http;
#endif
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Analogy.LogViewer.IISLogsProvider.Managers
{
    internal class GeoLocationManager
    {
        private static readonly Lazy<GeoLocationManager> _instance =
            new Lazy<GeoLocationManager>(() => new GeoLocationManager());
        public static GeoLocationManager Instance { get; set; } = _instance.Value;
#if NET
        public HttpClient Client { get; set; } = new HttpClient();
#endif
        private Dictionary<string, GeoLocation> Results = new(StringComparer.InvariantCultureIgnoreCase);

        public async Task<string> GetCountry(string ipOrHostName)
        {
#if NET
            if (Results.TryGetValue(ipOrHostName, out var location))
            {
                return location.Country;
            }

            string url = $"http://ip-api.com/json/{ipOrHostName}";
            var result = await Client.GetAsync(url);
            if (result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync();
                GeoLocation loc = System.Text.Json.JsonSerializer.Deserialize<GeoLocation>(json);
                Results.Add(ipOrHostName, loc);
                return loc.Country;
            }
            Results.Add(ipOrHostName, new GeoLocation());
            return "";
#else
            return "";
#endif
        }
    }
}