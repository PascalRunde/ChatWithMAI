namespace ChatWithMAI.Services;

public interface IMessageRoutingService
{
    
}

public class MessageRoutingService: IMessageRoutingService
{
    
}

public static class MessageRoutingServiceExtensions
{
    public static IServiceCollection AddMessageRoutingService(this IServiceCollection services)
    {
        return services.AddTransient<IMessageRoutingService, MessageRoutingService>();
    }
}