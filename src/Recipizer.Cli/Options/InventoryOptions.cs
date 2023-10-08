using CommandLine;

namespace Recipizer.Cli.Options;

[Verb("inventory")]
internal sealed class InventoryOptions
{
    [Option('l', "list", SetName = "list")]
    public bool List { get; set; }

    [Option('m', "match", SetName = "list")]
    public string? Match { get; set; }

    [Option('a', "add", SetName = "add-remove")]
    public bool Add { get; set; }

    [Option('r', "remove", SetName = "add-remove")]
    public bool Remove { get; set; }

    [Option(
        "id",
        SetName = "add-remove",
        HelpText = "ID of ingredient to add or remove from inventory"
    )]
    public long IngredientId { get; set; }
}
