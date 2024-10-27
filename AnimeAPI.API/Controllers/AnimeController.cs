using AnimeAPI.Domain.Entities;
using AnimeAPI.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AnimeAPI.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AnimeController : ControllerBase
{
    private readonly IAnimeService _animeService;
    private readonly ILogger<AnimeController> _logger;

    public AnimeController(IAnimeService animeService, ILogger<AnimeController> logger)
    {
        _animeService = animeService;
        _logger = logger;
    }

    // POST: api/v1/Anime
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AnimeCreateDto animeDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Tentativa de criação com dados inválidos.");
            return BadRequest(ModelState);
        }

        var existingAnime = await _animeService.FindByNameAsync(animeDto.Name);
        if (existingAnime != null)
        {
            _logger.LogWarning("Conflito: Um anime com o nome {AnimeName} já existe.", animeDto.Name);
            return Conflict(new { message = "Anime com esse nome já existe." });
        }

        try
        {
            _logger.LogInformation("Iniciando a criação de um novo anime.");
            var anime = new Anime() { Name = animeDto.Name, Director = animeDto.Director, Summary = animeDto.Summary };
            var createdAnime = await _animeService.AddAsync(anime);

            var responseDto = new AnimeResponseDto
            {
                Id = createdAnime.Id,
                Name = createdAnime.Name,
                Director = createdAnime.Director,
                Summary = createdAnime.Summary!
            };
            _logger.LogInformation("Anime {AnimeName} criado com sucesso com ID {AnimeId}.", createdAnime.Name, createdAnime.Id);
            return CreatedAtAction(nameof(GetById), new { id = createdAnime.Id }, responseDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar o anime {AnimeName}.", animeDto.Name);
            return StatusCode(500, new { message = "Ocorreu um erro ao processar a requisição." });
        }
    }

    // GET: api/v1/Animes/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("Iniciando busca pelo anime com ID {Id}.", id);

        try
        {
            var anime = await _animeService.GetByIdAsync(id);

            if (anime == null)
            {
                _logger.LogWarning("Anime com ID {Id} não encontrado.", id);
                return NotFound(new { message = $"Anime com ID {id} não foi encontrado." });
            }

            var responseDto = new AnimeResponseDto
            {
                Id = anime.Id,
                Name = anime.Name,
                Director = anime.Director,
                Summary = anime.Summary!
            };

            _logger.LogInformation("Anime com ID {Id} encontrado: {AnimeName}.", id, anime.Name);
            return Ok(responseDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar anime com ID {Id}.", id);
            return StatusCode(500, new { message = "Ocorreu um erro ao processar a requisição." });
        }
    }

    // GET: api/animes
    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] string? director,
    [FromQuery] string? name,
    [FromQuery] string? summaryKeyword,
    int pageIndex = 1,
    int pageSize = 10)
    {
        _logger.LogInformation("Iniciando busca de animes com filtros: Diretor={Director}, Nome={Name}, Palavra-Chave no Resumo={SummaryKeyword}, Página={PageIndex}, Tamanho da Página={PageSize}.",
                               director, name, summaryKeyword, pageIndex, pageSize);

        try
        {
            var animes = await _animeService.GetAllAsync(director, name, summaryKeyword, pageIndex, pageSize);

            if (animes == null || !animes.Any())
            {
                _logger.LogWarning("Nenhum anime encontrado com os filtros aplicados.");
                return NotFound(new { message = "Nenhum anime encontrado com os filtros aplicados." });
            }

            var animeDtos = animes.Select(anime => new AnimeResponseDto
            {
                Id = anime.Id,
                Name = anime.Name,
                Director = anime.Director,
                Summary = anime.Summary!
            });

            _logger.LogInformation("Busca de animes concluída. {Count} resultados encontrados.", animeDtos.Count());
            return Ok(new
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItems = animeDtos.Count(),
                Data = animeDtos
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar animes com os filtros: Diretor={Director}, Nome={Name}, Palavra-Chave no Resumo={SummaryKeyword}.",
                             director, name, summaryKeyword);
            return StatusCode(500, new { message = "Ocorreu um erro ao processar a requisição." });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AnimeUpdateDto animeDto)
    {
        _logger.LogInformation("Iniciando atualização do anime com ID {Id}.", id);

        try
        {
            var existingAnime = await _animeService.GetByIdAsync(id);
            if (existingAnime == null)
            {
                _logger.LogWarning("Anime com ID {Id} não encontrado para atualização.", id);
                return NotFound(new { message = $"Anime com ID {id} não foi encontrado." });
            }

            existingAnime.Name = animeDto.Name;
            existingAnime.Director = animeDto.Director;
            existingAnime.Summary = animeDto.Summary;

            await _animeService.UpdateAsync(existingAnime);
            _logger.LogInformation("Anime com ID {Id} atualizado com sucesso.", id);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar o anime com ID {Id}.", id);
            return StatusCode(500, new { message = "Ocorreu um erro ao processar a requisição." });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Iniciando exclusão do anime com ID {Id}.", id);

        try
        {
            var existingAnime = await _animeService.GetByIdAsync(id);
            if (existingAnime == null)
            {
                _logger.LogWarning("Anime com ID {Id} não encontrado para exclusão.", id);
                return NotFound(new { message = $"Anime com ID {id} não foi encontrado." });
            }

            await _animeService.DeleteByIdAsync(id);
            _logger.LogInformation("Anime com ID {Id} excluído com sucesso.", id);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir o anime com ID {Id}.", id);
            return StatusCode(500, new { message = "Ocorreu um erro ao processar a requisição." });
        }
    }
}

