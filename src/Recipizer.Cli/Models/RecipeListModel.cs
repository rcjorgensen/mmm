namespace Recipizer.Cli.Models;

internal sealed class RecipeListModel
{
    public required long RecipeId { get; set; }
    public required string Name { get; set; }
    public string Details { get; set; } = "";
    public List<IngredientListModel> AllIngredients { get; set; } = new List<IngredientListModel>();
    public List<IngredientListModel> MissingIngredients { get; set; } =
        new List<IngredientListModel>();
    public List<IngredientListModel> InventoryIngredients { get; set; } =
        new List<IngredientListModel>();
}
