using AnimeAPI.Domain.Entities;

namespace AnimeAPI.Application;

public interface IAnimeRepository
{
    Task<Anime> AddAsync(Anime anime);
    Task<Anime> GetById(int id);
    Task<IEnumerable<Anime>> GetAllAsync(
        string? director = null,
        string? name = null,
        string? summaryKeyword = null,
        int pageIndex = 1,
        int pageSize = 10
        );
    Task UpdateAsync(Anime anime);
    Task DeleteByIdAsync(int id);
}
