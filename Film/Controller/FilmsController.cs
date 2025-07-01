using Film.Clients;
using Film.Data;
using Film.Dtos;
using Film.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Film.Controller;

[ApiController]
[Route("api/[controller]")]
public class FilmsController : ControllerBase
{
    private readonly FilmsDbContext _context;
    private readonly IEventPublisher _eventPublisher;
    private readonly DetailFilmClient _detailFilmClient;

    public FilmsController(FilmsDbContext context, IEventPublisher eventPublisher, DetailFilmClient detailFilmClient)
    {
        _context = context;
        _eventPublisher = eventPublisher;
        _detailFilmClient = detailFilmClient;
    }

    // POST: api/films
    [HttpPost]
    public async Task<ActionResult<Models.Film>> CreateFilm(Models.Film film)
    {
        film.Id = Guid.NewGuid();
        _context.Films.Add(film);
        await _context.SaveChangesAsync();

        _eventPublisher.PublishFilmEvent(new FilmEvent
        {
            FilmId = film.Id,
            Message = $"Film créé : {film.Titre}",
            Code = "FILM_CREATED"
        });

        return CreatedAtAction(nameof(GetFilmById), new { id = film.Id }, film);
    }

    // PUT: api/films/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFilm(Guid id, Models.Film film)
    {
        if (id != film.Id)
        {
            _eventPublisher.PublishFilmEvent(new FilmEvent
            {
                FilmId = id,
                Message = $"Échec de modification - ID non correspondant : {id}",
                Code = "UPDATE_FILM_BAD_REQUEST"
            });

            return BadRequest("ID mismatch");
        }

        _context.Entry(film).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();

            _eventPublisher.PublishFilmEvent(new FilmEvent
            {
                FilmId = film.Id,
                Message = $"Film modifié : {film.Titre}",
                Code = "FILM_UPDATED"
            });
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FilmExists(id))
            {
                _eventPublisher.PublishFilmEvent(new FilmEvent
                {
                    FilmId = id,
                    Message = $"Film à modifier non trouvé : {id}",
                    Code = "UPDATE_FILM_NOT_FOUND"
                });

                return NotFound();
            }
            else
                throw;
        }

        return NoContent();
    }

    // DELETE: api/films/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFilm(Guid id)
    {
        var film = await _context.Films.FindAsync(id);
        if (film == null)
        {
            _eventPublisher.PublishFilmEvent(new FilmEvent
            {
                FilmId = id,
                Message = $"Film à supprimer non trouvé : {id}",
                Code = "DELETE_FILM_NOT_FOUND"
            });

            return NotFound();
        }

        _context.Films.Remove(film);
        await _context.SaveChangesAsync();

        _eventPublisher.PublishFilmEvent(new FilmEvent
        {
            FilmId = film.Id,
            Message = $"Film supprimé : {film.Titre}",
            Code = "FILM_DELETED"
        });

        return NoContent();
    }

    // GET: api/films
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Models.Film>>> GetAllFilms()
    {
        var films = await _context.Films.ToListAsync();

        _eventPublisher.PublishFilmEvent(new FilmEvent
        {
            Message = "Récupération de tous les films",
            Code = "GET_ALL_FILMS"
        });

        return films;
    }

    // GET: api/films/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Models.Film>> GetFilmById(Guid id)
    {
        var film = await _context.Films.FindAsync(id);

        if (film == null)
        {
            _eventPublisher.PublishFilmEvent(new FilmEvent
            {
                FilmId = id,
                Message = $"Film non trouvé : {id}",
                Code = "FILM_NOT_FOUND"
            });

            return NotFound();
        }

        _eventPublisher.PublishFilmEvent(new FilmEvent
        {
            FilmId = film.Id,
            Message = $"Film récupéré : {film.Titre}",
            Code = "GET_FILM"
        });

        return film;
    }

    // GET: api/films/{id}/details
    [HttpGet("{id}/details")]
    public async Task<ActionResult<FilmDetailDto>> GetFilmWithDetails(Guid id)
    {
        var film = await _context.Films.FindAsync(id);
        if (film == null)
        {
            _eventPublisher.PublishFilmEvent(new FilmEvent
            {
                FilmId = id,
                Message = $"Film (avec détails) non trouvé : {id}",
                Code = "FILM_DETAILS_NOT_FOUND"
            });
            return NotFound();
        }

        var detailFilm = await _detailFilmClient.GetDetailFilmAsync(id);
        if (detailFilm == null)
        {
            _eventPublisher.PublishFilmEvent(new FilmEvent
            {
                FilmId = id,
                Message = $"Détail film non trouvé dans DetailFilm service : {id}",
                Code = "DETAILFILM_NOT_FOUND"
            });

            return Ok(new FilmDetailDto
            {
                Id = film.Id,
                Titre = film.Titre,
                Description = film.Description,
                Categorie = film.Categorie,
                Prix = film.Prix,
                DescriptionLongue = string.Empty,
                Acteurs = new List<string>(),
                Realisateurs = new List<string>()
            });
        }

        _eventPublisher.PublishFilmEvent(new FilmEvent
        {
            FilmId = film.Id,
            Message = $"Film (avec détails) récupéré : {film.Titre}",
            Code = "GET_FILM_DETAILS"
        });

        var result = new FilmDetailDto
        {
            Id = film.Id,
            Titre = film.Titre,
            Description = film.Description,
            Categorie = film.Categorie,
            Prix = film.Prix,
            DescriptionLongue = detailFilm.DescriptionLongue,
            Acteurs = detailFilm.Acteurs,
            Realisateurs = detailFilm.Realisateurs
        };

        return Ok(result);
    }

    private bool FilmExists(Guid id)
    {
        return _context.Films.Any(f => f.Id == id);
    }
}