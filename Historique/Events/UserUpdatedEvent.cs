namespace Historique.Events;

public class UserUpdatedEvent
{
    public Guid Uuid { get; set; }
    public string Login { get; set; }
}