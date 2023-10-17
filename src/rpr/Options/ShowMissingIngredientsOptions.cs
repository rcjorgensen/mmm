using CommandLine;

namespace Recipizer.Cli.Options;

[Verb(
    "show-missing-ingredients",
    aliases: new[] { "smi" },
    HelpText = "Show ingredients missing from inventory"
)]
internal sealed class ShowMissingIngredientsOptions
{
    [Option('n', "name", HelpText = "Filter by name of ingredient, supports % as wildcard")]
    public string? Name { get; set; }

    [Option('l', "label", HelpText = "Filter by label")]
    public string? Label { get; set; }

    [Option('t', "take", HelpText = "Limit the number of ingredients shown")]
    public int? Take { get; set; }
}
