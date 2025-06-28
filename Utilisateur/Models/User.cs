using System.ComponentModel.DataAnnotations;

namespace Utilisateur.Models;

public class User
{
    [Key]
    public Guid Uuid { get; set; }
    [MaxLength(256)]
    public required string Login { get; set; }
    [MaxLength(256)]
    public required string Password { get; set; }
    [MaxLength(1000)]
    public string? Token { get; set; }
    [MaxLength(1000)]
    public string? OAuthId { get; set; }
    [MaxLength(1000)]
    public string? OAuthToken { get; set; }
    public DateTime TokenGenerationTime { get; set; }
    public DateTime TokenLastUseTime { get; set; }
    public DateTime DateNaissance { get; set; }
    [Required]
    [MaxLength(150)]
    public required string Email { get; set; }
    [Required]
    [MaxLength(100)]
    public required string Nom { get; set; }
    [Required]
    [MaxLength(100)]
    public required string Prenom { get; set; }
    public bool Admin { get; set; }
}