using CommandLine;

namespace Recipizer.Cli.Options;

[Verb("ingredients")]
internal sealed class IngredientsOptions
{
    [Option('l', "list", SetName = "list")]
    public bool List { get; set; }

    [Option("missing", SetName = "missing")]
    public bool Missing { get; set; }

    [Option("inventory", SetName = "inventory")]
    public bool Inventory { get; set; }

    [Option('m', "match")]
    public string? Match { get; set; }

    [Option('a', "add", SetName = "add")]
    public bool Add { get; set; }

    [Option('i', "to-inventory", SetName = "add")]
    public IEnumerable<long> ToInventory { get; set; } = new List<long>();

    [Option('r', "remove", SetName = "remove")]
    public bool Remove { get; set; }

    [Option('o', "from-inventory", SetName = "remove")]
    public IEnumerable<long> FromInventory { get; set; } = new List<long>();
}
