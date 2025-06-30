using System.ComponentModel.DataAnnotations;

namespace DetailFilm.Models;

public class DetailFilm
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string DescriptionLongue { get; set; }

    [Required]
    public DateTime DateSortieFilm { get; set; }

    [Required]
    public DateTime DateSortiePlateforme { get; set; }

    [Required]
    public int AgeMinimum { get; set; }

    public string Acteurs { get; set; }

    public string Realisateurs { get; set; }

    [Required]
    public Guid FilmId { get; set; }
}