namespace Utilisateur.Events;

public class UserAccessedEvent
{
    public string Message { get; set; }
    public string Source { get; set; }
    public string IpPort { get; set; }
    public string Code { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
}