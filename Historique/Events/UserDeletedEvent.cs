namespace Historique.Events;

public class UserDeletedEvent
{
    public Guid Uuid { get; set; }
    public string Login { get; set; }
}