namespace Utilisateur.Events;

public class UserDeletedEvent
{
    public Guid Uuid { get; set; } 
    public string Login { get; set; }
    public string Message { get; set; }
    public string Source { get; set; }
    public string IpPort { get; set; }
    public string Code { get; set; }
}