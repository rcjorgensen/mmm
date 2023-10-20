using System.Text.Json;
using System.Text.Json.Nodes;

using mmm.Models;

namespace mmm;

internal sealed class Deserializer : IDeserializer
{
    public List<RecipeInitModel>? DeserializeRecipes(string data)
    {
        return JsonNode
            .Parse(data)
            ?["recipes"].Deserialize<List<RecipeInitModel>>(
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
            );
    }

    public string? DeserializeRecipeSource(string data)
    {
        return JsonNode
            .Parse(data)
            ?["recipeSource"].Deserialize<string>(
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
            );
    }

    public string[]? DeserializeLabels(string data)
    {
        return JsonNode
            .Parse(data)
            ?["labels"].Deserialize<string[]>(
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
            );
    }

    public string[]? DeserializeInventory(string data)
    {
        return JsonNode
            .Parse(data)
            ?["inventory"].Deserialize<string[]>(
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
            );
    }
}
