using AnimeAPI.Domain.Entities;

namespace AnimeAPI.Application.Services;

public class AnimeService : IAnimeService
{
    public IAnimeRepository _animeRepository;

    public AnimeService(IAnimeRepository animeRepository) => _animeRepository = animeRepository;

    public async Task<Anime> AddAsync(Anime anime)
    {
        if (string.IsNullOrEmpty(anime.Name) || string.IsNullOrEmpty(anime.Director))
        {
            throw new ArgumentException("Nome e Diretor são obrigatorios");
        }
        return await _animeRepository.AddAsync(anime);
    }

    public async Task<IEnumerable<Anime>> GetAllAsync(string? director = null, string? name = null, string? summaryKeyword = null, int pageIndex = 1, int pageSize = 10)
    {
        return await _animeRepository.GetAllAsync(director, name, summaryKeyword, pageIndex, pageSize);
    }

    public async Task UpdateAsync(Anime anime)
    {
        if (anime.Id <= 0)
        {
            throw new ArgumentException("ID inválido.");
        }
        await _animeRepository.UpdateAsync(anime);
    }

    public async Task DeleteByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("ID inválido.");
        }
        await _animeRepository.DeleteByIdAsync(id);
    }

    public Task<Anime> GetByIdAsync(int id)
    {
        return _animeRepository.GetById(id);
    }
}
