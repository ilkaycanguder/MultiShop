using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MultiShop.WebUI.Services.Interfaces;
using System.Net;
using System.Net.Http.Headers;

namespace MultiShop.WebUI.Handlers
{
    public class ResourceOwnerPasswordTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;

        public ResourceOwnerPasswordTokenHandler(IHttpContextAccessor httpContextAccessor, IIdentityService identityService)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            // Token kontrolü ve log
            if (string.IsNullOrEmpty(accessToken))
            {
                Console.WriteLine("Uyarı: Access token boş veya null! Kimlik doğrulama yapılamamış olabilir.");
                // Kimlik doğrulama hataları için yine de isteği gönder
            }
            else
            {
                Console.WriteLine("Access token alındı. Uzunluk: " + accessToken.Length);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var response = await base.SendAsync(request, cancellationToken);
            Console.WriteLine($"API isteği yanıt kodu: {response.StatusCode}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Yetkisiz erişim hatası (401). Token yenileme deneniyor...");
                var tokenResponse = await _identityService.GetRefreshToken();
                if (tokenResponse != null)
                {
                    accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
                    Console.WriteLine("Token yenilendi. Yeni token uzunluğu: " + (accessToken?.Length ?? 0));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    response = await base.SendAsync(request, cancellationToken);
                    Console.WriteLine($"Yenilenen token ile API yanıtı: {response.StatusCode}");
                }
                else
                {
                    Console.WriteLine("Token yenileme başarısız!");
                }
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Token yenileme sonrası hala Yetkisiz erişim hatası (401).");
            }

            return response;
        }
    }
}
