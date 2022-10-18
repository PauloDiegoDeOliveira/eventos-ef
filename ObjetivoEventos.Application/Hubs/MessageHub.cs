using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ObjetivoEventos.Application.Dtos.Reserva;
using ObjetivoEventos.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.Hubs
{
    public class MessageHub : Hub
    {
        private readonly IHubContext<MessageHub> hubContext;
        private readonly IServiceScope scope;

        public MessageHub(IServiceProvider serviceProvider,
                          IHubContext<MessageHub> hubContext)
        {
            scope = serviceProvider.CreateScope();
            this.hubContext = hubContext;
        }

        public async Task SendMessage(ViewReservaDto reserva)
        {
            JsonSerializerSettings jsonSerializerSettings = new();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            await hubContext.Clients.Groups(reserva.EventoId.ToString()).SendAsync("ReceiveMessage",
                JsonConvert.SerializeObject(reserva, jsonSerializerSettings));
        }

        public async Task JoinGroup(string idEvento)
        {
            await hubContext.Groups.AddToGroupAsync(Context.ConnectionId, idEvento);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            IReservaApplication reserva = scope.ServiceProvider.GetRequiredService<IReservaApplication>();
            List<ViewReservaDto> listReservas = await reserva.DeleteByConnectionId(Context.ConnectionId);

            foreach (ViewReservaDto viewReservaDto in listReservas)
            {
                await SendMessage(viewReservaDto);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}