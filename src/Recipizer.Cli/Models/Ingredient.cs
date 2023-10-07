namespace Recipizer.Cli.Models;

internal record Ingredient(long IngredientId, string Name, bool InStock)
{
    public Ingredient(long IngredientId, string Name, long InStock)
        : this(IngredientId, Name, InStock == 1) { }

    public Ingredient(long IngredientId, string Name)
        : this(IngredientId, Name, false) { }
}
