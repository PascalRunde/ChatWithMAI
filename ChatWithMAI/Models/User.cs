namespace ChatWithMAI.Models;

public record User(string Username, string ConnectionId)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public bool IsPlaying { get; set; } = false;

    public string Username { get; } = Username ?? throw new ArgumentNullException(nameof(Username));
    public string ConnectionId { get; } = ConnectionId ?? throw new ArgumentNullException(nameof(ConnectionId));
}
