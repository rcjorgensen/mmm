using CommandLine;

namespace r7r.Options;

[Verb("add-ingredient", aliases: new[] { "ai" }, HelpText = "Add ingredient to database")]
internal sealed class AddIngredientOptions
{
    [Value(0, Required = true, HelpText = "The name of the ingredient")]
    public required string Name { get; set; }
}
