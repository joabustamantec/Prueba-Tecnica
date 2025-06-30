using Proyecto.DAL.Models;
using System.Net.Http.Json;

namespace Proyecto.WebMVC.Services
{
    public class RegionService
    {
        private readonly HttpClient _httpClient;

        public RegionService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        // GET /region
        public async Task<List<Region>> ObtenerRegiones()
            => await _httpClient.GetFromJsonAsync<List<Region>>("region")
               ?? new List<Region>();

        // GET /region/{id}
        public async Task<Region?> ObtenerRegionPorId(int id)
            => await _httpClient.GetFromJsonAsync<Region>($"region/{id}");

        // POST /region
        public async Task<bool> GuardarRegion(Region region)
        {
            var response = await _httpClient.PostAsJsonAsync("region", region);
            return response.IsSuccessStatusCode;
        }


    }
}
