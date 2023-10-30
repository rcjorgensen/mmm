using System.Data.SQLite;

using Dapper;

using Recipizer.Core;

DefaultTypeMap.MatchNamesWithUnderscores = true;

var builder = WebApplication.CreateBuilder(args);

var databaseFilePath = Environment.GetEnvironmentVariable("R7R_DB_PATH");

if (databaseFilePath == null)
{
    Console.WriteLine("ERROR: Could find R7R_DB_PATH environment variable");
    return;
}

using var sqlConnection = new SQLiteConnection($"Data Source={databaseFilePath}");

builder.Services.AddSingleton(sqlConnection);
builder.Services.AddSingleton<Repository>();

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();
