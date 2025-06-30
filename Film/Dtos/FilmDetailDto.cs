namespace Film.Dtos;

public class FilmDetailDto
{
    public Guid Id { get; set; }
    public string Titre { get; set; }
    public string Description { get; set; }
    public string Categorie { get; set; }
    public decimal Prix { get; set; }

    // Pour le microservice DétailsFilm
    public string DescriptionLongue { get; set; } = string.Empty;
    public List<string> Acteurs { get; set; } = new();
    public List<string> Realisateurs { get; set; } = new();
}