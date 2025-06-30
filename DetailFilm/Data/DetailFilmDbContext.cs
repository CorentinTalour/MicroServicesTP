using Microsoft.EntityFrameworkCore;

namespace DetailFilm.Data;

public class DetailFilmDbContext : DbContext
{
    public DetailFilmDbContext(DbContextOptions<DetailFilmDbContext> options) : base(options)
    {
    }

    public DbSet<Models.DetailFilm> DetailFilms { get; set; }
}