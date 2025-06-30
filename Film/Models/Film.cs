using System.ComponentModel.DataAnnotations;

namespace Film.Models;

public class Film
{
    [Key] 
    public Guid Id { get; set; }

    [Required] [MaxLength(256)] 
    public string Titre { get; set; }

    [MaxLength(500)] 
    public string Description { get; set; }

    [Required] [MaxLength(100)] 
    public string Categorie { get; set; }

    [Required] 
    public decimal Prix { get; set; }
}