using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Film.Data;

public class FilmsDbContextFactory : IDesignTimeDbContextFactory<FilmsDbContext>
{
    public FilmsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FilmsDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5435;Database=filmdb;Username=postgres;Password=postgres");

        return new FilmsDbContext(optionsBuilder.Options);
    }
}