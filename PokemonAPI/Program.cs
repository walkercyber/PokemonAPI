namespace PokemonAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var pokemons = new List<Pokemon>
            {
                new Pokemon {Id = 1, Name = "Bulbasaur", Type = "Grass"},
                new Pokemon {Id = 2, Name = "Ivysaur", Type = "Grass" },
                new Pokemon {Id = 3, Name = "Venosaur", Type = "Grass"},
                new Pokemon {Id = 4, Name = "Charmander", Type = "Fire"}
            };

            // Create
            app.MapPost("/pokemon", (Pokemon pokemon) =>
            {
                pokemons.Add(pokemon);
                return Results.Ok(pokemon);
            });

            // Read by ID
            app.MapGet("/pokemon/{id}", (int id) =>
            {
                var pokemon = pokemons.Find(p => p.Id == id);

                if (pokemon == null)
                {
                    return Results.NotFound("Sorry, this pokemon does not exist");
                }
                return Results.Ok(pokemon);
            });

            // Read all
            app.MapGet("/pokemons", () =>
            {
                return Results.Ok(pokemons);
            });

            //Update
            app.MapPut("/pokemon/{id}", (Pokemon updatedPokemon, int id) =>
            {
                var pokemon = pokemons.Find(p => p.Id == id);
                if (pokemon == null)
                {
                    return Results.NotFound("This pokemon does not exist");
                }

                pokemons[id - 1] = updatedPokemon;


                return Results.Ok(pokemon);
            });


            //Delete

            app.MapDelete("/pokemon/{id}", (int id) =>
            {
                var pokemon = pokemons.Find(p => p.Id == id);

                if (pokemon == null)
                {
                    return Results.NotFound("not found");
                }

                pokemons.Remove(pokemon);

                return Results.Ok("Works");
            });

            app.Run();
        }
    }
}