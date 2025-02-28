using FluentValidation;
using System.Text.Json.Serialization;

namespace BlockingCountriesApi.Models.Response
{
    public class GeoLocationResponse
    {
        [JsonPropertyName("ip")]
        public string IpAddress { get; set; }

        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }

        [JsonPropertyName("continent_code")]
        public string ContinentCode { get; set; }

        [JsonPropertyName("continent_name")]
        public string ContinentName { get; set; }

        [JsonPropertyName("country_code2")]
        public string CountryCode { get; set; }

        [JsonPropertyName("country_code3")]
        public string CountryCode3 { get; set; }

        [JsonPropertyName("country_name")]
        public string CountryName { get; set; }

        [JsonPropertyName("country_name_official")]
        public string CountryNameOfficial { get; set; }

        [JsonPropertyName("country_capital")]
        public string CountryCapital { get; set; }

        [JsonPropertyName("state_prov")]
        public string StateProvince { get; set; }

        [JsonPropertyName("state_code")]
        public string StateCode { get; set; }

        [JsonPropertyName("district")]
        public string District { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("zipcode")]
        public string ZipCode { get; set; }

        [JsonPropertyName("latitude")]
        public string Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public string Longitude { get; set; }

        [JsonPropertyName("is_eu")]
        public bool IsEU { get; set; }

        [JsonPropertyName("calling_code")]
        public string CallingCode { get; set; }

        [JsonPropertyName("country_tld")]
        public string CountryTld { get; set; }

        [JsonPropertyName("languages")]
        public string Languages { get; set; }

        [JsonPropertyName("country_flag")]
        public string CountryFlag { get; set; }

        [JsonPropertyName("isp")]
        public string ISP { get; set; }


    }
}
