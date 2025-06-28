using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utilisateur.Data;
using Utilisateur.Models;
using Utilisateur.Events;

namespace Utilisateur.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UtilisateurDbContext _context;
    private readonly IEventPublisher _eventPublisher;

    public UsersController(UtilisateurDbContext context, IEventPublisher eventPublisher)
    {
        _context = context;
        _eventPublisher = eventPublisher;
    }

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();

        _eventPublisher.PublishUserCreated(new UserCreatedEvent
        {
            Message = "Récupération de tous les utilisateurs",
            Source = "utilisateur-service",
            IpPort = "utilisateur:8080",
            Code = "GET_ALL_USERS"
        });

        return users;
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            _eventPublisher.PublishUserCreated(new UserCreatedEvent
            {
                Message = $"Utilisateur non trouvé : {id}",
                Source = "utilisateur-service",
                IpPort = "utilisateur:8080",
                Code = "USER_NOT_FOUND"
            });

            return NotFound();
        }

        _eventPublisher.PublishUserCreated(new UserCreatedEvent
        {
            Message = $"Utilisateur récupéré : {user.Login}",
            Source = "utilisateur-service",
            IpPort = "utilisateur:8080",
            Code = "GET_USER"
        });

        return user;
    }

    // POST: api/users
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        user.Uuid = Guid.NewGuid();
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _eventPublisher.PublishUserCreated(new UserCreatedEvent
        {
            Uuid = user.Uuid,
            Login = user.Login,
            Message = $"Utilisateur créé : {user.Login}",
            Source = "utilisateur-service",
            IpPort = "utilisateur:8080",
            Code = "USER_CREATED"
        });

        return CreatedAtAction(nameof(GetUser), new { id = user.Uuid }, user);
    }

    // PUT: api/users/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, User user)
    {
        if (id != user.Uuid)
        {
            _eventPublisher.PublishUserUpdated(new UserUpdatedEvent()
            {
                Uuid = user.Uuid,
                Login = user.Login,
                Message = $"Échec de modification - Id non correspondant : {id}",
                Source = "utilisateur-service",
                IpPort = "utilisateur:8080",
                Code = "UPDATE_USER_BAD_REQUEST"
            });

            return BadRequest("Id mismatch.");
        }

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();

            _eventPublisher.PublishUserUpdated(new UserUpdatedEvent()
            {
                Uuid = user.Uuid,
                Login = user.Login,
                Message = $"Utilisateur modifié : {user.Login}",
                Source = "utilisateur-service",
                IpPort = "utilisateur:8080",
                Code = "USER_UPDATED"
            });
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                _eventPublisher.PublishUserUpdated(new UserUpdatedEvent()
                {
                    Uuid = user.Uuid,
                    Login = user.Login,
                    Message = $"Utilisateur à modifier non trouvé : {id}",
                    Source = "utilisateur-service",
                    IpPort = "utilisateur:8080",
                    Code = "UPDATE_USER_NOT_FOUND"
                });

                return NotFound();
            }
            else
                throw;
        }

        return NoContent();
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            _eventPublisher.PublishUserDeleted(new UserDeletedEvent()
            {
                Uuid = user.Uuid,
                Login = user.Login,
                Message = $"Utilisateur à supprimer non trouvé : {id}",
                Source = "utilisateur-service",
                IpPort = "utilisateur:8080",
                Code = "DELETE_USER_NOT_FOUND"
            });

            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        _eventPublisher.PublishUserDeleted(new UserDeletedEvent()
        {
            Uuid = user.Uuid,
            Login = user.Login,
            Message = $"Utilisateur supprimé : {user.Login}",
            Source = "utilisateur-service",
            IpPort = "utilisateur:8080",
            Code = "USER_DELETED"
        });

        return NoContent();
    }

    private bool UserExists(Guid id)
    {
        return _context.Users.Any(e => e.Uuid == id);
    }
}