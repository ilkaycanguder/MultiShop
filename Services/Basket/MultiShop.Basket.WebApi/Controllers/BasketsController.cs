using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Basket.BusinessLayer.Abstract;
using MultiShop.Basket.DataAccessLayer.Abstract;
using MultiShop.DtoLayer.BasketDtos;
using System.Security.Claims;
using System.Text.Json;

namespace MultiShop.Basket.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly ILogger<BasketsController> _logger;

        public BasketsController(IBasketService basketService, ILogger<BasketsController> logger)
        {
            _basketService = basketService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetMyBasketDetail()
        {
            try
            {
                string userId = "defaultuser";

                if (User.Identity.IsAuthenticated)
                {
                    userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    _logger.LogInformation("GetMyBasketDetail - Doğrulanmış kullanıcı: {UserId}", userId);
                }
                else
                {
                    _logger.LogInformation("GetMyBasketDetail - Doğrulanmamış kullanıcı, default kullanıcı kimliği kullanılıyor: {UserId}", userId);
                }

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("GetMyBasketDetail - Kullanıcı kimliği alınamadı");
                    return BadRequest("Kullanıcı kimliği alınamadı");
                }

                _logger.LogInformation("GetMyBasketDetail - Redis'ten sepet alınıyor. UserId: {UserId}", userId);
                var values = await _basketService.GetBasket(userId);

                if (values == null)
                {
                    _logger.LogInformation("GetMyBasketDetail - Redis'te sepet bulunamadı. UserId: {UserId}", userId);
                    return Ok(new BasketTotalDto { BasketItems = new List<BasketItemDto>() });
                }

                _logger.LogInformation("GetMyBasketDetail - Sepet alındı. Ürün sayısı: {Count}", values.BasketItems?.Count ?? 0);
                return Ok(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetMyBasketDetail - Hata meydana geldi");
                return StatusCode(500, "Sepet alınırken bir hata oluştu: " + ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveMyBasket(BasketTotalDto basketTotalDto)
        {
            try
            {
                // Request body loglama
                var requestBody = JsonSerializer.Serialize(basketTotalDto);
                _logger.LogInformation("SaveMyBasket - İstek gövdesi: {RequestBody}", requestBody);

                // Null kontrolü
                if (basketTotalDto == null)
                {
                    _logger.LogWarning("SaveMyBasket - Gelen sepet verisi null");
                    return BadRequest("Sepet verisi alınamadı");
                }

                // Kullanıcı kimliği kontrolü
                string userId = "defaultuser";

                if (User.Identity.IsAuthenticated)
                {
                    userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    _logger.LogInformation("SaveMyBasket - Doğrulanmış kullanıcı: {UserId}", userId);
                }
                else
                {
                    _logger.LogInformation("SaveMyBasket - Doğrulanmamış kullanıcı, default kullanıcı kimliği kullanılıyor: {UserId}", userId);
                }

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("SaveMyBasket - Kullanıcı kimliği alınamadı");
                    return BadRequest("Kullanıcı kimliği alınamadı");
                }

                // Kullanıcı kimliğini atama
                basketTotalDto.UserId = userId;
                _logger.LogInformation("SaveMyBasket - Kullanıcı kimliği atandı: {UserId}", userId);

                // Sepet içeriği kontrolü
                if (basketTotalDto.BasketItems == null)
                {
                    _logger.LogWarning("SaveMyBasket - BasketItems null, yeni liste oluşturuluyor");
                    basketTotalDto.BasketItems = new List<BasketItemDto>();
                }

                // Toplam fiyat hesaplama
                if (basketTotalDto.BasketItems.Any())
                {
                    decimal totalPrice = basketTotalDto.BasketItems.Sum(item => item.Price * item.Quantity);
                    basketTotalDto.TotalPrice = totalPrice;
                    _logger.LogInformation("SaveMyBasket - Toplam fiyat hesaplandı: {TotalPrice}", totalPrice);
                }
                else
                {
                    _logger.LogInformation("SaveMyBasket - Sepet boş, toplam fiyat 0 olarak ayarlandı");
                    basketTotalDto.TotalPrice = 0;
                }

                // Redis'e sepeti kaydetme
                _logger.LogInformation("SaveMyBasket - Redis'e sepet kaydediliyor. Ürün sayısı: {Count}", basketTotalDto.BasketItems.Count);
                await _basketService.SaveBasket(basketTotalDto);

                _logger.LogInformation("SaveMyBasket - Sepet başarıyla kaydedildi");
                return Ok("Sepet başarıyla kaydedildi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SaveMyBasket - Hata meydana geldi: {Message}", ex.Message);
                return StatusCode(500, "Sepet kaydedilirken bir hata oluştu: " + ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation("DeleteBasket - Kullanıcı kimliği alınıyor: {UserId}", userId);

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("DeleteBasket - Kullanıcı kimliği alınamadı");
                    return BadRequest("Kullanıcı kimliği alınamadı");
                }

                _logger.LogInformation("DeleteBasket - Sepet siliniyor. UserId: {UserId}", userId);
                await _basketService.DeleteBasket(userId);

                _logger.LogInformation("DeleteBasket - Sepet başarıyla silindi");
                return Ok("Sepet başarıyla silindi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteBasket - Hata meydana geldi");
                return StatusCode(500, "Sepet silinirken bir hata oluştu: " + ex.Message);
            }
        }
    }
}