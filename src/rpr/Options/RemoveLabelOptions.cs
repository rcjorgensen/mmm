using CommandLine;

namespace Recipizer.Cli.Options;

[Verb("remove-label", aliases: new[] { "rl" }, HelpText = "Remove label from ingredients")]
internal sealed class RemoveLabelOptions
{
    [Value(0, Required = true, HelpText = "The label to remove")]
    public required string Label { get; set; }

    [Value(1, Required = true, HelpText = "IDs of ingredients to remove the label from")]
    public required IEnumerable<long> IngredientIds { get; set; }
}
