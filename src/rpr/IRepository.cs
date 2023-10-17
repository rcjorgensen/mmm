using Recipizer.Cli.Models;

namespace Recipizer.Cli;

internal interface IRepository
{
    Task<int> InitializeSchema();
    Task<long> CreateRecipeSource(string name);
    Task<long> CreateLabel(string label);
    Task<long> CreateRecipe(string name, string? details = null, long? recipeSourceId = null);
    Task<long> CreateIngredient(string name);
    Task<long> AddIngredientToRecipe(long recipeId, long ingredientId);
    Task<IEnumerable<RecipeListModel>> GetRecipes(string? match = null);
    Task<IEnumerable<RecipeListModel>> GetRecipesWithIngredients(string? match = null);
    Task DeleteRecipe(long id);
    Task<IEnumerable<IngredientListModel>> GetIngredients(
        string? match = null,
        string? label = null
    );
    Task<IEnumerable<IngredientListModel>> GetInventory(string? match = null, string? label = null);
    Task<IEnumerable<IngredientListModel>> GetMissing(string? match = null, string? label = null);
    Task<long> CreateInventoryIngredient(long ingredientId);
    Task DeleteInventoryIngredient(long ingredientId);
    Task<long> AddLabelToIngredient(long ingredientId, long labelId);
    Task RemoveLabelFromIngredient(long ingredientId, string label);
    Task<IEnumerable<long>> GetIngredientIdsForLabel(string label);
    Task DeleteLabel(string label);
}
