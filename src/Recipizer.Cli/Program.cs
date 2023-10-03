using System.Data.SQLite;
using System.Text.Json;
using Dapper;
using Recipizer.Cli.Models;

DefaultTypeMap.MatchNamesWithUnderscores = true;

if (args.Length != 4)
{
    Console.WriteLine("ERROR: Expected 3 arguments");
    return;
}

var command = args[0];

if (command != "init")
{
    Console.WriteLine($"ERROR: Unknown command {command}");
    return;
}

var dbFile = args[1];
var schemaFile = args[2];
var dataFile = args[3];

if (File.Exists(dbFile))
{
    Console.WriteLine("ERROR: Database file already exists and would be overwritten");
    return;
}

File.Create(dbFile);

using var connection = new SQLiteConnection($"Data Source={dbFile}");

var schema = File.ReadAllText(schemaFile);

await connection.ExecuteAsync(schema);

var recipes = JsonSerializer
    .Deserialize<RecipesJson>(
        await File.ReadAllTextAsync(dataFile),
        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
    )
    ?.Recipes;

if (recipes == null)
{
    Console.WriteLine("ERROR: Could not read recipes");
    return;
}

foreach (var recipe in recipes)
{
    // Create recipe


    await connection.ExecuteAsync(
        "INSERT INTO recipe (name, recipe_source_id, details) VALUES (@name, 1, @details)",
        new { name = recipe.Name, details = recipe.Details }
    );

    var recipeId = await connection.QuerySingleAsync<int>(
        "SELECT recipe_id FROM recipe WHERE name = @name",
        new { name = recipe.Name }
    );

    foreach (var ingredient in recipe.Ingredients)
    {
        // Get or create ingredient


        var ingredientIdQuery = "SELECT ingredient_id FROM ingredient WHERE name = @ingredient";

        var ingredientId = await connection.QuerySingleOrDefaultAsync<int?>(
            ingredientIdQuery,
            new { ingredient }
        );

        if (ingredientId == null)
        {
            await connection.ExecuteAsync(
                "INSERT INTO ingredient (name) VALUES (@ingredient)",
                new { ingredient }
            );

            ingredientId = await connection.QuerySingleAsync<int>(
                ingredientIdQuery,
                new { ingredient }
            );
        }

        // Create relationship


        await connection.ExecuteAsync(
            "INSERT INTO recipe_ingredient (recipe_id, ingredient_id) VALUES (@recipeId, @ingredientId)",
            new { recipeId, ingredientId }
        );
    }
}
