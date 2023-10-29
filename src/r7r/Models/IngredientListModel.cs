namespace r7r.Models;

internal sealed class IngredientListModel
{
    public required long IngredientId { get; set; }
    public required string Name { get; set; }
    public string Added { get; set; } = string.Empty;
    public List<string> Labels { get; set; } = new List<string>();
    public string JoinedLabels => string.Join(", ", Labels);
}
