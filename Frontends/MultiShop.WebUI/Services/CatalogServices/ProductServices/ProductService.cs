using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace MultiShop.WebUI.Services.CatalogServices.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateProductAsync(CreateProductDto createProductDto)
        {
            await _httpClient.PostAsJsonAsync<CreateProductDto>("products", createProductDto);
        }

        public async Task DeleteProductAsync(string id)
        {
            await _httpClient.DeleteAsync("products?id=" + id);
        }

        public async Task<List<ResultProductDto>> GetAllProductAsync()
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("products");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        var values = JsonConvert.DeserializeObject<List<ResultProductDto>>(jsonData);
                        return values ?? new List<ResultProductDto>();
                    }
                }
                Console.WriteLine($"API Hata (GetAllProductAsync): {responseMessage.StatusCode}, İçerik: {await responseMessage.Content.ReadAsStringAsync()}");
                return new List<ResultProductDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAllProductAsync Hatası: {ex.Message}");
                return new List<ResultProductDto>();
            }
        }

        public async Task<UpdateProductDto> GetByIdProductAsync(string id)
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("products/" + id);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        var values = JsonConvert.DeserializeObject<UpdateProductDto>(jsonData);
                        return values ?? new UpdateProductDto();
                    }
                }
                Console.WriteLine($"API Hata (GetByIdProductAsync): {responseMessage.StatusCode}, İçerik: {await responseMessage.Content.ReadAsStringAsync()}");
                return new UpdateProductDto();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetByIdProductAsync Hatası: {ex.Message}");
                return new UpdateProductDto();
            }
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductsWithCategoryAsync()
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("products/productlistwithcategory");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        Console.WriteLine($"ProductListWithCategory JSON veri: {jsonData}");

                        // Manuel dönüşüm deneyelim
                        try
                        {
                            var jArray = JArray.Parse(jsonData);
                            var resultList = new List<ResultProductWithCategoryDto>();

                            foreach (var item in jArray)
                            {
                                var productDto = new ResultProductWithCategoryDto
                                {
                                    ProductId = item["productId"]?.ToString(),
                                    ProductName = item["productName"]?.ToString(),
                                    ProductPrice = item["productPrice"]?.Value<decimal>() ?? 0,
                                    ProductImageUrl = item["productImageUrl"]?.ToString(),
                                    ProductDescription = item["productDescription"]?.ToString(),
                                    CategoryId = item["categoryId"]?.ToString(),
                                    Category = item["category"] != null
                                        ? new MultiShop.DtoLayer.CatalogDtos.CategoryDtos.ResultCategoryDto
                                        {
                                            CategoryId = item["category"]["categoryId"]?.ToString(),
                                            CategoryName = item["category"]["categoryName"]?.ToString()
                                        }
                                        : null
                                };
                                resultList.Add(productDto);
                            }

                            return resultList;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Manuel dönüşüm hatası: {ex.Message}");
                            // Standart dönüşümü dene
                            var values = JsonConvert.DeserializeObject<List<ResultProductWithCategoryDto>>(jsonData);
                            return values ?? new List<ResultProductWithCategoryDto>();
                        }
                    }
                }
                Console.WriteLine($"API Hata: {responseMessage.StatusCode}, İçerik: {await responseMessage.Content.ReadAsStringAsync()}");
                return new List<ResultProductWithCategoryDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JSON Dönüşüm Hatası: {ex.Message}");
                return new List<ResultProductWithCategoryDto>();
            }
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductWithCategoryByCategoryIdAsync(string CategoryId)
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("products/ProductListWithCategoryByCategoryId/" + CategoryId);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        Console.WriteLine($"ProductListWithCategoryById JSON veri: {jsonData}");

                        // Manuel dönüşüm deneyelim
                        try
                        {
                            var jArray = JArray.Parse(jsonData);
                            var resultList = new List<ResultProductWithCategoryDto>();

                            foreach (var item in jArray)
                            {
                                var productDto = new ResultProductWithCategoryDto
                                {
                                    ProductId = item["productId"]?.ToString(),
                                    ProductName = item["productName"]?.ToString(),
                                    ProductPrice = item["productPrice"]?.Value<decimal>() ?? 0,
                                    ProductImageUrl = item["productImageUrl"]?.ToString(),
                                    ProductDescription = item["productDescription"]?.ToString(),
                                    CategoryId = item["categoryId"]?.ToString(),
                                    Category = item["category"] != null
                                        ? new MultiShop.DtoLayer.CatalogDtos.CategoryDtos.ResultCategoryDto
                                        {
                                            CategoryId = item["category"]["categoryId"]?.ToString(),
                                            CategoryName = item["category"]["categoryName"]?.ToString()
                                        }
                                        : null
                                };
                                resultList.Add(productDto);
                            }

                            return resultList;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Manuel dönüşüm hatası: {ex.Message}");
                            // Standart dönüşümü dene
                            var values = JsonConvert.DeserializeObject<List<ResultProductWithCategoryDto>>(jsonData);
                            return values ?? new List<ResultProductWithCategoryDto>();
                        }
                    }
                }
                Console.WriteLine($"API Hata: {responseMessage.StatusCode}, İçerik: {await responseMessage.Content.ReadAsStringAsync()}");
                return new List<ResultProductWithCategoryDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JSON Dönüşüm Hatası: {ex.Message}");
                return new List<ResultProductWithCategoryDto>();
            }
        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateProductDto>("products", updateProductDto);
        }
    }
}
