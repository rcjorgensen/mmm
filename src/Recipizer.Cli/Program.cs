using System.Data.SQLite;
using System.Text.Json;
using Dapper;
using Recipizer.Cli.Models;

DefaultTypeMap.MatchNamesWithUnderscores = true;

var command = args[0];

var dbFile = "recipizer.db";

if (command == "init")
{
    var schemaFile = args[1];
    var dataFile = args[2];

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
}
else if (command == "list")
{
    var subCommand = args[1];

    // Get number of recipes and ingredients


    if (!File.Exists(dbFile))
    {
        Console.WriteLine("ERROR: Database file does not exist, run `init`");
        return;
    }

    using var connection = new SQLiteConnection($"Data Source={dbFile}");

    if (subCommand == "ingredients")
    {
        IEnumerable<Ingredient> ingredients;
        if (args.Length >= 3)
        {
            var nameFilter = args[2].ToLower();
            ingredients = await connection.QueryAsync<Ingredient>(
                "SELECT * FROM ingredient WHERE LOWER(name) LIKE @nameFilter",
                new { nameFilter }
            );
        }
        else
        {
            ingredients = await connection.QueryAsync<Ingredient>("SELECT * FROM ingredient");
        }

        Console.WriteLine("Id|Name");
        Console.WriteLine("-------");
        foreach (var ingredient in ingredients)
        {
            Console.WriteLine($"{ingredient.IngredientId}|{ingredient.Name}");
        }
    }
    else if (subCommand == "inventory")
    {
        var ingredients = await connection.QueryAsync<InventoryIngredient>(
            "SELECT i.ingredient_id, i.name, ii.added FROM inventory_ingredient ii JOIN ingredient i ON ii.ingredient_id = i.ingredient_id"
        );

        Console.WriteLine("Id|Name|Added");
        Console.WriteLine("-------------");
        foreach (var ingredient in ingredients)
        {
            Console.WriteLine($"{ingredient.IngredientId}|{ingredient.Name}|{ingredient.Added}");
        }
    }
    else if (subCommand == "missing")
    {
        var ingredients = await connection.QueryAsync<Ingredient>(
            "SELECT * FROM ingredient WHERE ingredient_id NOT IN (SELECT ingredient_id FROM inventory_ingredient)"
        );

        Console.WriteLine("Id|Name");
        Console.WriteLine("-------");
        foreach (var ingredient in ingredients)
        {
            Console.WriteLine($"{ingredient.IngredientId}|{ingredient.Name}");
        }
    }
}
else if (command == "inventory")
{
    var subCommand = args[1];
    var ingredientIds = args[2].Split(',').Select(long.Parse);

    if (!File.Exists(dbFile))
    {
        Console.WriteLine("ERROR: Database file does not exist, run `init`");
        return;
    }

    using var connection = new SQLiteConnection($"Data Source={dbFile}");

    if (subCommand == "add")
    {
        foreach (var ingredientId in ingredientIds)
        {
            await connection.ExecuteAsync(
                "INSERT INTO inventory_ingredient (ingredient_id) VALUES (@ingredientId)",
                new { ingredientId }
            );
        }
    }
    else if (subCommand == "remove")
    {
        foreach (var ingredientId in ingredientIds)
        {
            await connection.ExecuteAsync(
                "DELETE FROM inventory_ingredient WHERE ingredient_id = @ingredientId",
                new { ingredientId }
            );
        }
    }
}
else
{
    Console.WriteLine($"ERROR: Unknown command {command}");
}
