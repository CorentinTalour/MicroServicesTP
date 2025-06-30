namespace Historique.Events;

public class DetailFilmEvent
{
    public string Message { get; set; }
    public string Source { get; set; }
    public string IpPort { get; set; }
    public string Code { get; set; }
}