namespace Utilisateur.Events;

public class UserCreatedEvent
{
    public string Message { get; set; }
    public string Source { get; set; }
    public string IpPort { get; set; }
    public string Code { get; set; }
}