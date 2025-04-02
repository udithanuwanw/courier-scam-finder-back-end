namespace courier_scam_finder_back_end.Controllers;
using courier_scam_finder_back_end.Data;
using courier_scam_finder_back_end.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/scammer")]
[ApiController]
public class ScammerController : ControllerBase
{
    private readonly ScamFinderDbContext _context;

    public ScammerController(ScamFinderDbContext context)
    {
        _context = context;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddScammer([FromBody] Scammer scammer)
    {
        _context.Scammers.Add(scammer);
        await _context.SaveChangesAsync();
        return Ok("Scammer added successfully");
    }

    [HttpGet("search/{query}")]
    public async Task<IActionResult> SearchScammer(string query)
    {
        var scammer = await _context.Scammers
            .FirstOrDefaultAsync(s => s.PhoneNumber == query || s.IdNumber == query);

        if (scammer == null)
            return NotFound("No record found");

        return Ok(scammer);
    }
}
