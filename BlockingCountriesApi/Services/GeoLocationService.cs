using BlockingCountriesApi.Models.Response;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Net;

namespace BlockingCountriesApi.Services
{
    public class GeoLocationService(HttpClient httpClient, IConfiguration configuration)
    {
        private readonly string _apiKey = configuration["GeoLocation:ApiKey"];

        public async Task<GeoLocationResponse> GetGeoLocationAsync(string ip)
        {
            var response = await httpClient.GetAsync($"ipgeo?apiKey={_apiKey}&ip={ip}");

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
                throw new HttpRequestException("Rate limit exceeded", null, HttpStatusCode.TooManyRequests);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GeoLocationResponse>();

        }


    }
}
