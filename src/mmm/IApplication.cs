using mmm.Options;

namespace mmm;

internal interface IApplication
{
    Task<string> AddIngredient(AddIngredientOptions options);
    Task<string> AddLabel(AddLabelOptions options);
    Task<string> AddRecipe(AddRecipeOptions options);
    Task<string> AddToInventory(AddToInventoryOptions options);
    Task<string> Import(ImportOptions options);
    Task<string> Initialize(InitializeOptions options);
    Task<string> RemoveFromInventory(RemoveFromInventoryOptions options);
    Task<string> RemoveLabel(RemoveLabelOptions options);
    Task<string> RemoveRecipe(RemoveRecipeOptions options);
    Task<string> ShowIngredients(ShowIngredientsOptions options);
    Task<string> ShowInventory(ShowInventoryOptions options);
    Task<string> ShowMissingIngredients(ShowMissingIngredientsOptions options);
    Task<string> ShowRecipes(ShowRecipesOptions options);
}
