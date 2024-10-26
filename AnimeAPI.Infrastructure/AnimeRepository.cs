using AnimeAPI.Application;
using AnimeAPI.Domain.Entities;
using AnimeAPI.Infrastructure.Excpetions;
using Microsoft.EntityFrameworkCore;

namespace AnimeAPI.Infrastructure;
public class AnimeRepository : IAnimeRepository
{
    private readonly ApplicationDbContext _context;

    public AnimeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Anime> GetById(int id)
    {
        try
        {
            return await _context.Animes.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
        }
        catch (Exception ex)
        {

            throw new DataAccessException("Erro ao buscar anime por ID", ex);
        }
    }

    async Task<Anime> IAnimeRepository.AddAsync(Anime anime)
    {
        try
        {
            await _context.AddAsync(anime);
            await _context.SaveChangesAsync();
            return anime;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw new DataAccessException("Erro ao adicionar anime no banco de dados", ex);
        }
    }

    async Task IAnimeRepository.DeleteByIdAsync(int id)
    {
        try
        {
            var anime = await _context.FindAsync<Anime>(id);
            if (anime != null) 
            {
                anime.IsDeleted = true;
                _context.Animes.Update(anime);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {

            throw new DataAccessException("Erro ao excluir anime", ex);
        }
    }

    async Task<IEnumerable<Anime>> IAnimeRepository.GetAllAsync(string? director, string? name, string? summaryKeyword, int pageIndex, int pageSize)
    {
        try
        {
            var query = _context.Animes.Where(a => !a.IsDeleted).AsQueryable();

            if (!string.IsNullOrEmpty(director)) 
                query = query.Where(a => a.Director.ToLower().Contains(director.ToLower()));

            if (!string.IsNullOrEmpty(name))
                query = query.Where(a => a.Name.ToLower().Contains(name.ToLower()));

            if (!string.IsNullOrEmpty(summaryKeyword))
                query = query.Where(a => a.Summary!.ToLower().Contains(summaryKeyword.ToLower()));

            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }
        catch (Exception ex)
        {

            throw new DataAccessException("Erro ao listar animes com filtros", ex);
        }
    }

    async Task IAnimeRepository.UpdateAsync(Anime anime)
    {
        try
        {
            _context.Update(anime);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            throw new DataAccessException("Erro ao atualizar anime", ex);
        }
    }
}
