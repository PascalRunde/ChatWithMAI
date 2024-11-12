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
        // Start AI Cd
        // ADD to searchingUsersList -> Triggers event
        var AiCountdown = 150;
        while (AiCountdown > 0)
        {
            Console.WriteLine($"Acting on ticket for {ticket.User.Username} where ticket is redeemed {ticket.IsRedeemed} on thread {Thread.CurrentThread.ManagedThreadId}");
            if (ticket.IsRedeemed)
            {
                return;
            }

            if (DoesPotentialOpponentExist(ticket))
            {
                Console.WriteLine($"Trying to Match {ticket.User.Username} with other User on thread {Thread.CurrentThread.ManagedThreadId}");
                var result = await MatchUserWithUser(ticket, sendConnectUserToSessionResponse);
                Console.WriteLine($"User {ticket.User.Username} has potential opponent: {result}");
                if (result)
                {
                    return;
                }
            }
            else if(searchingUsers.TryAdd(ticket.User, ticket))
            {
                Console.WriteLine($"User {ticket.User.Username} has no potential opponent");
            }
            AiCountdown--;
            Console.WriteLine($"User {ticket.User.Username} reduces countdown to {AiCountdown}");
            await Task.Delay(100);
        }

        Console.WriteLine($"User {ticket.User.Username} will be connected with Ai and is redeemed: {ticket.IsRedeemed}");
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
            Console.WriteLine($"Trying to redeem from matching for opponent: {opponent.Username} on thread {Thread.CurrentThread.ManagedThreadId}");
            foundMatch = opponentTicket.Redeem();
        }

        var stillAttends = false;
        if (foundMatch)
        {
            Console.WriteLine($"Trying to redeem from matching for self: {ticket.User.Username} on thread {Thread.CurrentThread.ManagedThreadId}");
            stillAttends = ticket.Redeem();
        }
        Console.WriteLine("Before Trying to invoke Connection Response");
        if (stillAttends && foundMatch)
        {
            Console.WriteLine($"Trying to invoke Connection Response from {ticket.User.Username} to {opponent.Username}");
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
