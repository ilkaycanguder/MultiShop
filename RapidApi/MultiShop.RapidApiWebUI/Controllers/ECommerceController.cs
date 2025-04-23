using Microsoft.AspNetCore.Mvc;
using MultiShop.RapidApiWebUI.Models;
using Newtonsoft.Json;

namespace MultiShop.RapidApiWebUI.Controllers
{
    public class ECommerceController : Controller
    {
        public async Task<IActionResult> ECommerceList()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://real-time-product-search.p.rapidapi.com/search-v2?q=logitech%20fare&country=tr&language=en&page=1&limit=10&sort_by=BEST_MATCH&product_condition=ANY&return_filters=true"),
                Headers =
            {
                { "x-rapidapi-key", "f59357f02dmshf8875a859b08ed3p176bc9jsn04fb190df32d" },
                { "x-rapidapi-host", "real-time-product-search.p.rapidapi.com" },
            },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var root = JsonConvert.DeserializeObject<ECommerceViewModel.Rootobject>(body);
                var productList = root?.data?.products?.ToList() ?? new List<ECommerceViewModel.Product>();

                return View(productList);
            }
        }
    }
}
