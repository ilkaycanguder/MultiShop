namespace MultiShop.Basket.LoginServices
{
    public class LoginService : ILoginService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginService(IHttpContextAccessor contextAccessor)
        {
            _httpContextAccessor = contextAccessor;
        }
        public string GetUserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst("sub")?.Value;
                Console.WriteLine($"[DEBUG] LoginService.GetUserId: {userId}");
                return userId;
            }
        }
    }
}
