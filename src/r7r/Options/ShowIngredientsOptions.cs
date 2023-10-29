using CommandLine;

namespace r7r.Options;

[Verb("show-ingredients", aliases: new[] { "si" }, HelpText = "Show ingredients")]
internal sealed class ShowIngredientsOptions
{
    [Option('n', "name", HelpText = "Filter by name of ingredient, supports % as wildcard")]
    public string? Name { get; set; }

    [Option('l', "label", HelpText = "Filter by label")]
    public string? Label { get; set; }

    [Option('t', "take", HelpText = "Limit the number of ingredients shown")]
    public int? Take { get; set; }
}
