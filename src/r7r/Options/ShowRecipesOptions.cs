using CommandLine;

namespace r7r.Options;

[Verb("show-recipes", aliases: new[] { "sr" }, HelpText = "Show recipes")]
internal sealed class ShowRecipesOptions
{
    [Option('n', "name", HelpText = "Filter by name of recipe, supports % as wildcard")]
    public string? Name { get; set; }

    [Option('t', "take", HelpText = "Limit the number of recipes shown")]
    public int? Take { get; set; }

    [Option(
        'm',
        "order-by-missing-ingredients",
        HelpText = "Order by missing ingredients, helpful for finding recipes that require fewest new ingredients to be bought"
    )]
    public bool OrderByMissing { get; set; }

    [Option(
        'a',
        "order-by-total-ingredients",
        HelpText = "Order by total ingredients, helpful for finding recipes that should be simple to make"
    )]
    public bool OrderByTotal { get; set; }

    [Option(
        'i',
        "order-by-in-inventory",
        HelpText = "Order by ingredients in inventory, helpful for finding recipes that uses up the inventory"
    )]
    public bool OrderByInInventory { get; set; }

    [Option(
        'l',
        "order-by-label",
        HelpText = "Order by ingredients with label, helpful for finding recipes based on a custom criteria"
    )]
    public string? OrderByLabel { get; set; }

    [Option("markdown", HelpText = "Use markdown format (experimental)")]
    public bool Markdown { get; set; }
}
