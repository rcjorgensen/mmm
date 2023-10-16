using Recipizer.Cli.Models;
using Recipizer.Cli.Options;

namespace Recipizer.Cli;

internal interface IApplication
{
    Task<string> Init(InitOptions options);
    Task<string> Recipes(RecipesOptions options);
    Task<string> Ingredients(IngredientsOptions options);
}

internal sealed class Application : IApplication
{
    private readonly Configuration _configuration;
    private readonly IRepository _repository;
    private readonly IFileSystem _fileSystem;
    private readonly IDeserializer _deserializer;
    private readonly ISerializer _serializer;

    public Application(
        Configuration configuration,
        IRepository repository,
        IFileSystem fileSystem,
        IDeserializer deserializer,
        ISerializer serializer
    )
    {
        _configuration = configuration;
        _repository = repository;
        _fileSystem = fileSystem;
        _deserializer = deserializer;
        _serializer = serializer;
    }

    public async Task<string> Init(InitOptions options)
    {
        var databaseFilePath = _configuration.DatabaseFilePath;
        if (databaseFilePath == null)
        {
            return "ERROR: Could not get database file from configuration";
        }

        var dataFilePath = _configuration.DataFilePath;
        if (dataFilePath == null)
        {
            return "ERROR: Could not get data file path from configuration";
        }

        if (options.Force && _fileSystem.Exists(databaseFilePath))
        {
            _fileSystem.Delete(databaseFilePath);
            _fileSystem.Create(databaseFilePath);
        }

        await _repository.InitializeSchema();

        Console.WriteLine($"Data file path: {Path.GetFullPath(dataFilePath)}");
        Console.WriteLine($"Current directory: {Directory.GetCurrentDirectory()}");

        var data = await _fileSystem.ReadAllText(dataFilePath);

        var labels = _deserializer.DeserializeLabels(data);

        if (labels == null)
        {
            return "ERROR: Could not read labels";
        }

        foreach (var label in labels)
        {
            await _repository.CreateLabel(label);
        }

        var recipes = _deserializer.DeserializeRecipes(data);

        if (recipes == null)
        {
            return "ERROR: Could not read recipes";
        }

        var recipeSource = _deserializer.DeserializeRecipeSource(data);

        if (recipeSource == null)
        {
            return "ERROR: Could not read recipe source";
        }

        var recipeSourceId = await _repository.CreateRecipeSource(recipeSource);

        foreach (var recipe in recipes)
        {
            var recipeId = await _repository.CreateRecipe(
                recipe.Name,
                recipe.Details,
                recipeSourceId
            );

            foreach (var ingredient in recipe.Ingredients)
            {
                var ingredientId = await _repository.GetOrCreateIngredient(ingredient);
                await _repository.AddIngredientToRecipe(recipeId, ingredientId);
            }
        }

        return "SUCCESS: Database initialized";
    }

    public async Task<string> Recipes(RecipesOptions options)
    {
        var databaseFilePath = _configuration.DatabaseFilePath;
        if (databaseFilePath == null)
        {
            return "ERROR: Could not get database file from configuration";
        }

        if (!_fileSystem.Exists(databaseFilePath))
        {
            return "ERROR: Database file does not exist, run `init`";
        }

        if (options.List)
        {
            if (options.WithIngredients)
            {
                var recipes = await _repository.GetRecipesWithIngredients(options.Match);

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

                return options.Markdown
                    ? MarkdownSerializer.SerializeRecipesWithIngredients(
                        recipes,
                        IngredientList.All
                    )
                    : _serializer.SerializeRecipesWithIngredients(recipes, IngredientList.All);
            }

            if (options.WithMissingIngredients)
            {
                var recipes = await _repository.GetRecipesWithIngredients(options.Match);

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

                return options.Markdown
                    ? MarkdownSerializer.SerializeRecipesWithIngredients(
                        recipes,
                        IngredientList.Missing
                    )
                    : _serializer.SerializeRecipesWithIngredients(recipes, IngredientList.Missing);
            }

            if (options.WithInventoryIngredients)
            {
                var recipes = await _repository.GetRecipesWithIngredients(options.Match);

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

                return options.Markdown
                    ? MarkdownSerializer.SerializeRecipesWithIngredients(
                        recipes,
                        IngredientList.Inventory
                    )
                    : _serializer.SerializeRecipesWithIngredients(
                        recipes,
                        IngredientList.Inventory
                    );
            }

            var recipesFallBack = await _repository.GetRecipes(options.Match);

            if (options.Take != null)
            {
                recipesFallBack = recipesFallBack.Take(options.Take.Value);
            }

            return options.Markdown
                ? MarkdownSerializer.SerializeRecipes(recipesFallBack)
                : _serializer.SerializeRecipes(recipesFallBack);
        }

        if (options.Add)
        {
            var name = options.Name;
            var details = options.Details;

            var id = await _repository.CreateRecipe(name, details);

            var recipes = await _repository.GetRecipes();
            var newRecipe = (from r in recipes where r.RecipeId == id select r).Single();
            newRecipe.Name += "*";

            return _serializer.SerializeRecipes(recipes);
        }

        if (options.Remove)
        {
            var id = options.Id;

            await _repository.DeleteRecipe(id);

            return _serializer.SerializeRecipes(await _repository.GetRecipes());
        }

        return _serializer.SerializeRecipes(await _repository.GetRecipes(options.Match));
    }

    public async Task<string> Ingredients(IngredientsOptions options)
    {
        var databaseFilePath = _configuration.DatabaseFilePath;
        if (databaseFilePath == null)
        {
            return "ERROR: Could not get database file from configuration";
        }

        if (!_fileSystem.Exists(databaseFilePath))
        {
            return "ERROR: Database file does not exist, run `init`";
        }

        if (options.List)
        {
            return _serializer.SerializeIngredientsWithAdded(
                await _repository.GetIngredients(options.Match)
            );
        }

        if (options.Missing)
        {
            return _serializer.SerializeIngredients(await _repository.GetMissing(options.Match));
        }

        if (options.Inventory)
        {
            return _serializer.SerializeIngredientsWithAdded(
                await _repository.GetInventory(options.Match)
            );
        }

        if (options.Remove)
        {
            foreach (var ingredientId in options.FromInventory)
            {
                await _repository.DeleteInventoryIngredient(ingredientId);
            }

            return _serializer.SerializeIngredientsWithAdded(await _repository.GetInventory());
        }

        if (options.Add)
        {
            var ingredientIds = options.ToInventory;

            foreach (var ingredientId in ingredientIds)
            {
                await _repository.CreateInventoryIngredient(ingredientId);
            }

            var inventory = await _repository.GetInventory();
            var newIngredients = (
                from x in inventory
                join y in ingredientIds on x.IngredientId equals y
                select x
            );
            foreach (var newIngredient in newIngredients)
            {
                newIngredient.Name += "*";
            }

            return _serializer.SerializeIngredientsWithAdded(inventory);
        }

        return _serializer.SerializeIngredientsWithAdded(
            await _repository.GetIngredients(options.Match)
        );
    }
}
