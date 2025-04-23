using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CommentServices;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Services.MessageServices;

namespace MultiShop.WebUI.Areas.Admin.ViewComponents.AdminLayoutViewComponents
{
    public class _AdminLayoutHeaderComponentPartial : ViewComponent
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly ICommentService _commentService;

        public _AdminLayoutHeaderComponentPartial(IMessageService messageService, IUserService userService, ICommentService commentService)
        {
            _messageService = messageService;
            _userService = userService;
            _commentService = commentService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userService.GetUserInfo();

            // Kullanıcı bilgilerini ViewBag'e ekle (boş değerleri kontrol et)
            string name = !string.IsNullOrEmpty(user.Name) ? user.Name : "Kullanıcı";
            string surname = !string.IsNullOrEmpty(user.Surname) ? user.Surname : "";
            ViewBag.NameSurname = $"{name} {surname}".Trim();
            ViewBag.Email = !string.IsNullOrEmpty(user.Email) ? user.Email : "kullanici@multishop.com";

            int messageCount = await _messageService.GetTotalMessageCountByReceiverId(user.Id);
            ViewBag.messageCount = messageCount;

            int totalCommentCount = await _commentService.GetTotalCommentCount();
            ViewBag.totalCommentCount = totalCommentCount;
            return View();
        }
    }
}
