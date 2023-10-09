using Recipizer.Cli.Models;
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

    internal async Task<string> Recipes(RecipesOptions options)
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
            if (options.WithIngredients)
            {
                var recipes = await repository.GetRecipesWithIngredients(options.Match);

                if (options.ByFewest)
                {
                    recipes = recipes
                        .OrderBy(x => x.AllIngredients.Count)
                        .ThenBy(x => x.MissingIngredients.Count);
                }
                else if (options.ByFewestMissing)
                {
                    recipes = recipes
                        .OrderBy(x => x.MissingIngredients.Count)
                        .ThenBy(x => x.AllIngredients.Count);
                }
                else if (options.ByMostInInventory)
                {
                    recipes = recipes
                        .OrderByDescending(x => x.InventoryIngredients.Count)
                        .ThenBy(x => x.AllIngredients.Count);
                }

                if (options.Take != null)
                {
                    recipes = recipes.Take(options.Take.Value);
                }

                return serializer.SerializeRecipesWithIngredients(recipes, IngredientList.All);
            }

            if (options.WithMissingIngredients)
            {
                var recipes = await repository.GetRecipesWithIngredients(options.Match);

                if (options.ByFewest)
                {
                    recipes = recipes
                        .OrderBy(x => x.AllIngredients.Count)
                        .ThenBy(x => x.MissingIngredients.Count);
                }
                else if (options.ByFewestMissing)
                {
                    recipes = recipes
                        .OrderBy(x => x.MissingIngredients.Count)
                        .ThenBy(x => x.AllIngredients.Count);
                }
                else if (options.ByMostInInventory)
                {
                    recipes = recipes
                        .OrderByDescending(x => x.InventoryIngredients.Count)
                        .ThenBy(x => x.AllIngredients.Count);
                }

                if (options.Take != null)
                {
                    recipes = recipes.Take(options.Take.Value);
                }

                return serializer.SerializeRecipesWithIngredients(recipes, IngredientList.Missing);
            }

            if (options.WithInventoryIngredients)
            {
                var recipes = await repository.GetRecipesWithIngredients(options.Match);

                if (options.ByFewest)
                {
                    recipes = recipes
                        .OrderBy(x => x.AllIngredients.Count)
                        .ThenBy(x => x.MissingIngredients.Count);
                }
                else if (options.ByFewestMissing)
                {
                    recipes = recipes
                        .OrderBy(x => x.MissingIngredients.Count)
                        .ThenBy(x => x.AllIngredients.Count);
                }
                else if (options.ByMostInInventory)
                {
                    recipes = recipes
                        .OrderByDescending(x => x.InventoryIngredients.Count)
                        .ThenBy(x => x.AllIngredients.Count);
                }

                if (options.Take != null)
                {
                    recipes = recipes.Take(options.Take.Value);
                }

                return serializer.SerializeRecipesWithIngredients(
                    recipes,
                    IngredientList.Inventory
                );
            }

            var recipesFallBack = await repository.GetRecipes(options.Match);

            if (options.Take != null)
            {
                recipesFallBack = recipesFallBack.Take(options.Take.Value);
            }

            return serializer.SerializeRecipes(recipesFallBack);
        }

        if (options.Add)
        {
            var name = options.Name;
            var details = options.Details;

            var id = await repository.CreateRecipe(name, details);

            var recipes = await repository.GetRecipes();
            var newRecipe = (from r in recipes where r.RecipeId == id select r).Single();
            newRecipe.Name += "*";

            return serializer.SerializeRecipes(recipes);
        }

        if (options.Remove)
        {
            var id = options.Id;

            await repository.DeleteRecipe(id);

            return serializer.SerializeRecipes(await repository.GetRecipes());
        }

        return serializer.SerializeRecipes(await repository.GetRecipes(options.Match));
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
            return serializer.SerializeIngredientsWithAdded(
                await repository.GetIngredients(options.Match)
            );
        }

        if (options.Missing)
        {
            return serializer.SerializeIngredients(await repository.GetMissing(options.Match));
        }

        if (options.Inventory)
        {
            return serializer.SerializeIngredientsWithAdded(
                await repository.GetInventory(options.Match)
            );
        }

        if (options.Remove)
        {
            foreach (var ingredientId in options.FromInventory)
            {
                await repository.DeleteInventoryIngredient(ingredientId);
            }

            return serializer.SerializeIngredientsWithAdded(await repository.GetInventory());
        }

        if (options.Add)
        {
            var ingredientIds = options.ToInventory;

            foreach (var ingredientId in ingredientIds)
            {
                await repository.CreateInventoryIngredient(ingredientId);
            }

            var inventory = await repository.GetInventory();
            var newIngredients = (
                from x in inventory
                join y in ingredientIds on x.IngredientId equals y
                select x
            );
            foreach (var newIngredient in newIngredients)
            {
                newIngredient.Name += "*";
            }

            return serializer.SerializeIngredientsWithAdded(inventory);
        }

        return serializer.SerializeIngredientsWithAdded(
            await repository.GetIngredients(options.Match)
        );
    }
}
