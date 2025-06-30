using System.ComponentModel.DataAnnotations;

namespace Utilisateur.Dtos;

public class UserUpdateDto
{
    [Required]
    [MaxLength(256)]
    public string Login { get; set; }

    [Required]
    [MaxLength(256)]
    public string Password { get; set; }

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
    public string Email { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nom { get; set; }

    [Required]
    [MaxLength(100)]
    public string Prenom { get; set; }

    public bool Admin { get; set; }
}