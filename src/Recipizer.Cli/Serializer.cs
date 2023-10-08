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

        sb.AppendRowFirstColumnRightAdjusted(
            (idInnerWidth, idHeader),
            (nameInnerWidth, nameHeader),
            (detailsInnerWidth, detailsHeader)
        );

        sb.AppendSeparator(idOuterWidth, nameOuterWidth, detailsOuterWidth);

        foreach (var recipe in recipes)
        {
            var recipeId = recipe.RecipeId.ToString();

            sb.AppendRowFirstColumnRightAdjusted(
                (idInnerWidth, recipeId),
                (nameInnerWidth, recipe.Name),
                (detailsInnerWidth, recipe.Details)
            );
        }

        sb.AppendBottom(idOuterWidth, nameOuterWidth, detailsOuterWidth);

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

        sb.AppendRowFirstColumnRightAdjusted(
            (idInnerWidth, idHeader),
            (nameInnerWidth, nameHeader)
        );

        sb.AppendSeparator(idOuterWidth, nameOuterWidth);

        foreach (var ingredient in ingredients)
        {
            var ingredientId = ingredient.IngredientId.ToString();

            sb.AppendRowFirstColumnRightAdjusted(
                (idInnerWidth, ingredientId),
                (nameInnerWidth, ingredient.Name)
            );
        }

        sb.AppendBottom(idOuterWidth, nameOuterWidth);

        return sb.ToString();
    }

    internal string SerializeInventory(IEnumerable<InventoryListModel> inventory)
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

        sb.AppendRowFirstColumnRightAdjusted(
            (idInnerWidth, idHeader),
            (nameInnerWidth, nameHeader),
            (addedInnerWidth, addedHeader)
        );

        sb.AppendSeparator(idOuterWidth, nameOuterWidth, addedOuterWidth);

        foreach (var ingredient in inventory)
        {
            var ingredientId = ingredient.IngredientId.ToString();

            sb.AppendRowFirstColumnRightAdjusted(
                (idInnerWidth, ingredientId),
                (nameInnerWidth, ingredient.Name),
                (addedInnerWidth, ingredient.Added)
            );
        }

        sb.AppendBottom(idOuterWidth, nameOuterWidth, addedOuterWidth);

        return sb.ToString();
    }
}
