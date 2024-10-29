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
        Console.WriteLine($"Adding user {ticket.User.Username}");
        if (searchingUsers.ContainsKey(ticket.User))
        {
            return;
        }
        Console.WriteLine("Found " + searchingUsers.Count + " users.");
        if (searchingUsers.Count > 0)
        {
            User? opponent = null;
            var foundMatch = false;
            while(!foundMatch)
            {
                opponent = searchingUsers.First().Key;
                var opponentTicket = searchingUsers.First().Value;
                searchingUsers.Remove(opponent);
                foundMatch = await opponentTicket.Redeem();
            }

            var stillAttends = false;
            if (foundMatch)
            {
                stillAttends = await ticket.Redeem();
            }
            Console.WriteLine("Before Trying to invoke Connection Response");
            if (stillAttends && foundMatch)
            {
                Console.WriteLine("Trying to invoke Connection Response");
                sessionService.AssignToSession(ticket.User, opponent!);
                Dispatcher.CreateDefault().InvokeAsync(() => sendConnectUserToSessionResponse.Invoke(ticket.User));
                Dispatcher.CreateDefault().InvokeAsync(() => sendConnectUserToSessionResponse.Invoke(opponent));
            }
        }
        else
        {
            searchingUsers.Add(ticket.User, ticket);
        }
    }

    public void MatchUserWithAi(User user, Func<User,Task> sendConnectUserToSessionResponse)
    {
        sessionService.AssignToAISession(user);
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