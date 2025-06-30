using Microsoft.EntityFrameworkCore;

namespace Film.Data;


public class FilmsDbContext : DbContext
{
    public FilmsDbContext(DbContextOptions<FilmsDbContext> options) : base(options) { }

    public DbSet<Models.Film> Films { get; set; }
}