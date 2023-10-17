using Recipizer.Cli.Models;

namespace Recipizer.Cli;

internal static class HtmlSerializer
{
    public static string SerializeRecipes(IEnumerable<RecipeListModel> recipes)
    {
        recipes = recipes.ToList();

        var idHeader = "Id";
        var nameHeader = "Name";
        var detailsHeader = "Details";

        var tb = new HtmlTableBuilder(3);

        tb.AppendHeaderRow(idHeader, nameHeader, detailsHeader);

        foreach (var recipe in recipes)
        {
            var recipeId = recipe.RecipeId.ToString();

            tb.AppendRow(recipeId, recipe.Name, recipe.Details);
        }

        return tb.Build();
    }

    internal static string SerializeIngredients(IEnumerable<IngredientListModel> ingredients)
    {
        ingredients = ingredients.ToList();

        var idHeader = "Id";
        var nameHeader = "Name";

        var tb = new HtmlTableBuilder(2);

        tb.AppendHeaderRow(idHeader, nameHeader);

        foreach (var ingredient in ingredients)
        {
            var ingredientId = ingredient.IngredientId.ToString();

            tb.AppendRow(ingredientId, ingredient.Name);
        }

        return tb.Build();
    }

    internal static string SerializeIngredientsWithAdded(IEnumerable<IngredientListModel> inventory)
    {
        inventory = inventory.ToList();

        var idHeader = "Id";
        var nameHeader = "Name";
        var addedHeader = "Added";

        var tb = new HtmlTableBuilder(3);

        tb.AppendHeaderRow(idHeader, nameHeader, addedHeader);

        foreach (var ingredient in inventory)
        {
            var ingredientId = ingredient.IngredientId.ToString();

            tb.AppendRow(ingredientId, ingredient.Name, ingredient.Added);
        }

        return tb.Build();
    }
}
