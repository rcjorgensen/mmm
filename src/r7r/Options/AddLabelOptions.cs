using CommandLine;

namespace r7r.Options;

[Verb("add-label", aliases: new[] { "al" }, HelpText = "Add label to ingredients")]
internal sealed class AddLabelOptions
{
    [Value(0, Required = true, HelpText = "The label to add")]
    public required string Label { get; set; }

    [Value(1, Required = true, HelpText = "IDs of ingredients to add the label to")]
    public required IEnumerable<long> IngredientIds { get; set; }
}
