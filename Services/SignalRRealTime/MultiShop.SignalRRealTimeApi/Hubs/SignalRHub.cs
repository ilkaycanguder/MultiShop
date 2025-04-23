using Microsoft.AspNetCore.SignalR;
using MultiShop.SignalRRealTimeApi.Services.SignalRCommentServices;
using MultiShop.SignalRRealTimeApi.Services.SignalRMessageServices;

namespace MultiShop.SignalRRealTimeApi.Hubs
{
    public class SignalRHub : Hub
    {
        private readonly ISignalRCommentService _signalRCommentService;
        private readonly ISignalRMessageService _signalRMessageService;

        public SignalRHub(ISignalRCommentService signalRCommentService, ISignalRMessageService signalRMessageService)
        {
            _signalRCommentService = signalRCommentService;
            _signalRMessageService = signalRMessageService;
        }

        public async Task SendStatisticCount(string id)
        {
            // Yorum sayısını al ve tüm istemcilere gönder
            var getTotalCommentCount = await _signalRCommentService.GetTotalCommentCount();
            await Clients.All.SendAsync("ReceiveCommentCount", getTotalCommentCount);

            try
            {
                // Mesaj sayısını al ve tüm istemcilere gönder
                var getTotalMessageCount = 0;
                if (!string.IsNullOrEmpty(id))
                {
                    getTotalMessageCount = await _signalRMessageService.GetTotalMessageCountByReceiverId(id);
                }
                await Clients.All.SendAsync("ReceiveMessageCount", getTotalMessageCount);
            }
            catch (Exception ex)
            {
                // Hata olursa 0 gönder
                await Clients.All.SendAsync("ReceiveMessageCount", 0);
                Console.WriteLine($"Mesaj sayısı alınırken hata oluştu: {ex.Message}");
            }
        }

        public override async Task OnConnectedAsync()
        {
            // Kullanıcı bağlandığında çalışacak kod
            Console.WriteLine($"Istemci bağlandı: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }
    }
}
