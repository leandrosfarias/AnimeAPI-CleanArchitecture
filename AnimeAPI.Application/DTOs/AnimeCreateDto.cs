namespace AnimeAPI.Application.DTOs;

public class AnimeCreateDto
{
    public required string Name { get; set; }
    public required string Summary { get; set; }
    public required string Director { get; set; }
}

