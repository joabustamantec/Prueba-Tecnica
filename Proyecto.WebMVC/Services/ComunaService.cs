using Proyecto.DAL.Models;
using System.Net.Http.Json;

namespace Proyecto.WebMVC.Services
{
    public class ComunaService
    {
        private readonly HttpClient _http;

        public ComunaService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // GET /region/{regionId}/comuna
        public async Task<List<Comuna>> ObtenerComunas(int regionId)
        {
            var path = $"region/{regionId}/comuna";
            var comunas = await _http.GetFromJsonAsync<List<Comuna>>(path);
            return comunas ?? new List<Comuna>();
        }

        // GET /region/{regionId}/comuna/{comunaId}
        public async Task<Comuna?> ObtenerComuna(int regionId, int comunaId)
        {
            var path = $"region/{regionId}/comuna/{comunaId}";
            return await _http.GetFromJsonAsync<Comuna>(path);
        }

        // POST /region/{regionId}/comuna
        public async Task<bool> GuardarComuna(Comuna comuna)
        {
            var path = $"region/{comuna.IdRegion}/comuna";
            var resp = await _http.PostAsJsonAsync(path, comuna);
            return resp.IsSuccessStatusCode;
        }
    }
}
