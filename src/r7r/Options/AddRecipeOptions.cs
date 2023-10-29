using CommandLine;

namespace r7r.Options;

[Verb("add-recipe", aliases: new[] { "ar" }, HelpText = "Add a recipe to the database")]
internal sealed class AddRecipeOptions
{
    [Option('n', "name", HelpText = "The name of the recipe")]
    public required string Name { get; set; }

    [Option('d', "details", HelpText = "Details of the recipe")]
    public string? Details { get; set; }
}
