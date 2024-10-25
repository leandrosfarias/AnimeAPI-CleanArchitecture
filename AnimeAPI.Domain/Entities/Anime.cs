namespace AnimeAPI.Domain.Entities;

public class Anime
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Summary { get; set; }
    public required string Director { get; set; }
    public bool IsDeleted { get; set; }
}
