using MultiShop.Basket.Dtos;
using MultiShop.Basket.Settings;
using System.Text.Json;

namespace MultiShop.Basket.Services
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;
        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }
        public async Task DeleteBasket(string userId)
        {
            await _redisService.GetDb().KeyDeleteAsync(userId);
        }
        public async Task<BasketTotalDto> GetBasket(string userId)
        {
            var existBasket = await _redisService.GetDb().StringGetAsync(userId);

            if (string.IsNullOrEmpty(existBasket))
            {
                // Kullanıcının sepeti hiç yoksa, boş DTO dön
                return new BasketTotalDto
                {
                    UserId = userId,
                    BasketItems = new List<BasketItemDto>()
                };
            }

            var basket = JsonSerializer.Deserialize<BasketTotalDto>(existBasket);

            // Ek güvenlik: deserialize başarısızsa veya liste null ise
            if (basket == null)
            {
                return new BasketTotalDto
                {
                    UserId = userId,
                    BasketItems = new List<BasketItemDto>()
                };
            }

            if (basket.BasketItems == null)
            {
                basket.BasketItems = new List<BasketItemDto>();
            }

            return basket;
        }

        public async Task SaveBasket(BasketTotalDto basketTotalDto)
        {
            await _redisService.GetDb().StringSetAsync(basketTotalDto.UserId, JsonSerializer.Serialize(basketTotalDto));

        }
    }
}