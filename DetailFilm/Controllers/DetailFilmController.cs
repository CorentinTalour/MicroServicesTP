using DetailFilm.Data;
using DetailFilm.Dtos;
using DetailFilm.Events;
using Microsoft.AspNetCore.Mvc;

namespace DetailFilmService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetailFilmController : ControllerBase
    {
        private readonly DetailFilmDbContext _context;
        private readonly IRabbitMQProducer _rabbitMQProducer;

        public DetailFilmController(DetailFilmDbContext context, IRabbitMQProducer rabbitMQProducer)
        {
            _context = context;
            _rabbitMQProducer = rabbitMQProducer;
        }

        // GET by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<DetailFilm.Models.DetailFilm>> GetDetailFilm(Guid id)
        {
            var detailFilm = await _context.DetailFilms.FindAsync(id);
            if (detailFilm == null)
                return NotFound();

            return Ok(detailFilm);
        }

        // POST - Create
        [HttpPost]
        public async Task<ActionResult<DetailFilm.Models.DetailFilm>> CreateDetailFilm(DetailFilm.Models.DetailFilm detailFilm)
        {
            detailFilm.Id = Guid.NewGuid();
            _context.DetailFilms.Add(detailFilm);
            await _context.SaveChangesAsync();

            SendLog($"Détail film créé: {detailFilm.Id}", 201);

            return CreatedAtAction(nameof(GetDetailFilm), new { id = detailFilm.Id }, detailFilm);
        }

        // PUT - Update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDetailFilm(Guid id, DetailFilm.Models.DetailFilm detailFilm)
        {
            if (id != detailFilm.Id)
                return BadRequest("L'ID fourni ne correspond pas.");

            var existingDetail = await _context.DetailFilms.FindAsync(id);
            if (existingDetail == null)
                return NotFound();

            existingDetail.DescriptionLongue = detailFilm.DescriptionLongue;
            existingDetail.DateSortieFilm = detailFilm.DateSortieFilm;
            existingDetail.DateSortiePlateforme = detailFilm.DateSortiePlateforme;
            existingDetail.Acteurs = detailFilm.Acteurs;
            existingDetail.Realisateurs = detailFilm.Realisateurs;
            existingDetail.FilmId = detailFilm.FilmId;

            await _context.SaveChangesAsync();

            SendLog($"Détail film modifié: {detailFilm.Id}", 200);

            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetailFilm(Guid id)
        {
            var detailFilm = await _context.DetailFilms.FindAsync(id);
            if (detailFilm == null)
                return NotFound();

            _context.DetailFilms.Remove(detailFilm);
            await _context.SaveChangesAsync();

            SendLog($"Détail film supprimé: {id}", 204);

            return NoContent();
        }

        private void SendLog(string message, int code)
        {
            try
            {
                var logEvent = new LogEvent
                {
                    Message = message,
                    Source = "DetailFilmService",
                    IpPort = "detailfilm:8084",
                    Code = code
                };
                _rabbitMQProducer.SendMessage(logEvent, "ms.detailfilm", "detailfilm.event");
            }
            catch (Exception ex)
            {

            }
        }
    }
}