namespace Recipizer.Cli.Models;

internal record Recipe(long RecipeId, string Name, string Details, List<Ingredient> Ingredients)
{
    public Recipe(long RecipeId, string Name, string Details)
        : this(RecipeId, Name, Details, new List<Ingredient>()) { }
}
