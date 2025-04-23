using MultiShop.WebUI.Services.Interfaces;
using System.Net;
using System.Net.Http.Headers;

namespace MultiShop.WebUI.Handlers
{
    public class ClientCredentialTokenHandler : DelegatingHandler
    {
        private readonly IClientCredentialTokenService _clientCredentialTokenService;

        public ClientCredentialTokenHandler(IClientCredentialTokenService clientCredentialTokenService)
        {
            _clientCredentialTokenService = clientCredentialTokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _clientCredentialTokenService.GetToken();

                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("ClientCredential token alınamadı! Token boş veya null.");
                }
                else
                {
                    Console.WriteLine("ClientCredential token alındı. Uzunluk: " + token.Length);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await base.SendAsync(request, cancellationToken);
                Console.WriteLine($"ClientCredential API isteği yanıt kodu: {response.StatusCode}");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("Yetkisiz erişim hatası (401). ClientCredential token ile yeniden deneme gerekebilir.");
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClientCredentialTokenHandler hatası: {ex.Message}");
                throw;
            }
        }
    }
}
