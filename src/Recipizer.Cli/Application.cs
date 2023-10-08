using Recipizer.Cli.Options;

namespace Recipizer.Cli;

internal sealed class Application
{
    private readonly Configuration configuration;
    private readonly Repository repository;
    private readonly FileSystem fileSystem;
    private readonly Deserializer deserializer;
    private readonly Serializer serializer;

    public Application(
        Configuration configuration,
        Repository repository,
        FileSystem fileSystem,
        Deserializer deserializer,
        Serializer serializer
    )
    {
        this.configuration = configuration;
        this.repository = repository;
        this.fileSystem = fileSystem;
        this.deserializer = deserializer;
        this.serializer = serializer;
    }

    public async Task<string> Init(InitOptions options)
    {
        var schemaFilePath = configuration.SchemaFilePath;
        if (schemaFilePath == null)
        {
            return "ERROR: Could not get schema file from configuration";
        }

        var databaseFilePath = configuration.DatabaseFilePath;
        if (databaseFilePath == null)
        {
            return "ERROR: Could not get database file from configuration";
        }

        var dataFilePath = configuration.DataFilePath;
        if (dataFilePath == null)
        {
            return "ERROR: Could not get data file path from configuration";
        }

        if (options.Force && fileSystem.Exists(databaseFilePath))
        {
            fileSystem.Delete(databaseFilePath);
            fileSystem.Create(databaseFilePath);
        }

        var schema = await fileSystem.ReadAllTextAsync(schemaFilePath);

        await repository.ExecuteRaw(schema);

        var data = await fileSystem.ReadAllTextAsync(dataFilePath);

        var recipes = deserializer.DeserializeRecipes(data);

        if (recipes == null)
        {
            return "ERROR: Could not read recipes";
        }

        var recipeSource = deserializer.DeserializeRecipeSource(data);

        if (recipeSource == null)
        {
            return "ERROR: Could not read recipe source";
        }

        var recipeSourceId = await repository.CreateRecipeSource(recipeSource);

        foreach (var recipe in recipes)
        {
            var recipeId = await repository.CreateRecipe(
                recipe.Name,
                recipe.Details,
                recipeSourceId
            );

            foreach (var ingredient in recipe.Ingredients)
            {
                var ingredientId = await repository.GetOrCreateIngredient(ingredient);
                await repository.AddIngredientToRecipe(recipeId, ingredientId);
            }
        }

        return "SUCCESS: Database initialized";
    }

    internal async Task<string> Recipes(RecipesOptions opts)
    {
        var databaseFilePath = configuration.DatabaseFilePath;
        if (databaseFilePath == null)
        {
            return "ERROR: Could not get database file from configuration";
        }

        if (!fileSystem.Exists(databaseFilePath))
        {
            return "ERROR: Database file does not exist, run `init`";
        }

        if (opts.List)
        {
            return serializer.SerializeRecipes(await repository.GetRecipes(opts.Match));
        }

        if (opts.Add)
        {
            var name = opts.Name;
            var details = opts.Details;

            var id = await repository.CreateRecipe(name, details);

            var recipes = await repository.GetRecipes();
            var newRecipe = (from r in recipes where r.RecipeId == id select r).Single();
            newRecipe.Name += "*";

            return serializer.SerializeRecipes(recipes);
        }

        if (opts.Remove)
        {
            var id = opts.Id;

            await repository.DeleteRecipe(id);

            return serializer.SerializeRecipes(await repository.GetRecipes());
        }

        return string.Empty;
    }

    internal async Task<string> Ingredients(IngredientsOptions options)
    {
        var databaseFilePath = configuration.DatabaseFilePath;
        if (databaseFilePath == null)
        {
            return "ERROR: Could not get database file from configuration";
        }

        if (!fileSystem.Exists(databaseFilePath))
        {
            return "ERROR: Database file does not exist, run `init`";
        }

        if (options.List)
        {
            return serializer.SerializeIngredients(await repository.GetIngredients(options.Match));
        }

        if (options.Missing)
        {
            return serializer.SerializeIngredients(await repository.GetMissing(options.Match));
        }

        return string.Empty;
    }

    internal async Task<string> Inventory(InventoryOptions options)
    {
        var databaseFilePath = configuration.DatabaseFilePath;
        if (databaseFilePath == null)
        {
            return "ERROR: Could not get database file from configuration";
        }

        if (!fileSystem.Exists(databaseFilePath))
        {
            return "ERROR: Database file does not exist, run `init`";
        }

        if (options.List)
        {
            return serializer.SerializeInventory(await repository.GetInventory(options.Match));
        }

        if (options.Add)
        {
            var ingredientId = options.IngredientId;

            await repository.CreateInventoryIngredient(ingredientId);

            var inventory = await repository.GetInventory();
            var ingredient = (
                from x in inventory
                where x.IngredientId == ingredientId
                select x
            ).Single();
            ingredient.Name += "*";

            return serializer.SerializeInventory(inventory);
        }

        if (options.Remove)
        {
            await repository.DeleteInventoryIngredient(options.IngredientId);

            return serializer.SerializeInventory(await repository.GetInventory());
        }

        return string.Empty;
    }
}
