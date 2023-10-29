using r7r.Models;

namespace r7r;

internal interface IDeserializer
{
    List<RecipeInitModel>? DeserializeRecipes(string data);
    string? DeserializeRecipeSource(string data);
    string[]? DeserializeLabels(string data);
    string[]? DeserializeInventory(string data);
}
