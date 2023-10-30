using Recipizer.Core.Models;

namespace r7r;

internal interface ISerializer
{
    string SerializeRecipes(IEnumerable<RecipeListModel> recipes);
    string SerializeRecipesWithIngredients(IEnumerable<RecipeListModel> recipes);
    string SerializeIngredients(IEnumerable<IngredientListModel> inventory);
}
