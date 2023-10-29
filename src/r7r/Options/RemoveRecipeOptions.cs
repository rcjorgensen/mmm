using CommandLine;

namespace r7r.Options;

[Verb("remove-recipe", aliases: new[] { "rr" }, HelpText = "Removes a recipe from the database")]
internal sealed class RemoveRecipeOptions
{
    [Option('i', "id", HelpText = "The ID of the recipe to remove")]
    public long RecipeId { get; set; }
}
