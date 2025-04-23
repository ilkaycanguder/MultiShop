using Microsoft.AspNetCore.Http;
using MultiShop.DtoLayer.BasketDtos;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MultiShop.WebUI.Services.BasketServices
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BasketService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task AddBasketItem(BasketItemDto basketItemDto)
        {
            try
            {
                Console.WriteLine($"AddBasketItem - Ürün eklenmeye çalışılıyor: PID:{basketItemDto.ProductId}, Miktar:{basketItemDto.Quantity}");

                // Mevcut sepeti al
                var basket = await GetBasket();

                // Sepet null ise yeni sepet oluştur
                if (basket == null)
                {
                    Console.WriteLine("AddBasketItem - Sepet null, yeni sepet oluşturuluyor");
                    basket = new BasketTotalDto
                    {
                        BasketItems = new List<BasketItemDto>()
                    };
                }

                // BasketItems null ise yeni liste oluştur
                if (basket.BasketItems == null)
                {
                    Console.WriteLine("AddBasketItem - BasketItems null, yeni liste oluşturuluyor");
                    basket.BasketItems = new List<BasketItemDto>();
                }

                // Sepette aynı ürün var mı kontrol et
                var existingItem = basket.BasketItems.FirstOrDefault(x => x.ProductId == basketItemDto.ProductId);

                if (existingItem != null)
                {
                    // Aynı ürün varsa miktarını artır
                    Console.WriteLine($"AddBasketItem - Sepette ürün mevcut, miktar artırılıyor. Eski miktar: {existingItem.Quantity}");
                    existingItem.Quantity += basketItemDto.Quantity;
                    Console.WriteLine($"AddBasketItem - Yeni miktar: {existingItem.Quantity}");
                }
                else
                {
                    // Yeni ürün ekle
                    Console.WriteLine($"AddBasketItem - Yeni ürün ekleniyor: {basketItemDto.ProductId}");
                    basket.BasketItems.Add(basketItemDto);
                }

                // Toplam fiyatı güncelle
                Console.WriteLine("AddBasketItem - Sepet kaydediliyor");
                await SaveBasket(basket);
                Console.WriteLine("AddBasketItem - Sepet başarıyla kaydedildi");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddBasketItem - Hata: {ex.Message}");
                Console.WriteLine($"AddBasketItem - Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task DeleteBasket(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<BasketTotalDto> GetBasket()
        {
            try
            {
                // Token kontrolü ve header ayarları
                SetAuthorizationToken();

                Console.WriteLine("GetBasket - Sepet alınıyor");
                var responseMessage = await _httpClient.GetAsync("baskets");

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine($"GetBasket - API yanıtı: {jsonData}");

                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        var basket = JsonConvert.DeserializeObject<BasketTotalDto>(jsonData);

                        if (basket == null)
                        {
                            Console.WriteLine("GetBasket - Sepet null olarak döndü");
                            return new BasketTotalDto { BasketItems = new List<BasketItemDto>() };
                        }

                        if (basket.BasketItems == null)
                        {
                            Console.WriteLine("GetBasket - Sepet içindeki öğeler null, boş liste oluşturuluyor");
                            basket.BasketItems = new List<BasketItemDto>();
                        }

                        Console.WriteLine($"GetBasket - Sepet alındı. Ürün sayısı: {basket.BasketItems.Count}");
                        return basket;
                    }
                    else
                    {
                        Console.WriteLine("GetBasket - API boş yanıt döndü");
                    }
                }
                else
                {
                    Console.WriteLine($"GetBasket - API hata kodu: {responseMessage.StatusCode}");
                    var errorContent = await responseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine($"GetBasket - API hata içeriği: {errorContent}");
                }

                // Hata durumunda veya boş yanıt durumunda yeni sepet döndür
                Console.WriteLine("GetBasket - Yeni sepet oluşturuluyor");
                return new BasketTotalDto { BasketItems = new List<BasketItemDto>() };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetBasket - Hata: {ex.Message}");
                Console.WriteLine($"GetBasket - Stack trace: {ex.StackTrace}");
                // Hata durumunda yeni sepet döndür
                return new BasketTotalDto { BasketItems = new List<BasketItemDto>() };
            }
        }

        public async Task<bool> RemoveBasketItem(string productId)
        {
            try
            {
                var values = await GetBasket();
                var deletedItem = values.BasketItems.FirstOrDefault(x => x.ProductId == productId);
                if (deletedItem != null)
                {
                    var result = values.BasketItems.Remove(deletedItem);
                    await SaveBasket(values);
                    return result;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RemoveBasketItem hatası: {ex.Message}");
                return false;
            }
        }

        public async Task SaveBasket(BasketTotalDto basketTotalDto)
        {
            try
            {
                // Token kontrolü ve header ayarları
                SetAuthorizationToken();

                // Sepet null ise yeni sepet oluştur
                if (basketTotalDto == null)
                {
                    Console.WriteLine("SaveBasket - Sepet null, yeni sepet oluşturuluyor");
                    basketTotalDto = new BasketTotalDto
                    {
                        BasketItems = new List<BasketItemDto>()
                    };
                }

                // BasketItems null ise yeni liste oluştur
                if (basketTotalDto.BasketItems == null)
                {
                    Console.WriteLine("SaveBasket - BasketItems null, yeni liste oluşturuluyor");
                    basketTotalDto.BasketItems = new List<BasketItemDto>();
                }

                // DiscountRate kontrolü (0 olması durumunda DiscountCode'u da null yapabiliriz)
                if (basketTotalDto.DiscountRate == 0)
                {
                    basketTotalDto.DiscountCode = null;
                    Console.WriteLine("SaveBasket - DiscountRate 0 olduğu için DiscountCode null yapıldı");
                }

                var content = new StringContent(JsonConvert.SerializeObject(basketTotalDto), Encoding.UTF8, "application/json");
                Console.WriteLine($"SaveBasket - Gönderilen veri: {await content.ReadAsStringAsync()}");

                var responseMessage = await _httpClient.PostAsync("baskets", content);
                Console.WriteLine($"SaveBasket - HTTP Yanıt Kodu: {responseMessage.StatusCode}");

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine($"SaveBasket - API yanıtı: {jsonData}");
                }
                else
                {
                    var errorContent = await responseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine($"SaveBasket - API hata kodu: {responseMessage.StatusCode}");
                    Console.WriteLine($"SaveBasket - API hata içeriği: {errorContent}");

                    throw new Exception($"Sepet kaydedilemedi: {responseMessage.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SaveBasket - Hata: {ex.Message}");
                Console.WriteLine($"SaveBasket - Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<bool> UpdateItemQuantity(string productId, int quantity)
        {
            try
            {
                if (string.IsNullOrEmpty(productId))
                {
                    Console.WriteLine("UpdateItemQuantity: ProductId boş veya null.");
                    return false;
                }

                if (quantity <= 0)
                {
                    Console.WriteLine($"UpdateItemQuantity: Geçersiz miktar değeri: {quantity}");
                    return false;
                }

                Console.WriteLine($"UpdateItemQuantity: Ürün miktarı güncelleniyor. ID: {productId}, Miktar: {quantity}");

                var basket = await GetBasket();

                if (basket == null || basket.BasketItems == null)
                {
                    Console.WriteLine("UpdateItemQuantity: Sepet veya sepet öğeleri null.");
                    return false;
                }

                var item = basket.BasketItems.FirstOrDefault(x => x.ProductId == productId);

                if (item == null)
                {
                    Console.WriteLine($"UpdateItemQuantity: Ürün sepette bulunamadı. ID: {productId}");
                    return false;
                }

                // Miktarı güncelle
                item.Quantity = quantity;
                Console.WriteLine($"UpdateItemQuantity: Ürün miktarı güncellendi. ID: {productId}, Yeni miktar: {quantity}");

                // Sepeti kaydet
                await SaveBasket(basket);
                Console.WriteLine("UpdateItemQuantity: Sepet başarıyla kaydedildi.");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateItemQuantity hatası: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        private void SetAuthorizationToken()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["MultiStoreCookieAuth"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine("Token başlığa eklendi");
            }
            else
            {
                Console.WriteLine("Token bulunamadı");
            }
        }
    }
}
