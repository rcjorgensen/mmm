namespace Recipizer.Cli.Models;

internal sealed class RecipeListModel
{
    public required long RecipeId { get; set; }
    public required string Name { get; set; }
    public string Details { get; set; } = "";
}
