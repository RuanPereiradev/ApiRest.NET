using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Entities;

namespace WebApplication1.Endpoints;

public static class GamesEndpoints
{

    const String GetGameEndpointName = "GetGame";

    
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
                        .WithParameterValidation();

        group.MapGet("/", async (GameStoreContext dbContext) =>
           await dbContext.Games
                    .Include(game => game.Genre)
                    .Select(game => game.ToGameSummaryDto())
                    .AsNoTracking()
                    .ToListAsync());

        group.MapGet("/{id}", async (int id, GameStoreContext dbConext) =>
        {
            Game? game = await dbConext.Games.FindAsync(id);

            return game is null ?
                Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        })

        .WithName(GetGameEndpointName);

        group.MapPost("/", async  (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();


            return Results.CreatedAtRoute(
                GetGameEndpointName,
                 new { id = game.Id },
                  game.ToGameDetailsDto());

        });
        group.MapPut("/{id}", async (int id, UpdateGameDto updateGame, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingGame)
                            .CurrentValues
                            .SetValues(updateGame.ToEntity(id));

          await  dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        //DELETE GAMES/1
        group.MapDelete("/{id}",async  (int id,GameStoreContext dbContext ) =>
        {
           await dbContext.Games
                    .Where(game => game.Id == id)
                    .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        return group;
    }

}
