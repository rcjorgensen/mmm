using mmm.Models;

namespace mmm;

internal sealed class Serializer : ISerializer
{
    public string SerializeRecipes(IEnumerable<RecipeListModel> recipes)
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

        var tb = new TableBuilder(columnInnerWidths);

        tb.AppendTop();
        tb.AppendRow(idHeader, nameHeader, detailsHeader);
        tb.AppendSeparator();

        foreach (var recipe in recipes)
        {
            var recipeId = recipe.RecipeId.ToString();

            tb.AppendRow(recipeId, recipe.Name, recipe.Details);
        }

        tb.AppendBottom();

        return tb.Build();
    }

    public string SerializeRecipesWithIngredients(IEnumerable<RecipeListModel> recipes)
    {
        recipes = recipes.ToList();

        var ingredientSubTables = new Dictionary<RecipeListModel, string[]>();
        foreach (var recipe in recipes)
        {
            ingredientSubTables.Add(
                recipe,
                SerializeIngredients(recipe.AllIngredients)
                    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
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
            (from x in ingredientSubTables.Values from y in x select y).MaxBy(i => i.Length)?.Length
                ?? 0
        );

        var tb = new TableBuilder(columnInnerWidths);

        tb.AppendTop();
        tb.AppendRow(idHeader, nameHeader, detailsHeader, ingredientsHeader);
        tb.AppendSeparator();

        foreach (var recipe in recipes)
        {
            var recipeId = recipe.RecipeId.ToString();

            tb.AppendRow(
                new[] { recipeId },
                new[] { recipe.Name },
                new[] { recipe.Details },
                ingredientSubTables[recipe]
            );
        }

        tb.AppendBottom();

        return tb.Build();
    }

    public string SerializeIngredients(IEnumerable<IngredientListModel> ingredients)
    {
        ingredients = ingredients.ToList();

        var idHeader = "Id";
        var nameHeader = "Name";
        var addedHeader = "Added";
        var labelsHeader = "Labels";

        var columnInnerWidths = new int[4];

        // MaxBy should never return null here
        columnInnerWidths[0] = Math.Max(
            idHeader.Length,
            ingredients.Select(x => x.IngredientId.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        columnInnerWidths[1] = Math.Max(
            nameHeader.Length,
            ingredients.Select(x => x.Name.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        columnInnerWidths[2] = Math.Max(
            addedHeader.Length,
            ingredients.Select(x => x.Added.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        columnInnerWidths[3] = Math.Max(
            labelsHeader.Length,
            ingredients.Select(x => x.JoinedLabels).MaxBy(i => i.Length)?.Length ?? 0
        );

        var tb = new TableBuilder(columnInnerWidths);
        tb.AppendTop();

        tb.AppendRow(idHeader, nameHeader, addedHeader, labelsHeader);

        tb.AppendSeparator();

        foreach (var ingredient in ingredients)
        {
            var ingredientId = ingredient.IngredientId.ToString();

            tb.AppendRow(ingredientId, ingredient.Name, ingredient.Added, ingredient.JoinedLabels);
        }

        tb.AppendBottom();

        return tb.Build();
    }
}
