using System.Data.SQLite;
using Dapper;
using Recipizer.Cli.Models;

namespace Recipizer.Cli;

internal interface IRepository
{
    Task<int> InitializeSchema();
    Task<long> CreateRecipeSource(string name);
    Task<long> CreateLabel(string label);
    Task<long> CreateRecipe(string name, string? details = null, long? recipeSourceId = null);
    Task<long> GetOrCreateIngredient(string name);
    Task AddIngredientToRecipe(long recipeId, long ingredientId);
    Task<IEnumerable<RecipeListModel>> GetRecipes(string? match = null);
    Task<IEnumerable<RecipeListModel>> GetRecipesWithIngredients(string? match = null);
    Task DeleteRecipe(long id);
    Task<IEnumerable<IngredientListModel>> GetIngredients(string? match = null);
    Task<IEnumerable<IngredientListModel>> GetInventory(string? match = null);
    Task<IEnumerable<IngredientListModel>> GetMissing(string? match);
    Task CreateInventoryIngredient(long ingredientId);
    Task DeleteInventoryIngredient(long ingredientId);
}

internal sealed class Repository : IRepository
{
    private readonly SQLiteConnection connection;

    public Repository(SQLiteConnection connection)
    {
        this.connection = connection;
    }

    public Task<int> InitializeSchema()
    {
        return connection.ExecuteAsync(
            """
            CREATE TABLE IF NOT EXISTS recipe_source (
                recipe_source_id INTEGER PRIMARY KEY,
                name TEXT NOT NULL UNIQUE
            );
            
            CREATE TABLE IF NOT EXISTS recipe (
                recipe_id INTEGER PRIMARY KEY,
                name TEXT NOT NULL UNIQUE,
                recipe_source_id INTEGER,
                details TEXT,
                FOREIGN KEY (recipe_source_id) REFERENCES recipe_source (recipe_source_id)
            );
            
            CREATE TABLE IF NOT EXISTS ingredient (
                ingredient_id INTEGER PRIMARY KEY,
                name TEXT NOT NULL UNIQUE
            );
            
            CREATE TABLE IF NOT EXISTS inventory_ingredient (
                ingredient_id INTEGER NOT NULL UNIQUE,
                added TEXT DEFAULT CURRENT_DATE,
                quantity INTEGER,
                unit TEXT,
                FOREIGN KEY (ingredient_id) REFERENCES ingredient (ingredient_id) ON DELETE CASCADE
            );
            
            CREATE TABLE IF NOT EXISTS recipe_ingredient (
                recipe_id INTEGER NOT NULL,
                ingredient_id INTEGER NOT NULL,
                quantity INTEGER,
                unit TEXT,
                details TEXT,
                PRIMARY KEY (recipe_id, ingredient_id),
                FOREIGN KEY (recipe_id) REFERENCES recipe (recipe_id) ON DELETE CASCADE,
                FOREIGN KEY (ingredient_id) REFERENCES ingredient (ingredient_id) ON DELETE CASCADE
            );
            
            CREATE TABLE IF NOT EXISTS label (
                label_id INTEGER PRIMARY KEY,
                label TEXT NOT NULL
            );
            
            CREATE TABLE IF NOT EXISTS ingredient_label (
                ingredient_id INTEGER NOT NULL,
                label_id INTEGER NOT NULL,
                PRIMARY KEY (ingredient_id, label_id),
                FOREIGN KEY (ingredient_id) REFERENCES ingredient (ingredient_id) ON DELETE CASCADE,
                FOREIGN KEY (label_id) REFERENCES label (label_id) ON DELETE CASCADE
            );
            """
        );
    }

    public Task<long> CreateRecipeSource(string name)
    {
        return connection.QuerySingleAsync<long>(
            """
                INSERT INTO recipe_source
                    (name)
                VALUES 
                    (@name)
                RETURNING recipe_source_id
                """,
            new { name, }
        );
    }

