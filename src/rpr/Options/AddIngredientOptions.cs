using CommandLine;

namespace Recipizer.Cli.Options;

[Verb("add-ingredient", aliases: new[] { "ai" }, HelpText = "Add ingredient to database")]
internal sealed class AddIngredientOptions
{
    [Value(0, Required = true, HelpText = "The name of the ingredient")]
    public required string Name { get; set; }
}
