using mmm.Models;

namespace mmm;

internal static class MarkdownSerializer
{
    public static string SerializeRecipes(IEnumerable<RecipeListModel> recipes)
    {
        recipes = recipes.ToList();

        var idHeader = "Id";
        var nameHeader = "Name";
        var detailsHeader = "Details";

        var columnInnerWidths = new int[3];

        // MaxBy should never return null here
        columnInnerWidths[0] = Math.Max(
            idHeader.Length,
            recipes.Select(x => x.RecipeId.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        columnInnerWidths[1] = Math.Max(
            nameHeader.Length,
            recipes.Select(x => x.Name.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        columnInnerWidths[2] = Math.Max(
            detailsHeader.Length,
            recipes.Select(x => x.Details.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        var tb = new MarkdownTableBuilder(columnInnerWidths);

        tb.AppendRow(idHeader, nameHeader, detailsHeader);
        tb.AppendSeparator();

        foreach (var recipe in recipes)
        {
            var recipeId = recipe.RecipeId.ToString();

            tb.AppendRow(recipeId, recipe.Name, recipe.Details);
        }

        return tb.Build();
    }

    internal static string SerializeRecipesWithIngredients(IEnumerable<RecipeListModel> recipes)
    {
        recipes = recipes.ToList();

        var ingredientSubTables = new Dictionary<RecipeListModel, string>();
        foreach (var recipe in recipes)
        {
            ingredientSubTables.Add(
                recipe,
                HtmlSerializer.SerializeIngredients(recipe.AllIngredients)
            );
        }

        var idHeader = "Id";
        var nameHeader = "Name";
        var detailsHeader = "Details";
        var ingredientsHeader = "Ingredients";

        var columnInnerWidths = new int[4];

        // MaxBy should never return null here
        columnInnerWidths[0] = Math.Max(
            idHeader.Length,
            recipes.Select(x => x.RecipeId.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        columnInnerWidths[1] = Math.Max(
            nameHeader.Length,
            recipes.Select(x => x.Name.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        columnInnerWidths[2] = Math.Max(
            detailsHeader.Length,
            recipes.Select(x => x.Details.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        columnInnerWidths[3] = Math.Max(
            ingredientsHeader.Length,
            ingredientSubTables.Values.MaxBy(i => i.Length)?.Length ?? 0
        );

        var tb = new MarkdownTableBuilder(columnInnerWidths);

        tb.AppendRow(idHeader, nameHeader, detailsHeader, ingredientsHeader);
        tb.AppendSeparator();

        foreach (var recipe in recipes)
        {
            var recipeId = recipe.RecipeId.ToString();

            tb.AppendRow(recipeId, recipe.Name, recipe.Details, ingredientSubTables[recipe]);
        }

        return tb.Build();
    }

    internal static string SerializeIngredients(IEnumerable<IngredientListModel> ingredients)
    {
        ingredients = ingredients.ToList();

        var idHeader = "Id";
        var nameHeader = "Name";

        var columnInnerWidths = new int[2];

        // MaxBy should never return null here
        columnInnerWidths[0] = Math.Max(
            idHeader.Length,
            ingredients.Select(x => x.IngredientId.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        columnInnerWidths[1] = Math.Max(
            nameHeader.Length,
            ingredients.Select(x => x.Name.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        var tb = new MarkdownTableBuilder(columnInnerWidths);

        tb.AppendRow(idHeader, nameHeader);
        tb.AppendSeparator();

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

        var columnInnerWidths = new int[3];

        // MaxBy should never return null here
        columnInnerWidths[0] = Math.Max(
            idHeader.Length,
            inventory.Select(x => x.IngredientId.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        columnInnerWidths[1] = Math.Max(
            nameHeader.Length,
            inventory.Select(x => x.Name.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        columnInnerWidths[2] = Math.Max(
            addedHeader.Length,
            inventory.Select(x => x.Added.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        var tb = new MarkdownTableBuilder(columnInnerWidths);

        tb.AppendRow(idHeader, nameHeader, addedHeader);
        tb.AppendSeparator();

        foreach (var ingredient in inventory)
        {
            var ingredientId = ingredient.IngredientId.ToString();

            tb.AppendRow(ingredientId, ingredient.Name, ingredient.Added);
        }

        return tb.Build();
    }
}
