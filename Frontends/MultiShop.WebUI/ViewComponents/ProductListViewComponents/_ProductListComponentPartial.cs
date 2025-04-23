using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using Newtonsoft.Json;

namespace MultiShop.WebUI.ViewComponents.ProductListViewComponents
{
    public class _ProductListComponentPartial : ViewComponent
    {
        private readonly IProductService _productService;

        public _ProductListComponentPartial(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            try
            {
                var values = await _productService.GetProductWithCategoryByCategoryIdAsync(id);
                return View(values ?? new List<ResultProductWithCategoryDto>());
            }
            catch (Exception ex)
            {
                // Hata durumunda boş liste gönder
                return View(new List<ResultProductWithCategoryDto>());
            }
        }
    }
}
