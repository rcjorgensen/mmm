using r7r.Options;

using Recipizer.Core;
using Recipizer.Core.Models;

namespace r7r;

internal sealed class Application : IApplication
{
    private readonly string _databaseFilePath;
    private readonly IRepository _repository;
    private readonly IFileSystem _fileSystem;
    private readonly IDeserializer _deserializer;
    private readonly ISerializer _serializer;

    public Application(
        string databaseFilePath,
        IRepository repository,
        IFileSystem fileSystem,
        IDeserializer deserializer,
        ISerializer serializer
    )
    {
        _databaseFilePath = databaseFilePath;
        _repository = repository;
        _fileSystem = fileSystem;
        _deserializer = deserializer;
        _serializer = serializer;
    }

    public async Task<string> AddIngredient(AddIngredientOptions options)
    {
        var validationError = ValidateDatabaseExists();
        if (validationError != null)
        {
            return validationError;
        }

        var id = await _repository.CreateIngredient(options.Name);

        var ingredients = await _repository.GetIngredients();
        var newIngredient = (from r in ingredients where r.IngredientId == id select r).Single();
        newIngredient.Name += "*";

        return _serializer.SerializeIngredients(ingredients);
    }

    public async Task<string> AddRecipe(AddRecipeOptions options)
    {
        var validationError = ValidateDatabaseExists();
        if (validationError != null)
        {
            return validationError;
        }

        var id = await _repository.CreateRecipe(options.Name, options.Details);

        var recipes = await _repository.GetRecipes();
        var newRecipe = (from r in recipes where r.RecipeId == id select r).Single();
        newRecipe.Name += "*";

        return _serializer.SerializeRecipes(recipes);
    }

    public async Task<string> AddToInventory(AddToInventoryOptions options)
    {
        var validationError = ValidateDatabaseExists();
        if (validationError != null)
        {
            return validationError;
        }

        var ingredientIds = options.IngredientIds;

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

        return _serializer.SerializeIngredients(inventory);
    }

    public async Task<string> Import(ImportOptions options)
    {
        var validationError = ValidateDatabaseExists();
        if (validationError != null)
        {
            return validationError;
        }

        var data = await _fileSystem.ReadAllText(options.Path);

        var labels = _deserializer.DeserializeLabels(data);

        if (labels == null)
        {
            return "ERROR: Could not read labels";
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

        var inventory = _deserializer.DeserializeInventory(data);

        if (inventory == null)
        {
            return "ERROR: Could not read inventory";
        }

        foreach (var label in labels)
        {
            await _repository.CreateLabel(label);
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
                var ingredientId = await _repository.CreateIngredient(ingredient);
                await _repository.AddIngredientToRecipe(recipeId, ingredientId);
            }
        }

        foreach (var ingredient in inventory)
        {
            var ingredientId = await _repository.CreateIngredient(ingredient);
            await _repository.CreateInventoryIngredient(ingredientId);
        }

        return "SUCCESS: Data imported";
    }

    public async Task<string> Initialize(InitializeOptions options)
    {
        if (_databaseFilePath == null)
        {
            return "ERROR: Could not get database file from configuration";
        }

        if (options.Force && _fileSystem.Exists(_databaseFilePath))
        {
            _fileSystem.Delete(_databaseFilePath);
            _fileSystem.Create(_databaseFilePath);
        }

        await _repository.InitializeSchema();

        return "SUCCESS: Database initialized";
    }

    public async Task<string> ShowIngredients(ShowIngredientsOptions options)
    {
        var validationError = ValidateDatabaseExists();

        if (validationError != null)
        {
            return validationError;
        }

        IEnumerable<IngredientListModel> ingredients = (
            await _repository.GetIngredients(options.Name, options.Label)
        ).OrderByDescending(x => x.Recipes.Count);

        if (options.Take != null)
        {
            ingredients = ingredients.Take(options.Take.Value);
        }

        return _serializer.SerializeIngredients(ingredients);
    }

    public async Task<string> ShowInventory(ShowInventoryOptions options)
    {
        var validationError = ValidateDatabaseExists();

        if (validationError != null)
        {
            return validationError;
        }

        IEnumerable<IngredientListModel> ingredients = (
            await _repository.GetInventory(options.Name, options.Label)
        ).OrderByDescending(x => x.Recipes.Count);

        if (options.Take != null)
        {
            ingredients = ingredients.Take(options.Take.Value);
        }

        return _serializer.SerializeIngredients(ingredients);
    }

