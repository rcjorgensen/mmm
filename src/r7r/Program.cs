using System.Data.SQLite;

using CommandLine;

using Dapper;

using r7r;
using r7r.Options;

using Recipizer.Core;

// Here we should handle:
// * Setting global state e.g. configuring Dapper
// * Reading environment variables
// * Reading configuration
// * Creating a composition root i.e. wiring up application with dependencies (we will be using Pure DI https://blog.ploeh.dk/2014/06/10/pure-di/)
// * Parsing command line arguments
// * Writing output

// Global state


DefaultTypeMap.MatchNamesWithUnderscores = true;

// Environment variables


var databaseFilePath = Environment.GetEnvironmentVariable("R7R_DB_PATH");

if (databaseFilePath == null)
{
    Console.WriteLine("ERROR: Could find R7R_DB_PATH environment variable");
    return;
}

// Creating composition root using Pure DI


using var sqlConnection = new SQLiteConnection($"Data Source={databaseFilePath}");

IApplication app = new Application(
    databaseFilePath,
    new Repository(sqlConnection),
    new FileSystem(),
    new Deserializer(),
    new Serializer()
);

// Parsing and mapping command line arguments


var result = await Parser.Default
    .ParseArguments<
        AddIngredientOptions,
        AddLabelOptions,
        RemoveLabelOptions,
        AddRecipeOptions,
        AddToInventoryOptions,
        ImportOptions,
        InitializeOptions,
        ShowIngredientsOptions,
        ShowInventoryOptions,
        ShowMissingIngredientsOptions,
        ShowRecipesOptions,
        RemoveFromInventoryOptions,
        RemoveRecipeOptions
    >(args)
    .MapResult(
        (AddIngredientOptions opts) => app.AddIngredient(opts),
        (AddLabelOptions opts) => app.AddLabel(opts),
        (RemoveLabelOptions opts) => app.RemoveLabel(opts),
        (AddRecipeOptions opts) => app.AddRecipe(opts),
        (AddToInventoryOptions opts) => app.AddToInventory(opts),
        (ImportOptions opts) => app.Import(opts),
        (InitializeOptions opts) => app.Initialize(opts),
        (RemoveFromInventoryOptions opts) => app.RemoveFromInventory(opts),
        (RemoveRecipeOptions opts) => app.RemoveRecipe(opts),
        (ShowIngredientsOptions opts) => app.ShowIngredients(opts),
        (ShowInventoryOptions opts) => app.ShowInventory(opts),
        (ShowMissingIngredientsOptions opts) => app.ShowMissingIngredients(opts),
        (ShowRecipesOptions opts) => app.ShowRecipes(opts),
        errs => Task.FromResult(string.Empty)
    );

// Presenting result

if (!string.IsNullOrWhiteSpace(result))
{
    Console.WriteLine(result);
}
