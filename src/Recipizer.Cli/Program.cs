using System.Data.SQLite;
using CommandLine;
using CommandLine.Text;
using Dapper;
using Microsoft.Extensions.Configuration;
using Recipizer.Cli;
using Recipizer.Cli.Options;

// Here we should handle:
// * Setting global state e.g. configuring Dapper
// * Reading configuration
// * Creating a composition root i.e. wiring up application with dependencies (we will be using Pure DI https://blog.ploeh.dk/2014/06/10/pure-di/)
// * Parsing command line arguments
// * Presenting i.e. calling static methods on `Console`

// Global state


DefaultTypeMap.MatchNamesWithUnderscores = true;

// Reading configuration


var appsettings = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var configuration = appsettings.Get<Configuration>();

if (configuration == null)
{
    Console.WriteLine("ERROR: Could not read configuration");
    return;
}

if (configuration.DatabaseFilePath == null)
{
    Console.WriteLine("ERROR: Could not get database file path from configuration");
    return;
}

// Creating composition root using Pure DI

var repository = new Repository(
    new SQLiteConnection($"Data Source={configuration.DatabaseFilePath}")
);

var app = new Application(configuration, repository, new FileSystem(), new Deserializer());

// Parsing command line arguments


var result = await Parser.Default
    .ParseArguments<InitOptions, RecipesOptions, IngredientsOptions>(args)
    .MapResult(
        (InitOptions opts) => app.Init(opts),
        (RecipesOptions opts) => app.Recipes(opts),
        (IngredientsOptions opts) => app.Ingredients(opts),
        errs => Task.FromResult(string.Empty)
    );

// Presenting result

if (!string.IsNullOrWhiteSpace(result))
{
    Console.WriteLine(result);
}
