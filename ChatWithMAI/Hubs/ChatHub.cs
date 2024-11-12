using ChatWithMAI.Models;
using ChatWithMAI.Services;
using Microsoft.AspNetCore.SignalR;
namespace ChatWithMAI.Hubs;

public class ChatHub(ISignUpService signUpService, ISessionService sessionService, IAiBotService aiBotService, ITicketService ticketService) : Hub
{
    public async Task SignUp(string user, string connectionId)
    {
        var userModel = signUpService.SignUp(user,connectionId);
        await Clients.Client(connectionId).SendAsync("WaitingForSession");
        ticketService.CreateNewTicket(userModel, SendConnectUserToSessionResponse).GetAwaiter().GetResult();
    }

    private async Task SendConnectUserToSessionResponse(User user)
    {
        await Clients.Client(user.ConnectionId).SendAsync("ConnectedToSession");
    }

    public async Task SendMessage(string connectionId, string message)
    {
        var user = signUpService.GetUserByConnectionId(connectionId);
        if (user is null)
            return;

        var session = sessionService.GetSessionByUser(user);

        if (session.HasAi)
        {
            await Task.Delay(1000);
            var result = await aiBotService.SendChatPromptAsync(message);
            await Clients.Client(user.ConnectionId).SendAsync("ReceiveMessage", user.Username, result.Content);
        }

        var otherUser = session.GetOtherUserIfExists(user);

        if (otherUser != null)
        {
            await Clients.Client(otherUser.ConnectionId).SendAsync("ReceiveMessage", user.Username, message);
        }

    }
}
