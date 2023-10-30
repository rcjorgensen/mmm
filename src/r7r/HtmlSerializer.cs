using Recipizer.Core.Models;

namespace r7r;

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

    internal static string SerializeIngredients(IEnumerable<IngredientListModel> inventory)
    {
        inventory = inventory.ToList();

        var idHeader = "Id";
        var nameHeader = "Name";
        var addedHeader = "Added";
        var labelsHeader = "Labels";

        var tb = new HtmlTableBuilder(4);

        tb.AppendHeaderRow(idHeader, nameHeader, addedHeader, labelsHeader);

        foreach (var ingredient in inventory)
        {
            var ingredientId = ingredient.IngredientId.ToString();

            tb.AppendRow(ingredientId, ingredient.Name, ingredient.Added, ingredient.JoinedLabels);
        }

        return tb.Build();
    }
}
