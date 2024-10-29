using ChatWithMAI.Models;

namespace ChatWithMAI.Services;

public interface ISignUpService
{
    public User SignUp(string username, string connectionId);
    public User? GetUserByConnectionId(string connectionId);
}

public class SignUpService: ISignUpService
{
    private readonly List<User?> _signedUsers = new();
    public User SignUp(string username, string connectionId)
    {
        var user = new User(username, connectionId);
        _signedUsers.Add(user);
        return user;
    }

    public User? GetUserByConnectionId(string connectionId)
    {
        return _signedUsers.FirstOrDefault(u => u.ConnectionId == connectionId);
    }

    public int GetUserCount() => _signedUsers.Count;


}

public static class SignUpServiceExtension
{
    public static IServiceCollection AddSignUpService(this IServiceCollection services)
    {
        return services.AddSingleton<ISignUpService, SignUpService>();
    }
}