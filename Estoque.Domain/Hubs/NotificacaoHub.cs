using Microsoft.AspNetCore.SignalR;
using Microsoft.Graph.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Estoque.Domain.Hubs
{
    public class NotificacaoHub : Hub
    {
        public NotificacaoHub()
        {

        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await base.OnDisconnectedAsync(ex);
        }
    }

    public static class NotificacaoHubExtensions
    {
        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public static async Task OnNovaCompra(this IHubContext<NotificacaoHub> hub, string mensagem)
        {
            var json = JsonConvert.SerializeObject(new { texto = mensagem }, _jsonSerializerSettings);

            await hub.Clients.All.SendAsync("OnNovaCompra", json);
        }
    }
}
