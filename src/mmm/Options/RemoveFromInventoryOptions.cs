using CommandLine;

namespace mmm.Options;

[Verb(
    "remove-from-inventory",
    aliases: new[] { "rinv" },
    HelpText = "Remove ingredient from inventory"
)]
internal sealed class RemoveFromInventoryOptions
{
    [Value(0, Required = true, HelpText = "The ID of the ingredient to remove from the inventory")]
    public required IEnumerable<long> IngredientIds { get; set; }
}
