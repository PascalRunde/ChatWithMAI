using ChatWithMAI.Models;
using Microsoft.AspNetCore.Components;

namespace ChatWithMAI.Services;

public interface IUserMatchingService
{
    public Task AddUserToSearch(Ticket ticket, Func<User, Task> sendConnectUserToSessionResponse);

    public void MatchUserWithAi(User user, Func<User, Task> sendConnectUserToSessionResponse);
}

public class UserMatchingService(ISessionService sessionService) : IUserMatchingService
{
    private readonly Dictionary<User, Ticket> searchingUsers = new ();

    public async Task AddUserToSearch(Ticket ticket, Func<User, Task> sendConnectUserToSessionResponse)
    {
        if (searchingUsers.ContainsKey(ticket.User))
        {
            return;
        }
        var AiCountdown = 150;
        while (AiCountdown > 0)
        {
            if (ticket.IsRedeemed)
            {
                return;
            }

            if (DoesPotentialOpponentExist(ticket))
            {
                var result = await MatchUserWithUser(ticket, sendConnectUserToSessionResponse);
                if (result)
                {
                    return;
                }
            }
            else if(searchingUsers.TryAdd(ticket.User, ticket))
            {
            }
            AiCountdown--;
            await Task.Delay(100);
        }

        ticket.Redeem();
        MatchUserWithAi(ticket.User, sendConnectUserToSessionResponse);
    }

    private bool DoesPotentialOpponentExist(Ticket ticket) => searchingUsers.Where(user => user.Value != ticket).ToArray().Length > 0;


    private async Task<bool> MatchUserWithUser(Ticket ticket, Func<User, Task> sendConnectUserToSessionResponse)
    {
        User? opponent = null;
        var foundMatch = false;
        while(!foundMatch && searchingUsers.Count > 0)
        {
            opponent = searchingUsers.First().Key;
            var opponentTicket = searchingUsers.First().Value;
            searchingUsers.Remove(opponent);
            foundMatch = opponentTicket.Redeem();
        }

        var stillAttends = false;
        if (foundMatch)
        {
            stillAttends = ticket.Redeem();
        }
        if (stillAttends && foundMatch)
        {
            sessionService.AssignToSession(ticket.User, opponent!);
            Dispatcher.CreateDefault().InvokeAsync(() => sendConnectUserToSessionResponse.Invoke(ticket.User));
            Dispatcher.CreateDefault().InvokeAsync(() => sendConnectUserToSessionResponse.Invoke(opponent));
            return true;
        }

        return false;
    }

    public void MatchUserWithAi(User user, Func<User,Task> sendConnectUserToSessionResponse)
    {
        searchingUsers.Remove(user);
        sessionService.AssignToAiSession(user);
        sendConnectUserToSessionResponse.Invoke(user);
    }
}

public static class UserMatchingServiceExtensions
{
    public static IServiceCollection AddUserMatchingService(this IServiceCollection services)
    {
        return services.AddSingleton<IUserMatchingService, UserMatchingService>();
    }
}
