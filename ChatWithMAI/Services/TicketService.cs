using ChatWithMAI.Models;
namespace ChatWithMAI.Services;

public interface ITicketService
{
    Task CreateNewTicket(User userModel, Func<User, Task> sendConnectUserToSessionResponse);
}

public class TicketService(IUserMatchingService userMatchingService) : ITicketService
{

    public Task CreateNewTicket(User userModel, Func<User, Task> sendConnectUserToSessionResponse)
    {
        var ticket = new Ticket(userModel);
        var ticketTask = ticket.Run().ContinueWith(_ => ResolveTicketWithAiMatch(userModel,sendConnectUserToSessionResponse));
        var userMatchingTask = userMatchingService.AddUserToSearch(ticket, sendConnectUserToSessionResponse);
        return Task.WhenAll(new []{
            ticketTask, userMatchingTask
        });
        
    }

    private void ResolveTicketWithAiMatch(User user, Func<User, Task> sendConnectUserToSessionResponse)
    {
        userMatchingService.MatchUserWithAi(user, sendConnectUserToSessionResponse);
    }
}

public static class TicketServiceExtensions
{
    public static IServiceCollection AddTicketService(this IServiceCollection services)
    {
        return services.AddSingleton<ITicketService, TicketService>();
    }
}