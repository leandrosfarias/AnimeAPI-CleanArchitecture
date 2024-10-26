using AnimeAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AnimeAPI.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AnimeController : ControllerBase
{
    private readonly IAnimeService _animeService;

    public AnimeController(IAnimeService animeService) => _animeService = animeService;

    // POST: api/animes
    [HttpPost]
    public async Task<IActionResult> Create(Anime anime)
    {
        anime.Id = 0;
        var createdAnime = await _animeService.AddAsync(anime);
        return CreatedAtAction(nameof(GetById), new { id = createdAnime.Id }, createdAnime);
    }

    // GET: api/animes/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var anime = await _animeService.GetByIdAsync(id);

        if (anime == null)
        {
            return NotFound();
        }
        return Ok(anime);
    }

    // GET: api/animes
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? director, [FromQuery] string? name, [FromQuery] string? summaryKeyword, int pageIndex = 1, int pageSize = 10)
    {
        var animes = await _animeService.GetAllAsync(director, name, summaryKeyword, pageIndex, pageSize);
        return Ok(animes);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Anime anime)
    {
        anime.Id = id;
        await _animeService.UpdateAsync(anime);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _animeService.DeleteByIdAsync(id);
        return NoContent();
    }
}

