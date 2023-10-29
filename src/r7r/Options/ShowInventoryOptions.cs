using CommandLine;

namespace r7r.Options;

[Verb("show-inventory", aliases: new[] { "sinv" }, HelpText = "Show ingredients in inventory")]
internal sealed class ShowInventoryOptions
{
    [Option('n', "name", HelpText = "Filter by name of ingredient, supports % as wildcard")]
    public string? Name { get; set; }

    [Option('l', "label", HelpText = "Filter by label")]
    public string? Label { get; set; }

    [Option('t', "take", HelpText = "Limit the number of ingredients shown")]
    public int? Take { get; set; }
}
