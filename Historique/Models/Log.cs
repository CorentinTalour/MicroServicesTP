using System.ComponentModel.DataAnnotations;

namespace Historique.Models;

public class Log
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public required string Message { get; set; }

    [Required]
    public required string Source { get; set; }

    [Required]
    public required string IpPort { get; set; }

    [Required]
    public required string Code { get; set; }
    
    public DateTime Date { get; set; } 
}