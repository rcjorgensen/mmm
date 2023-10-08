using CommandLine;

namespace Recipizer.Cli.Options;

[Verb("ingredients")]
internal sealed class IngredientsOptions
{
    [Option('l', "list", SetName = "list")]
    public bool List { get; set; }

    [Option("missing", SetName = "list")]
    public bool Missing { get; set; }

    [Option('m', "match", SetName = "list")]
    public string? Match { get; set; }

    [Option('a', "add", SetName = "add")]
    public bool Add { get; set; }

    [Option("name", SetName = "add")]
    public required string Name { get; set; }

    [Option('r', "remove", SetName = "remove")]
    public bool Remove { get; set; }

    [Option("id", SetName = "remove")]
    public long Id { get; set; }

    [Option("add-to-recipe", SetName = "add-to-recipe")]
    public bool AddToRecipe { get; set; }

    [Option("recipe-id", SetName = "add-to-recipe")]
    public long RecipeId { get; set; }
}
