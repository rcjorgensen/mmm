namespace Recipizer.Cli.Models;

internal sealed class IngredientListModel
{
    public required long IngredientId { get; set; }
    public required string Name { get; set; }
    public string Added { get; set; } = string.Empty;
}
