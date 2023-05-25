using Microsoft.AspNetCore.SignalR;
using SignalR_Eksempel.Services;

namespace SignalR_Eksempel.Hubs
{
    public class ChatHub(ILogger<ChatHub> Logger,UserManager UserManager,ChatFilterService chatFilter) : Hub
    {

        public override Task OnConnectedAsync()
        {
            Logger.LogInformation("Client Connected to hub");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Logger.LogInformation("Client disconnected from the hub");

            var user = UserManager.GetUserbyId(Context.ConnectionId);
            await Clients.Others.SendAsync("OnUserLogout", user.UserName);

            UserManager.RemoveUser(Context.ConnectionId);
            Logger.LogInformation("User session removed..");

            if (exception != null)
            {
                Logger.LogError(exception.Message);
                Logger.LogError(exception.StackTrace);
            }
        }

        public async Task Login(string UserName)
        {
            try
            {
                UserManager.CreateUser(UserName, Context.ConnectionId);

                Logger.LogInformation($"User {UserName} Logged in with Connection {Context.ConnectionId}");
                await Clients.Caller.SendAsync("ReceiveUserACK");
                await Clients.Others.SendAsync("OnUserLogin", UserName);

                foreach (var user in UserManager.GetUsers())
                {
                    if (user.UserName == UserName)
                    {
                        continue;
                    }
                    await Clients.Caller.SendAsync("OnUserLogin", user.UserName);
                }

            }
            catch (Exception e)
            {
                Logger.LogWarning($"Failed to login {UserName}: {e.Message}");
                await Clients.Caller.SendAsync("ReceiveUserNACK");
            }
        }

        public async Task SendMessage(string user,string message)
        {
            await chatFilter.RunFilterCheck(Context.ConnectionId, message);
            await Clients.Others.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMessageTo(string Sender, string reciever, string message)
        {
            try
            {
                var user = UserManager.GetUser(reciever);
                var userClient = Clients.Client(user.UserId);
                await chatFilter.RunFilterCheck(user.UserId, message);
                await userClient.SendAsync("ReceivePrivateMessage", Sender, message);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
