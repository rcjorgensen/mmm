using Recipizer.Cli.Models;

namespace Recipizer.Cli;

internal static class Serializer
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

    internal static string SerializeRecipesWithIngredients(
        IEnumerable<RecipeListModel> recipes,
        IngredientList ingredientList = IngredientList.All
    )
    {
        recipes = recipes.ToList();

        var ingredientSubTables = new Dictionary<RecipeListModel, string[]>();
        foreach (var recipe in recipes)
        {
            ingredientSubTables.Add(
                recipe,
                ingredientList switch
                {
                    IngredientList.All
                        => SerializeIngredientsWithAdded(recipe.AllIngredients)
                            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    IngredientList.Missing
                        => SerializeIngredients(recipe.MissingIngredients)
                            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    IngredientList.Inventory
                        => SerializeIngredientsWithAdded(recipe.InventoryIngredients)
                            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    _ => throw new NotImplementedException()
                }
            );
        }

        var idHeader = "Id";
        var nameHeader = "Name";
        var detailsHeader = "Details";
        var ingredientsHeader = ingredientList switch
        {
            IngredientList.All => "Ingredients",
            IngredientList.Missing => "Ingredients missing",
            IngredientList.Inventory => "Ingredients in inventory",
            _ => throw new NotImplementedException()
        };

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

        var tb = new TableBuilder(columnInnerWidths);

        tb.AppendTop();
        tb.AppendRow(idHeader, nameHeader);
        tb.AppendSeparator();

        foreach (var ingredient in ingredients)
        {
            var ingredientId = ingredient.IngredientId.ToString();

            tb.AppendRow(ingredientId, ingredient.Name);
        }

        tb.AppendBottom();

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

        var tb = new TableBuilder(columnInnerWidths);
        tb.AppendTop();

        tb.AppendRow(idHeader, nameHeader, addedHeader);

        tb.AppendSeparator();

        foreach (var ingredient in inventory)
        {
            var ingredientId = ingredient.IngredientId.ToString();

            tb.AppendRow(ingredientId, ingredient.Name, ingredient.Added);
        }

        tb.AppendBottom();

        return tb.Build();
    }

    internal static string SerializeIngredientsToMarkdown(
        IEnumerable<IngredientListModel> ingredients
    )
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
}
