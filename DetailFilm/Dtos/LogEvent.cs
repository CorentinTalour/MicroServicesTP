namespace DetailFilm.Dtos;

public class LogEvent
{
    public string Message { get; set; }
    public string Source { get; set; }
    public string IpPort { get; set; }
    public int Code { get; set; }
}