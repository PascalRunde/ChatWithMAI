namespace ChatWithMAI.Models;

public class Ticket(User user)
{
    public Guid Id = Guid.NewGuid();
    public User User { get; } = user?? throw new ArgumentNullException(nameof(user));
    private bool redeemingRequested;
    public bool IsRedeemed { get; private set; }

    public async Task Run()
    {
        var counter = 0;
        while (!redeemingRequested)
        {
            if (counter >= 150)
            {
                IsRedeemed = true;
                return;
            }
            counter++;
            
            await Task.Delay(100);
        }
        IsRedeemed = true;
    }

    public async Task<bool> Redeem()
    {
        Console.WriteLine("Redeeming...");
        if (IsRedeemed)
        {
            throw new InvalidOperationException("Ticket is already redeemed");
        }
        redeemingRequested = true;
        await Task.Delay(500);
        
        return IsRedeemed;
    }
}