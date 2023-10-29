namespace r7r.Models;

internal sealed class RecipeInitModel
{
    public required string Name { get; set; }
    public required string Details { get; set; }
    public required List<string> Ingredients { get; set; }
}
