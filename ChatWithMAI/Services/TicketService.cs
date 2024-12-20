using ChatWithMAI.Models;
namespace ChatWithMAI.Services;

using Components;

public interface ITicketService
{
    Task CreateNewTicket(User userModel, Func<User, Task> sendConnectUserToSessionResponse);
}

public class TicketService(IUserMatchingService userMatchingService) : ITicketService
{

    public async Task CreateNewTicket(User userModel, Func<User, Task> sendConnectUserToSessionResponse)
    {
        var ticket = new Ticket(userModel);
        await userMatchingService.AddUserToSearch(ticket, sendConnectUserToSessionResponse);
    }
}

public static class TicketServiceExtensions
{
    public static IServiceCollection AddTicketService(this IServiceCollection services)
    {
        return services.AddSingleton<ITicketService, TicketService>();
    }
}
