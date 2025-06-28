using Historique.Data;
using Historique.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Historique.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogsController : ControllerBase
{
    private readonly HistoriqueDbContext _context;

    public LogsController(HistoriqueDbContext context)
    {
        _context = context;
    }

    // GET: api/logs
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Log>>> GetLogs()
    {
        return await _context.Logs.ToListAsync();
    }

    // GET: api/logs/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Log>> GetLog(Guid id)
    {
        var log = await _context.Logs.FindAsync(id);

        if (log == null)
            return NotFound();

        return log;
    }

    // POST: api/logs
    [HttpPost]
    public async Task<ActionResult<Log>> CreateLog(Log log)
    {
        log.Id = Guid.NewGuid();
        _context.Logs.Add(log);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLog), new { id = log.Id }, log);
    }
}