namespace AnimeAPI.Application.DTOs;

public class AnimeResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Director { get; set; }
    public required string Summary { get; set; }
}
