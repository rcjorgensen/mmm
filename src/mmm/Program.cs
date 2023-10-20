using System.Data.SQLite;
using System.Reflection;

using CommandLine;
using Dapper;
using Microsoft.Extensions.Configuration;
using mmm;
using mmm.Options;

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


var installDir = Environment.GetEnvironmentVariable("MMM_INSTALL_DIR");

if (installDir == null)
{
    Console.WriteLine("ERROR: Could not read environment variable `MMM_INSTALL_DIR`");
    return;
}

// Reading configuration


var appsettings = new ConfigurationBuilder()
    .SetBasePath(installDir)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var configuration = appsettings.Get<Configuration>();

if (configuration == null)
{
    Console.WriteLine("ERROR: Could not read configuration");
    return;
}

var databaseFilePath = configuration.DatabaseFilePath;
if (databaseFilePath == null)
{
    Console.WriteLine("ERROR: Could not get database file path from configuration");
    return;
}

configuration.DatabaseFilePath = Path.Combine(installDir, databaseFilePath);

var dataFilePath = configuration.DataFilePath;
if (dataFilePath != null)
{
    configuration.DataFilePath = Path.Combine(installDir, dataFilePath);
}

// Creating composition root using Pure DI


using var sqlConnection = new SQLiteConnection($"Data Source={configuration.DatabaseFilePath}");

IApplication app = new Application(
    configuration,
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
