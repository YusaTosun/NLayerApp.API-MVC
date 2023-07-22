namespace NLayer.WEB.Services
{
    public class CategoryApiServices
    {
        private readonly HttpClient _httpClient;

        public CategoryApiServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
