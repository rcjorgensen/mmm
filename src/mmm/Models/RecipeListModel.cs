namespace mmm.Models;

internal sealed class RecipeListModel
{
    public required long RecipeId { get; set; }
    public required string Name { get; set; }
    public string Details { get; set; } = "";
    public List<IngredientListModel> AllIngredients { get; set; } = new List<IngredientListModel>();
    public List<IngredientListModel> MissingIngredients =>
        AllIngredients.Where(x => x.Added == string.Empty).ToList();
    public List<IngredientListModel> InventoryIngredients =>
        AllIngredients.Where(x => x.Added != string.Empty).ToList();

    public List<IngredientListModel> GetIngredientsWithLabel(string label) =>
        AllIngredients.Where(x => x.Labels.Contains(label)).ToList();
}
