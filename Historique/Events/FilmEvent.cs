namespace Historique.Events;

public class FilmEvent
{
    public Guid FilmId { get; set; }
    public string Message { get; set; }
    public string Source { get; set; }
    public string IpPort { get; set; }
    public string Code { get; set; }
}