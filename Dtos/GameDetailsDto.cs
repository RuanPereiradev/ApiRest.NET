namespace WebApplication1.Dtos;

public record class GameDetailsDto(
    int Id,
    String Name,
    int GenreId,
    decimal Price,
    DateOnly ReleaseDate);

