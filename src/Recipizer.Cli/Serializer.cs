using System.Text;
using Recipizer.Cli.Models;

namespace Recipizer.Cli;

internal sealed class Serializer
{
    public string SerializeRecipes(IEnumerable<RecipeListModel> recipes)
    {
        recipes = recipes.ToList();

        var idHeader = "Id";
        var nameHeader = "Name";
        var detailsHeader = "Details";
        var horizontalPadding = 2;

        // MaxBy should never return null here
        var idInnerWidth = Math.Max(
            idHeader.Length,
            recipes.Select(x => x.RecipeId.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        var nameInnerWidth = Math.Max(
            nameHeader.Length,
            recipes.Select(x => x.Name.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );
        var detailsInnerWidth = Math.Max(
            detailsHeader.Length,
            recipes.Select(x => x.Details.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        var idOuterWidth = idInnerWidth + horizontalPadding;
        var nameOuterWidth = nameInnerWidth + horizontalPadding;
        var detailsOuterWidth = detailsInnerWidth + horizontalPadding;

        var sb = new StringBuilder();
        sb.AppendTop(idOuterWidth, nameOuterWidth, detailsOuterWidth);

        sb.AppendRow(
            (idInnerWidth, idHeader, true),
            (nameInnerWidth, nameHeader, false),
            (detailsInnerWidth, detailsHeader, false)
        );

        sb.AppendSeparator(idOuterWidth, nameOuterWidth, detailsOuterWidth);

        foreach (var recipe in recipes)
        {
            var recipeId = recipe.RecipeId.ToString();

            sb.AppendRow(
                (idInnerWidth, recipeId, true),
                (nameInnerWidth, recipe.Name, false),
                (detailsInnerWidth, recipe.Details, false)
            );
        }

        sb.AppendBottom(idOuterWidth, nameOuterWidth, detailsOuterWidth);

        return sb.ToString();
    }

    internal string SerializeRecipesWithIngredients(
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

        var horizontalPadding = 2;

        // MaxBy should never return null here
        var idInnerWidth = Math.Max(
            idHeader.Length,
            recipes.Select(x => x.RecipeId.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        var nameInnerWidth = Math.Max(
            nameHeader.Length,
            recipes.Select(x => x.Name.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        var detailsInnerWidth = Math.Max(
            detailsHeader.Length,
            recipes.Select(x => x.Details.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        var ingredientsInnerWidth = Math.Max(
            ingredientsHeader.Length,
            (from x in ingredientSubTables.Values from y in x select y).MaxBy(i => i.Length)?.Length
                ?? 0
        );

        var idOuterWidth = idInnerWidth + horizontalPadding;
        var nameOuterWidth = nameInnerWidth + horizontalPadding;
        var detailsOuterWidth = detailsInnerWidth + horizontalPadding;
        var ingredientsOuterWidth = ingredientsInnerWidth + horizontalPadding;

        var sb = new StringBuilder();
        sb.AppendTop(idOuterWidth, nameOuterWidth, detailsOuterWidth, ingredientsOuterWidth);

        sb.AppendRow(
            (idInnerWidth, idHeader, true),
            (nameInnerWidth, nameHeader, false),
            (detailsInnerWidth, detailsHeader, false),
            (ingredientsInnerWidth, ingredientsHeader, false)
        );

        sb.AppendSeparator(idOuterWidth, nameOuterWidth, detailsOuterWidth, ingredientsOuterWidth);

        foreach (var recipe in recipes)
        {
            var recipeId = recipe.RecipeId.ToString();

            sb.AppendRow(
                (idInnerWidth, new[] { recipeId }, true),
                (nameInnerWidth, new[] { recipe.Name }, false),
                (detailsInnerWidth, new[] { recipe.Details }, false),
                (ingredientsInnerWidth, ingredientSubTables[recipe], false)
            );
        }

        sb.AppendBottom(idOuterWidth, nameOuterWidth, detailsOuterWidth, ingredientsOuterWidth);

        return sb.ToString();
    }

    internal string SerializeIngredients(IEnumerable<IngredientListModel> ingredients)
    {
        ingredients = ingredients.ToList();

        var idHeader = "Id";
        var nameHeader = "Name";
        var horizontalPadding = 2;

        // MaxBy should never return null here
        var idInnerWidth = Math.Max(
            idHeader.Length,
            ingredients.Select(x => x.IngredientId.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        var nameInnerWidth = Math.Max(
            nameHeader.Length,
            ingredients.Select(x => x.Name.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        var idOuterWidth = idInnerWidth + horizontalPadding;
        var nameOuterWidth = nameInnerWidth + horizontalPadding;

        var sb = new StringBuilder();
        sb.AppendTop(idOuterWidth, nameOuterWidth);

        sb.AppendRow((idInnerWidth, idHeader, true), (nameInnerWidth, nameHeader, false));

        sb.AppendSeparator(idOuterWidth, nameOuterWidth);

        foreach (var ingredient in ingredients)
        {
            var ingredientId = ingredient.IngredientId.ToString();

            sb.AppendRow(
                (idInnerWidth, ingredientId, true),
                (nameInnerWidth, ingredient.Name, false)
            );
        }

        sb.AppendBottom(idOuterWidth, nameOuterWidth);

        return sb.ToString();
    }

    internal string SerializeIngredientsWithAdded(IEnumerable<IngredientListModel> inventory)
    {
        inventory = inventory.ToList();

        var idHeader = "Id";
        var nameHeader = "Name";
        var addedHeader = "Added";
        var horizontalPadding = 2;

        // MaxBy should never return null here
        var idInnerWidth = Math.Max(
            idHeader.Length,
            inventory.Select(x => x.IngredientId.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        var nameInnerWidth = Math.Max(
            nameHeader.Length,
            inventory.Select(x => x.Name.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        var addedInnerWidth = Math.Max(
            addedHeader.Length,
            inventory.Select(x => x.Added.ToString()).MaxBy(i => i.Length)?.Length ?? 0
        );

        var idOuterWidth = idInnerWidth + horizontalPadding;
        var nameOuterWidth = nameInnerWidth + horizontalPadding;
        var addedOuterWidth = addedInnerWidth + horizontalPadding;

        var sb = new StringBuilder();
        sb.AppendTop(idOuterWidth, nameOuterWidth, addedOuterWidth);

        sb.AppendRow(
            (idInnerWidth, idHeader, true),
            (nameInnerWidth, nameHeader, false),
            (addedInnerWidth, addedHeader, false)
        );

        sb.AppendSeparator(idOuterWidth, nameOuterWidth, addedOuterWidth);

        foreach (var ingredient in inventory)
        {
            var ingredientId = ingredient.IngredientId.ToString();

            sb.AppendRow(
                (idInnerWidth, ingredientId, true),
                (nameInnerWidth, ingredient.Name, false),
                (addedInnerWidth, ingredient.Added, false)
            );
        }

        sb.AppendBottom(idOuterWidth, nameOuterWidth, addedOuterWidth);

        return sb.ToString();
    }
}
