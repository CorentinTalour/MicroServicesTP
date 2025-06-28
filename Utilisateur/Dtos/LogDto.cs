namespace Utilisateur.Dtos;

public class LogDto
{
    public Guid IdLog { get; set; }
    public string Message { get; set; }
    public string Source { get; set; }
    public string IpPort { get; set; }
    public string Code { get; set; }
}