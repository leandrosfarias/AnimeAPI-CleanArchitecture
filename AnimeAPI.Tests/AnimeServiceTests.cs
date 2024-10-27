using AnimeAPI.Application;
using AnimeAPI.Application.Services;
using AnimeAPI.Domain.Entities;
using Moq;

namespace AnimeAPI.Tests;

public class AnimeServiceTests
{
    private readonly Mock<IAnimeRepository> _animeRepositoryMock;
    private readonly IAnimeService _animeService;

    public AnimeServiceTests()
    {
        _animeRepositoryMock = new Mock<IAnimeRepository>();
        _animeService = new AnimeService(_animeRepositoryMock.Object);
    }

    [Fact]
    public async void AddAsync_ShouldCallAdd_WhenAnimeIsValid()
    {
        // Arrange
        var newAnime = new Anime { Name = "Naruto", Director = "Masashi Kishimot" };

        // Act
        await _animeService.AddAsync(newAnime);

        // Assert
        _animeRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Anime>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAnime_WhenAnimeExists()
    {
        // Arrange
        var anime = new Anime { Id = 1, Name = "Dragon Ball", Director = "Akira Toriyama" };
        _animeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(anime);

        // Act
        var result = await _animeService.GetByIdAsync(1);

        // Assert
        Assert.Equal(anime, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenAnimeDoesNotExist()
    {
        // Arrange
        _animeRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Anime)null);

        // Act
        var result = await _animeService.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAnimes_WhenNoFiltersProvided()
    {
        // Arrange
        var animes = new List<Anime>
        {
            new() { Id = 1, Name = "Naruto", Director = "Masashi Kishimoto", Summary = "A ninja story" },
            new() { Id = 2, Name = "Dragon Ball", Director = "Akira Toriyama", Summary = "A martial arts journey" }
        };

        _animeRepositoryMock.Setup(r => r.GetAllAsync(null, null, null, 1, 10)).ReturnsAsync(animes);

        // Act
        var result = await _animeService.GetAllAsync(null, null, null, 1, 10);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, a => a.Name == "Naruto");
        Assert.Contains(result, a => a.Name == "Dragon Ball");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAnimes_WhenNameFilterProvided()
    {
        // Arrange
        var animes = new List<Anime>
    {
        new() { Id = 1, Name = "Naruto", Director = "Masashi Kishimoto", Summary = "A ninja story" }
    };

        _animeRepositoryMock.Setup(r => r.GetAllAsync(null, "Naruto", null, 1, 10)).ReturnsAsync(animes);

        // Act
        var result = await _animeService.GetAllAsync(null, "Naruto", null, 1, 10);

        // Assert
        Assert.Single(result);
        Assert.Contains(result, a => a.Name == "Naruto");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAnimes_WhenDirectorFilterProvided()
    {
        // Arrange
        var animes = new List<Anime>
    {
        new() { Id = 1, Name = "Naruto", Director = "Masashi Kishimoto", Summary = "A ninja story" }
    };

        _animeRepositoryMock.Setup(r => r.GetAllAsync("Masashi Kishimoto", null, null, 1, 10)).ReturnsAsync(animes);

        // Act
        var result = await _animeService.GetAllAsync("Masashi Kishimoto", null, null, 1, 10);

        // Assert
        Assert.Single(result);
        Assert.Contains(result, a => a.Director == "Masashi Kishimoto");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAnimes_WhenSummaryKeywordFilterProvided()
    {
        // Arrange
        var animes = new List<Anime>
    {
        new() { Id = 1, Name = "Naruto", Director = "Masashi Kishimoto", Summary = "A ninja story" }
    };

        _animeRepositoryMock.Setup(r => r.GetAllAsync(null, null, "ninja", 1, 10)).ReturnsAsync(animes);

        // Act
        var result = await _animeService.GetAllAsync(null, null, "ninja", 1, 10);

        // Assert
        Assert.Single(result);
        Assert.Contains(result, a => a.Summary.Contains("ninja"));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnCorrectPageOfAnimes_WhenPaginationIsApplied()
    {
        // Arrange
        var animesPage1 = new List<Anime>
    {
        new () { Id = 1, Name = "Naruto", Director = "Masashi Kishimoto", Summary = "A ninja story" }
    };

        var animesPage2 = new List<Anime>
    {
        new () { Id = 2, Name = "Dragon Ball", Director = "Akira Toriyama", Summary = "A martial arts journey" }
    };

        // Simulando diferentes resultados para páginas 1 e 2
        _animeRepositoryMock.Setup(r => r.GetAllAsync(null, null, null, 1, 1)).ReturnsAsync(animesPage1);
        _animeRepositoryMock.Setup(r => r.GetAllAsync(null, null, null, 2, 1)).ReturnsAsync(animesPage2);

        // Act
        var resultPage1 = await _animeService.GetAllAsync(null, null, null, 1, 1);
        var resultPage2 = await _animeService.GetAllAsync(null, null, null, 2, 1);

        // Assert
        Assert.Single(resultPage1);
        Assert.Contains(resultPage1, a => a.Name == "Naruto");

        Assert.Single(resultPage2);
        Assert.Contains(resultPage2, a => a.Name == "Dragon Ball");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoAnimesMatchFilters()
    {
        // Arrange
        _animeRepositoryMock.Setup(r => r.GetAllAsync("NonExistentDirector", null, null, 1, 10)).ReturnsAsync(new List<Anime>());

        // Act
        var result = await _animeService.GetAllAsync("NonExistentDirector", null, null, 1, 10);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldSetIsDeletedTrue_WhenAnimeExists()
    {
        // Arrange
        var anime = new Anime { Id = 1, Name = "Naruto", Director = "Kishimoto", IsDeleted = false };
        _animeRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(anime);
        _animeRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Anime>())).Returns(Task.CompletedTask);
        // Act
        await _animeService.DeleteByIdAsync(1);

        // Assert
        Assert.True(anime.IsDeleted);
        _animeRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Anime>()), Times.Once);
    }
}