using ChatWithMAI.Models;

namespace ChatWithMAI.Services;

public interface ISessionService
{
    public void AssignToSession(User user, User user2);
    public void AssignToAiSession(User user);

    public Session? GetSessionByUser(User user);
}

public class SessionService: ISessionService
{
    private readonly List<Session> sessions = [];

    private Session CreateNewSession(User user)
    {
        var session = new Session(user);
        sessions.Add(session);
        return session;
    }
    public void AssignToSession(User user, User user2)
    {
        var session = CreateNewSession(user).WithUser2(user2);
        session.StartSession();
    }

    public void AssignToAiSession(User user)
    {
        var session = CreateNewSession(user).WithAi();
        session.StartSession();
    }

    public Session? GetSessionByUser(User user) => sessions.FirstOrDefault(session => session.HasUser(user));
}

public static class SessionServiceExtensions
{
    public static IServiceCollection AddSessionService(this IServiceCollection services)
    {
        return services.AddSingleton<ISessionService, SessionService>();
    }
}
