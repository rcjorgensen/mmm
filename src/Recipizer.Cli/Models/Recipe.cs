namespace Recipizer.Cli.Models;

internal record Recipe(string Name, string Details, List<string> Ingredients);

internal record RecipesJson(List<Recipe> Recipes);
