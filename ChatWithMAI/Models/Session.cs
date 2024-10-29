namespace ChatWithMAI.Models;

public class Session(User user)
{
    public Guid Id = Guid.NewGuid();
    private User User1 { get;} = user ?? throw new ArgumentNullException(nameof(user));
    private User? User2 { get; set; }
    
    public bool HasAi { get; private set; }

    private bool HasStarted { get; set; }

    public Session WithUser2(User user)
    {
        User2 = user ?? throw new ArgumentNullException(nameof(user));
        return this;
    }

    public Session WithAi()
    {
        HasAi = true;
        return this;
    }
    public void StartSession()
    {
        HasStarted = true;
    }

    public bool HasUser(User user)
    {
        return User1.Equals(user) || User2 != null && User2.Equals(user);
    }

    public User? GetOtherUserIfExists(User user)
    {
        return User1.Equals(user) ? User2 : User1;
    }

    public bool CanStart()
    {
        return (User2 != null || HasAi) && !HasStarted;
    }
}