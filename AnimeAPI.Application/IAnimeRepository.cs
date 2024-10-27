using AnimeAPI.Domain.Entities;

namespace AnimeAPI.Application;

public interface IAnimeRepository
{
    Task<Anime> AddAsync(Anime anime);
    Task<Anime> GetByIdAsync(int id);
    Task<IEnumerable<Anime>> GetAllAsync(
        string? director = null,
        string? name = null,
        string? summaryKeyword = null,
        int pageIndex = 1,
        int pageSize = 10
        );
    Task<Anime> FindByNameAsync(string name);
    Task UpdateAsync(Anime anime);
    Task DeleteByIdAsync(int id);
}
