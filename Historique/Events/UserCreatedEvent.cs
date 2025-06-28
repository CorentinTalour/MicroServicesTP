namespace Historique.Events;

public class UserCreatedEvent
{
    public Guid Uuid { get; set; }
    public string Login { get; set; }
}