namespace ChatWithMAI.Models;

public class Ticket(User user)
{
    public Guid Id = Guid.NewGuid();
    public User User { get; } = user?? throw new ArgumentNullException(nameof(user));
    private bool redeemingRequested;
    public bool IsRedeemed { get; private set; }

    public bool IsRedeemingRequested() => redeemingRequested;

    public bool Redeem()
    {
        Console.WriteLine($"Redeeming for {User.Username}");
        if (IsRedeemed)
        {
            throw new InvalidOperationException("Ticket is already redeemed");
        }
        redeemingRequested = true;
        return IsRedeemed;
    }


}
