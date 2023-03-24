using Microsoft.AspNetCore.SignalR;

namespace SharedDependency.Hubs;

public static class UserHandler
{
    public static HashSet<string> ConnectedIds = new HashSet<string>();
}

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        // if exist more 1 client connected end
        if (UserHandler.ConnectedIds.Count > 1)
        {
            return;
        }

        await Clients.All.SendAsync("ReceiveMessage", "hi");
    }

    public override Task OnConnectedAsync()
    {
        Console.WriteLine("OnConnectedAsync");
        UserHandler.ConnectedIds.Add(Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        UserHandler.ConnectedIds.Remove(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}