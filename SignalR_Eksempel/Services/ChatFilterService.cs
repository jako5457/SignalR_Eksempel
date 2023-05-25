using Microsoft.AspNetCore.SignalR;
using SignalR_Eksempel.Hubs;

namespace SignalR_Eksempel.Services
{
    public class ChatFilterService(IHubContext<ChatHub> hub)
    {

        List<string> BannedWords = new List<string> { "fuck", "ass", "stink", "smell" };

        public async Task RunFilterCheck(string clientId,string text)
        {
            foreach (var t in BannedWords)
            {
                if (text.Contains(t))
                {
                    await hub.Clients.Client(clientId).SendAsync("ReceivePrivateMessage", "SYSTEM", "Please do not speak in that way again!");
                }
            }


        }

    }
}
