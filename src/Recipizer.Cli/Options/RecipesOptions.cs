using CommandLine;

namespace Recipizer.Cli.Options;

[Verb("recipes")]
internal sealed class RecipesOptions
{
    [Option('l', "list", SetName = "list")]
    public bool List { get; set; }

    [Option('m', "match", SetName = "list")]
    public string? Match { get; set; }

    [Option('t', "take", SetName = "list")]
    public int? Take { get; set; }

    [Option('i', "with-ingredients", SetName = "list")]
    public bool WithIngredients { get; set; }

    [Option("with-missing-ingredients", SetName = "list")]
    public bool WithMissingIngredients { get; set; }

    [Option("with-inventory-ingredients", SetName = "list")]
    public bool WithInventoryIngredients { get; set; }

    [Option("by-fewest", SetName = "list")]
    public bool ByFewest { get; set; }

    [Option("by-fewest-missing", SetName = "list")]
    public bool ByFewestMissing { get; set; }

    [Option("by-most-in-inventory", SetName = "list")]
    public bool ByMostInInventory { get; set; }

    [Option('a', "add", SetName = "add")]
    public bool Add { get; set; }

    [Option("name", SetName = "add")]
    public required string Name { get; set; }

    [Option("details", SetName = "add")]
    public string? Details { get; set; }

    [Option('r', "remove", SetName = "remove")]
    public bool Remove { get; set; }

    [Option("id", SetName = "remove")]
    public long Id { get; set; }
}
