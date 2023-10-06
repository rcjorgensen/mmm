using System.Data.SQLite;
using System.Text.Json;
using System.Text.Json.Nodes;
using Dapper;
using Recipizer.Cli.Models;

DefaultTypeMap.MatchNamesWithUnderscores = true;

var command = args[0];

var dbFile = "recipizer.db";

if (command == "init")
{
    var schemaFile = "../sql/tables.sql"; // args[1];
    var dataFile = "../../data/recipes.json"; // args[2];
    var force = args.Contains("--force") || args.Contains("-f");

    if (File.Exists(dbFile))
    {
        if (force)
        {
            File.Delete(dbFile);
        }
        else
        {
            Console.WriteLine("ERROR: Database file already exists and would be overwritten");
            return;
        }
    }

    File.Create(dbFile);

    using var connection = new SQLiteConnection($"Data Source={dbFile}");

    var schema = File.ReadAllText(schemaFile);

    await connection.ExecuteAsync(schema);

    var recipes = JsonNode
        .Parse(await File.ReadAllTextAsync(dataFile))
        ?["recipes"].Deserialize<List<RecipeJson>>(
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );

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

        Console.WriteLine("Id|Name|Labels");
        Console.WriteLine("--------------");
        foreach (var ingredient in ingredients)
        {
            var labels = await connection.QueryAsync<string>(
                "SELECT l.label FROM label l JOIN ingredient_label il ON l.label_id = il.label_id WHERE il.ingredient_id = @ingredientId",
                new { ingredientId = ingredient.IngredientId }
            );
            Console.WriteLine(
                $"{ingredient.IngredientId}|{ingredient.Name}|{string.Join(',', labels)}"
            );
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
else if (command == "add")
{
    var subCommand = args[1];

    if (!File.Exists(dbFile))
    {
        Console.WriteLine("ERROR: Database file does not exist, run `init`");
        return;
    }

    using var connection = new SQLiteConnection($"Data Source={dbFile}");

    if (subCommand == "label")
    {
        var label = args[3];

        var labelIdQuery = "SELECT label_id FROM label WHERE label = @label";

        var labelId = await connection.QuerySingleOrDefaultAsync<int?>(labelIdQuery, new { label });

        if (labelId == null)
        {
            await connection.ExecuteAsync(
                "INSERT INTO label (label) VALUES (@label)",
                new { label }
            );

            labelId = await connection.QuerySingleAsync<int>(labelIdQuery, new { label });
        }

        // Create relationships


        var ingredientIds = args[2].Split(',').Select(long.Parse);
        foreach (var ingredientId in ingredientIds)
        {
            await connection.ExecuteAsync(
                "INSERT INTO ingredient_label (ingredient_id, label_id) VALUES (@ingredientId, @labelId)",
                new { ingredientId, labelId }
            );
        }
    }
}
else if (command == "optimize")
{
    // Recipes with fewest ingredients -- optimizes for ease

    var subCommand = args[1];

    if (!File.Exists(dbFile))
    {
        Console.WriteLine("ERROR: Database file does not exist, run `init`");
        return;
    }

    using var connection = new SQLiteConnection($"Data Source={dbFile}");

    if (subCommand == "fewest-ingredients")
    {
        var rows = args.Length >= 3 ? int.Parse(args[2]) : 0;

        IEnumerable<Recipe> recipes = (
            from r in await connection.QueryAsync<Recipe, Ingredient, Recipe>(
                """
                SELECT r.recipe_id, r.name, r.details, i.ingredient_id, i.name FROM recipe r
                JOIN recipe_ingredient ri ON r.recipe_id = ri.recipe_id
                JOIN ingredient i ON ri.ingredient_id = i.ingredient_id
                """,
                (recipe, ingredient) =>
                {
                    recipe.Ingredients.Add(ingredient);
                    return recipe;
                },
                splitOn: "ingredient_id"
            )
            group r by r.Name into g
            select g.First() with
            {
                Ingredients = (from r in g from i in r.Ingredients select i).ToList()
            }
        ).OrderBy(r => r.Ingredients.Count());

        if (rows > 0)
        {
            recipes = recipes.Take(rows);
        }

        foreach (var recipe in recipes)
        {
            Console.WriteLine("[Recipe]");
            Console.WriteLine($"Id: {recipe.RecipeId}");
            Console.WriteLine($"Name: {recipe.Name}");
            Console.WriteLine($"Details: {recipe.Details}");
            Console.WriteLine();
            Console.WriteLine("[Ingredients]");
            Console.WriteLine($"Count: {recipe.Ingredients.Count}");
            Console.WriteLine("Id|Name");
            Console.WriteLine("-------");
            foreach (var ingredient in recipe.Ingredients)
            {
                Console.WriteLine($"{ingredient.IngredientId}|{ingredient.Name}");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }

    // Recipes with fewest missing ingredients -- optimizes for economy

    // Recipes with most ingredients in stock -- optimizes for reducing food waste

    // Most used ingredients -- optimizes for diversity

    // Recipes with widely used ingredients -- optimizes for reuse
}
else
{
    Console.WriteLine($"ERROR: Unknown command {command}");
}
