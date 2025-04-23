using MultiShop.DtoLayer.CatalogDtos.ProductImageDtos;
using Newtonsoft.Json;

namespace MultiShop.WebUI.Services.CatalogServices.ProductImageServices
{
    public class ProductImageService : IProductImageService
    {
        private readonly HttpClient _httpClient;

        public ProductImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateProductImageAsync(CreateProductImageDto createProductImageDto)
        {
            await _httpClient.PostAsJsonAsync<CreateProductImageDto>("productimages", createProductImageDto);
        }

        public async Task DeleteProductImageAsync(string id)
        {
            await _httpClient.DeleteAsync("productimages?id=" + id);
        }

        public async Task<List<ResultProductImageDto>> GetAllProductImageAsync()
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("productimages");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        var values = JsonConvert.DeserializeObject<List<ResultProductImageDto>>(jsonData);
                        return values ?? new List<ResultProductImageDto>();
                    }
                }
                Console.WriteLine($"API Hata (GetAllProductImageAsync): {responseMessage.StatusCode}, İçerik: {await responseMessage.Content.ReadAsStringAsync()}");
                return new List<ResultProductImageDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAllProductImageAsync Hatası: {ex.Message}");
                return new List<ResultProductImageDto>();
            }
        }

        public async Task<GetByIdProductImageDto> GetByIdProductImageAsync(string id)
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("productimages/" + id);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        var values = JsonConvert.DeserializeObject<GetByIdProductImageDto>(jsonData);
                        return values ?? new GetByIdProductImageDto();
                    }
                }
                Console.WriteLine($"API Hata (GetByIdProductImageAsync): {responseMessage.StatusCode}, İçerik: {await responseMessage.Content.ReadAsStringAsync()}");
                return new GetByIdProductImageDto();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetByIdProductImageAsync Hatası: {ex.Message}");
                return new GetByIdProductImageDto();
            }
        }

        public async Task<GetByIdProductImageDto> GetByProductIdProductImageAsync(string id)
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("productimages/ProductImagesByProductId/" + id);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        Console.WriteLine($"ProductImagesByProductId JSON veri: {jsonData}");
                        var values = JsonConvert.DeserializeObject<GetByIdProductImageDto>(jsonData);
                        return values ?? new GetByIdProductImageDto();
                    }
                }
                Console.WriteLine($"API Hata (GetByProductIdProductImageAsync): {responseMessage.StatusCode}, İçerik: {await responseMessage.Content.ReadAsStringAsync()}");
                return new GetByIdProductImageDto();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetByProductIdProductImageAsync Hatası: {ex.Message}");
                return new GetByIdProductImageDto();
            }
        }

        public async Task UpdateProductImageAsync(UpdateProductImageDto updateProductImageDto)
        {
            try
            {
                await _httpClient.PutAsJsonAsync<UpdateProductImageDto>("productimages", updateProductImageDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateProductImageAsync Hatası: {ex.Message}");
                throw;
            }
        }
    }
}
