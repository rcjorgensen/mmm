using System.Data.SQLite;
using Dapper;
using Recipizer.Cli.Models;

namespace Recipizer.Cli;

internal sealed class Repository
{
    private readonly SQLiteConnection connection;

    public Repository(SQLiteConnection connection)
    {
        this.connection = connection;
    }

    public Task<int> ExecuteRaw(string sql)
    {
        return connection.ExecuteAsync(sql);
    }

    internal Task<long> CreateRecipeSource(string name)
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

    internal Task<IEnumerable<RecipeListModel>> GetRecipes(string? match = null)
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

    internal Task DeleteRecipe(long id)
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

    internal Task<IEnumerable<IngredientListModel>> GetIngredients(string? match = null)
    {
        return match != null
            ? connection.QueryAsync<IngredientListModel>(
                """
                SELECT
                    i.ingredient_id,
                    i.name
                FROM ingredient i
                WHERE LOWER (i.name) LIKE @match
                """,
                new { match }
            )
            : connection.QueryAsync<IngredientListModel>(
                """
                SELECT
                    i.ingredient_id,
                    i.name
                FROM ingredient i
                """
            );
    }

    internal Task<IEnumerable<InventoryListModel>> GetInventory(string? match = null)
    {
        return match != null
            ? connection.QueryAsync<InventoryListModel>(
                """
                SELECT
                    i.ingredient_id,
                    i.name,
                    ii.added
                FROM ingredient i
                JOIN inventory_ingredient ii ON i.ingredient_id = ii.ingredient_id
                WHERE LOWER (i.name) LIKE @match
                """,
                new { match }
            )
            : connection.QueryAsync<InventoryListModel>(
                """
                SELECT
                    i.ingredient_id,
                    i.name,
                    ii.added
                FROM ingredient i
                JOIN inventory_ingredient ii ON i.ingredient_id = ii.ingredient_id
                """
            );
    }

    internal Task<IEnumerable<IngredientListModel>> GetMissing(string? match)
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
                """
            );
    }

    internal Task CreateInventoryIngredient(long ingredientId)
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

    internal Task DeleteInventoryIngredient(long ingredientId)
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
