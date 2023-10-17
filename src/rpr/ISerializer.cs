using Recipizer.Cli.Models;

namespace Recipizer.Cli;

internal interface ISerializer
{
    string SerializeRecipes(IEnumerable<RecipeListModel> recipes);
    string SerializeRecipesWithIngredients(IEnumerable<RecipeListModel> recipes);
    string SerializeIngredients(IEnumerable<IngredientListModel> inventory);
}
