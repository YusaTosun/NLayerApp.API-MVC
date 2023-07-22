using NLayer.Core.DTOs;
using System.Runtime.InteropServices;

namespace NLayer.WEB.Services
{
    public class CategoryApiServices
    {
        private readonly HttpClient _httpClient;

        public CategoryApiServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<CategoryDto>>>("categories");
            return response.Data;
        }
    }
}
