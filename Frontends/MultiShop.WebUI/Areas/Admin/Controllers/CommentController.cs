using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CommentDtos;
using MultiShop.WebUI.Services.CommentServices;
using Newtonsoft.Json;
using System.Text;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    [Route("Admin/Comment")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Yorumlar";
            ViewBag.v3 = "Yorum Listesi";
            ViewBag.v0 = "Yorum İşlemleri";

            try
            {
                var values = await _commentService.GetAllCommentAsync();
                return View(values);
            }
            catch (Exception ex)
            {
                // Hata durumunu loglama
                Console.WriteLine($"Yorum listesi alınırken hata: {ex.Message}");
                return View(new List<ResultCommentDto>());
            }
        }

        [Route("DeleteComment/{id}")]
        public async Task<IActionResult> DeleteComment(string id)
        {
            try
            {
                await _commentService.DeleteCommentAsync(id);
                return RedirectToAction("Index", "Comment", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                // Hata durumunu loglama
                Console.WriteLine($"Yorum silinirken hata: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        [Route("UpdateComment/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateComment(string id)
        {
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Yorumlar";
            ViewBag.v3 = "Yorum Listesi";
            ViewBag.v0 = "Yorum İşlemleri";

            try
            {
                var values = await _commentService.GetByIdCommentAsync(id);
                return View(values);
            }
            catch (Exception ex)
            {
                // Hata durumunu loglama
                Console.WriteLine($"Yorum detayı alınırken hata: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        [Route("UpdateComment/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateComment(UpdateCommentDto updateCommentDto)
        {
            try
            {
                updateCommentDto.Status = true;
                await _commentService.UpdateCommentAsync(updateCommentDto);
                return RedirectToAction("Index", "Comment", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                // Hata durumunu loglama
                Console.WriteLine($"Yorum güncellenirken hata: {ex.Message}");
                return View(updateCommentDto);
            }
        }
    }
}