    public async Task<string> ShowMissingIngredients(ShowMissingIngredientsOptions options)
    {
        var validationError = ValidateDatabaseExists();

        if (validationError != null)
        {
            return validationError;
        }

        IEnumerable<IngredientListModel> ingredients = (
            await _repository.GetMissing(options.Name, options.Label)
        ).OrderByDescending(x => x.Recipes.Count);

        if (options.Take != null)
        {
            ingredients = ingredients.Take(options.Take.Value);
        }

        return _serializer.SerializeIngredients(ingredients);
    }

    public async Task<string> ShowRecipes(ShowRecipesOptions options)
    {
        var validationError = ValidateDatabaseExists();
        if (validationError != null)
        {
            return validationError;
        }

        var recipes = await _repository.GetRecipesWithIngredients(options.Name);

        var label = options.OrderByLabel;

        if (options.OrderByTotal)
        {
            recipes = recipes
                .OrderBy(x => x.AllIngredients.Count)
                .ThenBy(x => x.MissingIngredients.Count)
                .ThenBy(x => x.InventoryIngredients.Count);
        }
        else if (options.OrderByMissing)
        {
            recipes = recipes
                .OrderBy(x => x.MissingIngredients.Count)
                .ThenBy(x => x.InventoryIngredients.Count)
                .ThenBy(x => x.AllIngredients.Count);
        }
        else if (options.OrderByInInventory)
        {
            recipes = recipes
                .OrderByDescending(x => x.InventoryIngredients.Count)
                .ThenBy(x => x.MissingIngredients.Count)
                .ThenBy(x => x.AllIngredients.Count);
        }
        else if (!string.IsNullOrWhiteSpace(label))
        {
            recipes = recipes
                .OrderByDescending(x => x.GetIngredientsWithLabel(label).Count)
                .ThenBy(x => x.MissingIngredients.Count)
                .ThenBy(x => x.InventoryIngredients.Count)
                .ThenBy(x => x.AllIngredients.Count);
        }

        if (options.Take != null)
        {
            recipes = recipes.Take(options.Take.Value);
        }

        return options.Markdown
            ? MarkdownSerializer.SerializeRecipesWithIngredients(recipes)
            : _serializer.SerializeRecipesWithIngredients(recipes);
    }

    public async Task<string> RemoveFromInventory(RemoveFromInventoryOptions options)
    {
        var validationError = ValidateDatabaseExists();
        if (validationError != null)
        {
            return validationError;
        }

        foreach (var ingredientId in options.IngredientIds)
        {
            await _repository.DeleteInventoryIngredient(ingredientId);
        }

        return _serializer.SerializeIngredients(await _repository.GetInventory());
    }

    public async Task<string> RemoveRecipe(RemoveRecipeOptions options)
    {
        var validationError = ValidateDatabaseExists();
        if (validationError != null)
        {
            return validationError;
        }

        var id = options.RecipeId;

        await _repository.DeleteRecipe(id);

        return _serializer.SerializeRecipes(await _repository.GetRecipes());
    }

    private string? ValidateDatabaseExists()
    {
        var databaseFilePath = _databaseFilePath;

        return databaseFilePath == null
            ? "ERROR: Could not get database file from configuration"
            : !_fileSystem.Exists(databaseFilePath)
                ? "ERROR: Database file does not exist, run `initialize`"
                : null;
    }

    public async Task<string> AddLabel(AddLabelOptions options)
    {
        var validationError = ValidateDatabaseExists();
        if (validationError != null)
        {
            return validationError;
        }

        var labelId = await _repository.CreateLabel(options.Label);

        foreach (var ingredientId in options.IngredientIds)
        {
            await _repository.AddLabelToIngredient(ingredientId, labelId);
        }

        return _serializer.SerializeIngredients(
            await _repository.GetIngredients(null, options.Label)
        );
    }

    public async Task<string> RemoveLabel(RemoveLabelOptions options)
    {
        var validationError = ValidateDatabaseExists();
        if (validationError != null)
        {
            return validationError;
        }

        foreach (var ingredientId in options.IngredientIds)
        {
            await _repository.RemoveLabelFromIngredient(ingredientId, options.Label);
        }

        var ingredientIds = await _repository.GetIngredientIdsForLabel(options.Label);

        if (!ingredientIds.Any())
        {
            await _repository.DeleteLabel(options.Label);
        }

        return _serializer.SerializeIngredients(await _repository.GetIngredients());
    }
}
