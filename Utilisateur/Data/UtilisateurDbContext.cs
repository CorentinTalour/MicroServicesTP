using Microsoft.EntityFrameworkCore;
using Utilisateur.Models;

namespace Utilisateur.Data;

public class UtilisateurDbContext : DbContext
{
    public UtilisateurDbContext(DbContextOptions<UtilisateurDbContext> options)
        : base(options)
    {
    }

    // Expose les tables de la base de données
    public DbSet<User> Users { get; set; }
}