    public Task<long> CreateLabel(string label)
    {
        return connection.QuerySingleAsync<long>(
            """
                INSERT INTO label
                    (label)
                VALUES 
                    (@label)
                RETURNING label_id
                """,
            new { label, }
        );
    }

    public Task<long> CreateRecipe(string name, string? details = null, long? recipeSourceId = null)
    {
        return connection.QuerySingleAsync<long>(
            """
                INSERT INTO recipe 
                    (name, recipe_source_id, details)
                VALUES 
                    (@name, @recipeSourceId, @details)
                RETURNING recipe_id
                """,
            new
            {
                name,
                recipeSourceId,
                details
            }
        );
    }

    public async Task<long> GetOrCreateIngredient(string name)
    {
        const string ingredientIdQuery = """
            SELECT 
                ingredient_id
            FROM ingredient 
            WHERE name = @name
            """;

        var ingredientId = await connection.QuerySingleOrDefaultAsync<long?>(
            ingredientIdQuery,
            new { name }
        );

        if (ingredientId == null)
        {
            await connection.ExecuteAsync(
                """
                INSERT INTO 
                    ingredient (name) 
                VALUES
                    (@name)
                """,
                new { name }
            );

            ingredientId = await connection.QuerySingleAsync<long>(ingredientIdQuery, new { name });
        }

        return ingredientId.Value;
    }

    public Task AddIngredientToRecipe(long recipeId, long ingredientId)
    {
        return connection.ExecuteAsync(
            """
            INSERT INTO 
                recipe_ingredient (recipe_id, ingredient_id) 
            VALUES 
                (@recipeId, @ingredientId)
            """,
            new { recipeId, ingredientId }
        );
    }

    public Task<IEnumerable<RecipeListModel>> GetRecipes(string? match = null)
    {
        return match != null
            ? connection.QueryAsync<RecipeListModel>(
                """
                SELECT
                    r.recipe_id,
                    r.name,
                    r.details
                FROM recipe r
                WHERE LOWER (r.name) LIKE @match
                """,
                new { match }
            )
            : connection.QueryAsync<RecipeListModel>(
                """
                SELECT
                    r.recipe_id,
                    r.name,
                    r.details
                FROM recipe r
                """
            );
    }

    public async Task<IEnumerable<RecipeListModel>> GetRecipesWithIngredients(string? match = null)
    {
        return match != null
            ? (
                from x in await connection.QueryAsync<
                    RecipeListModel,
                    IngredientListModel,
                    RecipeListModel
                >(
                    """
                    SELECT
                        r.recipe_id,
                        r.name,
                        r.details,
                        i.ingredient_id,
                        i.name,
                        ii.added
                    FROM recipe r
                    JOIN recipe_ingredient ri ON r.recipe_id = ri.recipe_id
                    JOIN ingredient i ON ri.ingredient_id = i.ingredient_id
                    LEFT JOIN inventory_ingredient ii ON i.ingredient_id = ii.ingredient_id
                    WHERE LOWER (r.name) LIKE @match
                    ORDER BY ii.added, i.ingredient_id
                    """,
                    (recipe, ingredient) =>
                    {
                        recipe.AllIngredients.Add(ingredient);
                        return recipe;
                    },
                    new { match },
                    splitOn: "ingredient_id"
                )
                group x by x.RecipeId into g
                select g
            ).Select(g =>
            {
                var recipe = g.First();
                recipe.AllIngredients = (from r in g from i in r.AllIngredients select i).ToList();
                return recipe;
            })
            : (
                from x in await connection.QueryAsync<
                    RecipeListModel,
                    IngredientListModel,
                    RecipeListModel
                >(
                    """
                    SELECT
                        r.recipe_id,
                        r.name,
                        r.details,
                        i.ingredient_id,
                        i.name,
                        ii.added
                    FROM recipe r
                    JOIN recipe_ingredient ri ON r.recipe_id = ri.recipe_id
                    JOIN ingredient i ON ri.ingredient_id = i.ingredient_id
                    LEFT JOIN inventory_ingredient ii ON i.ingredient_id = ii.ingredient_id
                    ORDER BY ii.added, i.ingredient_id 
                    """,
                    (recipe, ingredient) =>
                    {
                        recipe.AllIngredients.Add(ingredient);
                        return recipe;
                    },
                    splitOn: "ingredient_id"
                )
                group x by x.RecipeId into g
                select g
            ).Select(g =>
            {
                var recipe = g.First();
                recipe.AllIngredients = (from r in g from i in r.AllIngredients select i).ToList();
                return recipe;
            });
    }

