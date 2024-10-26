using AnimeAPI.Domain.Entities;

public interface IAnimeService
{
    Task<Anime> AddAsync(Anime anime);
    Task<Anime> GetByIdAsync(int id);
    Task<IEnumerable<Anime>> GetAllAsync(string? director = null, string? name = null, string? summaryKeyword = null, int pageIndex = 1, int pageSize = 10);
    Task UpdateAsync(Anime anime);
    Task DeleteByIdAsync(int id);
}

