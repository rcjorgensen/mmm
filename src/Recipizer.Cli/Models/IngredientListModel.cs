namespace Recipizer.Cli.Models;

internal sealed class IngredientListModel
{
    public required long IngredientId { get; set; }
    public required string Name { get; set; }
}

internal sealed class InventoryListModel
{
    public required long IngredientId { get; set; }
    public required string Name { get; set; }
    public required string Added { get; set; }
}
