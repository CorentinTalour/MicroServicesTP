namespace Film.Events;

public class FilmEvent
{
    public Guid FilmId { get; set; }
    public string Message { get; set; }
    public string Source { get; set; } = "film-service";
    public string IpPort { get; set; } = "film:8080";
    public string Code { get; set; }
}