    public Task DeleteRecipe(long id)
    {
        return connection.ExecuteAsync(
            """
            DELETE FROM recipe
            WHERE recipe_id = @id;

            DELETE FROM recipe_ingredient
            WHERE recipe_id = @id;
            """,
            new { id }
        );
    }

    public Task<IEnumerable<IngredientListModel>> GetIngredients(string? match = null)
    {
        return match != null
            ? connection.QueryAsync<IngredientListModel>(
                """
                SELECT
                    i.ingredient_id,
                    i.name,
                    ii.added
                FROM ingredient i
                LEFT JOIN inventory_ingredient ii ON i.ingredient_id = ii.ingredient_id
                WHERE LOWER (i.name) LIKE @match
                ORDER BY ii.added, i.ingredient_id
                """,
                new { match }
            )
            : connection.QueryAsync<IngredientListModel>(
                """
                SELECT
                    i.ingredient_id,
                    i.name,
                    ii.added
                FROM ingredient i
                LEFT JOIN inventory_ingredient ii ON i.ingredient_id = ii.ingredient_id
                ORDER BY ii.added, i.ingredient_id
                """
            );
    }

    public Task<IEnumerable<IngredientListModel>> GetInventory(string? match = null)
    {
        return match != null
            ? connection.QueryAsync<IngredientListModel>(
                """
                SELECT
                    i.ingredient_id,
                    i.name,
                    ii.added
                FROM ingredient i
                JOIN inventory_ingredient ii ON i.ingredient_id = ii.ingredient_id
                WHERE LOWER (i.name) LIKE @match
                ORDER BY ii.added, i.ingredient_id
                """,
                new { match }
            )
            : connection.QueryAsync<IngredientListModel>(
                """
                SELECT
                    i.ingredient_id,
                    i.name,
                    ii.added
                FROM ingredient i
                JOIN inventory_ingredient ii ON i.ingredient_id = ii.ingredient_id
                ORDER BY ii.added, i.ingredient_id
                """
            );
    }

    public Task<IEnumerable<IngredientListModel>> GetMissing(string? match)
    {
        return match != null
            ? connection.QueryAsync<IngredientListModel>(
                """
                SELECT
                    i.ingredient_id,
                    i.name
                FROM ingredient i
                WHERE NOT EXISTS (SELECT * FROM inventory_ingredient ii WHERE ii.ingredient_id = i.ingredient_id)
                AND LOWER (i.name) LIKE @match
                ORDER BY i.ingredient_id
                """,
                new { match }
            )
            : connection.QueryAsync<IngredientListModel>(
                """
                SELECT
                    i.ingredient_id,
                    i.name
                FROM ingredient i
                WHERE NOT EXISTS (SELECT * FROM inventory_ingredient ii WHERE ii.ingredient_id = i.ingredient_id)
                ORDER BY i.ingredient_id
                """
            );
    }

    public Task CreateInventoryIngredient(long ingredientId)
    {
        return connection.ExecuteAsync(
            """
                INSERT INTO inventory_ingredient 
                    (ingredient_id)
                VALUES 
                    (@ingredientId)
                """,
            new { ingredientId }
        );
    }

    public Task DeleteInventoryIngredient(long ingredientId)
    {
        return connection.ExecuteAsync(
            """
            DELETE FROM inventory_ingredient
            WHERE ingredient_id = @ingredientId
            """,
            new { ingredientId }
        );
    }
}
