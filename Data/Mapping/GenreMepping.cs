using System;
using WebApplication1.Dtos;
using WebApplication1.Entities;

namespace WebApplication1.Data.Mapping;

public static class GenreMepping
{
    public static GenreDto ToDto(this Genre genre)
    {
        return new GenreDto(genre.Id, genre.Name);
    }
}
