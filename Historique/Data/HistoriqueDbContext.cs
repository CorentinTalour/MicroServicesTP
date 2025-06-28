using Historique.Models;
using Microsoft.EntityFrameworkCore;

namespace Historique.Data;

public class HistoriqueDbContext : DbContext
{
    public HistoriqueDbContext(DbContextOptions<HistoriqueDbContext> options) : base(options)
    {
    }

    public DbSet<Log> Logs { get; set; }
}