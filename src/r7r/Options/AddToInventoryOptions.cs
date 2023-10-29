using CommandLine;

namespace r7r.Options;

[Verb("add-to-inventory", aliases: new[] { "ainv" }, HelpText = "Add ingredient to inventory")]
internal sealed class AddToInventoryOptions
{
    [Value(0, Required = true, HelpText = "The IDs of the ingredient to add to the inventory")]
    public required IEnumerable<long> IngredientIds { get; set; }
}
