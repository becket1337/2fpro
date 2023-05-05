using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace _2fpro.Models
{
    public class IPGeoLocation
    {
        [JsonProperty("query")]
        public string IP { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("country")]
        public string CountryName { get; set; }

        [JsonProperty("region")]
        public string RegionCode { get; set; }

        [JsonProperty("regionName")]
        public string RegionName { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("zip")]
        public string ZipCode { get; set; }

        [JsonProperty("timezone")]
        public string TimeZone { get; set; }

        [JsonProperty("lat")]
        public float Latitude { get; set; }

        [JsonProperty("lon")]
        public float Longitude { get; set; }

        [JsonProperty("isp")]
        public string Isp { get; set; }

        [JsonProperty("org")]
        public string Org { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("as")]
        public string As { get; set; }

        [JsonProperty("mobile")]
        public bool isMobile { get; set; }

        private IPGeoLocation() { }

        public static async Task<IPGeoLocation> QueryGeographicalLocationAsync(string ipAddress)
        {
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync("http://ip-api.com/json/" + ipAddress + "?lang=ru");

            return JsonConvert.DeserializeObject<IPGeoLocation>(result);
        }
    }
}
