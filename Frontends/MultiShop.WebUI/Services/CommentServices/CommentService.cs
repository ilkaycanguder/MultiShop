using MultiShop.DtoLayer.CommentDtos;
using MultiShop.WebUI.Services.CommentServices;
using Newtonsoft.Json;

namespace MultiShop.WebUI.Services.CommentServices
{
    public class CommentService : ICommentService
    {
        private readonly HttpClient _httpClient;

        public CommentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ResultCommentDto>> CommentListByProductId(string id)
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync($"comments/CommentListByProductId/{id}");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        var values = JsonConvert.DeserializeObject<List<ResultCommentDto>>(jsonData);
                        return values ?? new List<ResultCommentDto>();
                    }
                }
                Console.WriteLine($"API Hata (CommentListByProductId): {responseMessage.StatusCode}, İçerik: {await responseMessage.Content.ReadAsStringAsync()}");
                return new List<ResultCommentDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CommentListByProductId Hatası: {ex.Message}");
                return new List<ResultCommentDto>();
            }
        }

        public async Task CreateCommentAsync(CreateCommentDto createCommentDto)
        {
            try
            {
                await _httpClient.PostAsJsonAsync<CreateCommentDto>("comments", createCommentDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CreateCommentAsync Hatası: {ex.Message}");
                // Hatayı üst seviyeye iletmek için tekrar throw edebilirsin
                throw;
            }
        }

        public async Task DeleteCommentAsync(string id)
        {
            try
            {
                await _httpClient.DeleteAsync("comments?id=" + id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteCommentAsync Hatası: {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetActiveCommentCount()
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("comments/GetActiveCommentCount");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        var value = JsonConvert.DeserializeObject<int>(jsonData);
                        return value;
                    }
                }
                Console.WriteLine($"API Hata (GetActiveCommentCount): {responseMessage.StatusCode}, İçerik: {await responseMessage.Content.ReadAsStringAsync()}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetActiveCommentCount Hatası: {ex.Message}");
                return 0;
            }
        }

        public async Task<List<ResultCommentDto>> GetAllCommentAsync()
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("comments");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        Console.WriteLine($"Yorum JSON veri: {jsonData}");
                        var values = JsonConvert.DeserializeObject<List<ResultCommentDto>>(jsonData);
                        return values ?? new List<ResultCommentDto>();
                    }
                }
                Console.WriteLine($"API Hata (GetAllCommentAsync): {responseMessage.StatusCode}, İçerik: {await responseMessage.Content.ReadAsStringAsync()}");
                return new List<ResultCommentDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAllCommentAsync Hatası: {ex.Message}");
                return new List<ResultCommentDto>();
            }
        }

        public async Task<UpdateCommentDto> GetByIdCommentAsync(string id)
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("comments/" + id);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        var values = JsonConvert.DeserializeObject<UpdateCommentDto>(jsonData);
                        return values ?? new UpdateCommentDto();
                    }
                }
                Console.WriteLine($"API Hata (GetByIdCommentAsync): {responseMessage.StatusCode}, İçerik: {await responseMessage.Content.ReadAsStringAsync()}");
                return new UpdateCommentDto();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetByIdCommentAsync Hatası: {ex.Message}");
                return new UpdateCommentDto();
            }
        }

        public async Task<int> GetPassiveCommentCount()
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("comments/GetPassiveCommentCount");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        var value = JsonConvert.DeserializeObject<int>(jsonData);
                        return value;
                    }
                }
                Console.WriteLine($"API Hata (GetPassiveCommentCount): {responseMessage.StatusCode}, İçerik: {await responseMessage.Content.ReadAsStringAsync()}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetPassiveCommentCount Hatası: {ex.Message}");
                return 0;
            }
        }

        public async Task<int> GetTotalCommentCount()
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("comments/GetTotalCommentCount");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        var value = JsonConvert.DeserializeObject<int>(jsonData);
                        return value;
                    }
                }
                Console.WriteLine($"API Hata (GetTotalCommentCount): {responseMessage.StatusCode}, İçerik: {await responseMessage.Content.ReadAsStringAsync()}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetTotalCommentCount Hatası: {ex.Message}");
                return 0;
            }
        }

        public async Task UpdateCommentAsync(UpdateCommentDto updateCommentDto)
        {
            try
            {
                await _httpClient.PutAsJsonAsync<UpdateCommentDto>("comments", updateCommentDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateCommentAsync Hatası: {ex.Message}");
                throw;
            }
        }
    }
}
