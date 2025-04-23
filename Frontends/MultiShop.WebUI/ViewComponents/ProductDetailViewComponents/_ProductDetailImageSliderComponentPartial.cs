﻿using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.DtoLayer.CatalogDtos.ProductImageDtos;
using MultiShop.WebUI.Services.CatalogServices.ProductImageServices;
using Newtonsoft.Json;

namespace MultiShop.WebUI.ViewComponents.ProductDetailViewComponents
{
    public class _ProductDetailImageSliderComponentPartial : ViewComponent
    {
        private readonly IProductImageService _productImageService;

        public _ProductDetailImageSliderComponentPartial(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            try
            {
                var values = await _productImageService.GetByProductIdProductImageAsync(id);
                return View(values ?? new GetByIdProductImageDto());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ürün Görselleri Hatası: {ex.Message}");
                return View(new GetByIdProductImageDto());
            }
        }
    }
}
