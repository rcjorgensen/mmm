using System.Data.SQLite;

using Dapper;

using Recipizer.Core.Models;

namespace Recipizer.Core;

public sealed class Repository : IRepository
{
    private readonly SQLiteConnection _connection;

    public Repository(SQLiteConnection connection)
    {
        _connection = connection;
    }

    public Task<int> InitializeSchema()
    {
        return _connection.ExecuteAsync(
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

    public async Task<long> CreateRecipeSource(string name)
    {
        const string select = """
            SELECT
                recipe_source_id
            FROM
                recipe_source
            WHERE name = @name
            """;

        const string insert = """
            INSERT INTO recipe_source
                (name)
            VALUES
                (@name)
            RETURNING recipe_source_id
            """;

        return await _connection.QuerySingleOrDefaultAsync<long?>(select, new { name })
            ?? await _connection.QuerySingleAsync<long>(insert, new { name, });
    }

    public async Task<long> CreateLabel(string label)
    {
        const string select = """
            SELECT 
                label_id
            FROM label
            WHERE label = @label
            """;

        const string insert = """
            INSERT INTO label
                (label)
            VALUES
                (@label)
            RETURNING label_id
            """;

        return await _connection.QuerySingleOrDefaultAsync<long?>(select, new { label })
            ?? await _connection.QuerySingleAsync<long>(insert, new { label, });
    }

    public async Task<long> CreateRecipe(
        string name,
        string? details = null,
        long? recipeSourceId = null
    )
    {
        const string select = """
            SELECT
                recipe_id
            FROM recipe
            where name = @name
            """;

        const string insert = """
            INSERT INTO recipe 
                (name, recipe_source_id, details)
            VALUES 
                (@name, @recipeSourceId, @details)
            RETURNING recipe_id
            """;

        return await _connection.QuerySingleOrDefaultAsync<long?>(select, new { name })
            ?? await _connection.QuerySingleAsync<long>(
                insert,
                new
                {
                    name,
                    recipeSourceId,
                    details
                }
            );
    }

    public async Task<long> CreateIngredient(string name)
    {
        const string select = """
            SELECT 
                ingredient_id
            FROM ingredient 
            WHERE name = @name
            """;

        const string insert = """
            INSERT INTO 
                ingredient (name) 
            VALUES
                (@name)
            RETURNING ingredient_id
            """;

        return await _connection.QuerySingleOrDefaultAsync<long?>(select, new { name })
            ?? await _connection.QuerySingleAsync<long>(insert, new { name });
    }

    public async Task<long> AddIngredientToRecipe(long recipeId, long ingredientId)
    {
        const string select = """
            SELECT
                recipe_id
            FROM recipe_ingredient
            WHERE
                recipe_id = @recipeId
            AND ingredient_id = @ingredientId
            """;

        const string insert = """
            INSERT INTO 
                recipe_ingredient (recipe_id, ingredient_id) 
            VALUES 
                (@recipeId, @ingredientId)
            RETURNING recipe_id
            """;

        return await _connection.QuerySingleOrDefaultAsync<long?>(
                select,
                new { recipeId, ingredientId }
            ) ?? await _connection.QuerySingleAsync<long>(insert, new { recipeId, ingredientId });
    }

    public Task<IEnumerable<RecipeListModel>> GetRecipes(string? match = null)
    {
        return match != null
            ? _connection.QueryAsync<RecipeListModel>(
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
            : _connection.QueryAsync<RecipeListModel>(
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
        return (
            from x in await _connection.QueryAsync<
                RecipeListModel,
                IngredientListModel,
                string?,
                RecipeListModel
            >(
                """
                SELECT
                    r.recipe_id,
                    r.name,
                    r.details,
                    i.ingredient_id,
                    i.name,
                    ii.added,
                    l.label
                FROM recipe r
                JOIN recipe_ingredient ri ON r.recipe_id = ri.recipe_id
                JOIN ingredient i ON ri.ingredient_id = i.ingredient_id
                LEFT JOIN inventory_ingredient ii ON i.ingredient_id = ii.ingredient_id
                LEFT JOIN ingredient_label il ON i.ingredient_id = il.ingredient_id
                LEFT JOIN label l ON il.label_id = l.label_id
                WHERE (@match IS NULL OR LOWER (r.name) LIKE @match)
                ORDER BY ii.added, i.ingredient_id
                """,
                (recipe, ingredient, label) =>
                {
                    if (!string.IsNullOrWhiteSpace(label))
                    {
                        ingredient.Labels.Add(label);
                    }

                    recipe.AllIngredients.Add(ingredient);
                    return recipe;
                },
                new { match },
                splitOn: "ingredient_id, label"
            )
            group x by x.RecipeId into g
            select g
        ).Select(g =>
        {
            var ingredientGroups =
                from r in g
                from i in r.AllIngredients
                group i by i.IngredientId into h
                select h;

            var ingredients = new List<IngredientListModel>();

            foreach (var ingredientGroup in ingredientGroups)
            {
                var ingredient = ingredientGroup.First();
                ingredient.Labels = (
                    from i in ingredientGroup
                    from l in i.Labels
                    select l
                ).ToList();
                ingredients.Add(ingredient);
            }

            var recipe = g.First();
            recipe.AllIngredients = ingredients;
            return recipe;
        });
    }

    public Task DeleteRecipe(long id)
    {
        return _connection.ExecuteAsync(
            """
            DELETE FROM recipe
            WHERE recipe_id = @id;

            DELETE FROM recipe_ingredient
            WHERE recipe_id = @id;
            """,
            new { id }
        );
    }

    public async Task<IEnumerable<IngredientListModel>> GetIngredients(
        string? name = null,
        string? label = null
    )
    {
        return (
            from x in await _connection.QueryAsync<
                IngredientListModel,
                string?,
                RecipeListModel,
                IngredientListModel
            >(
                """
                SELECT
                    i.ingredient_id,
                    i.name,
                    ii.added,
                    l.label,
                    r.recipe_id,
                    r.name,
                    r.details
                FROM ingredient i
                JOIN recipe_ingredient ri ON i.ingredient_id = ri.ingredient_id
                JOIN recipe r ON ri.recipe_id = r.recipe_id
                LEFT JOIN inventory_ingredient ii ON i.ingredient_id = ii.ingredient_id
                LEFT JOIN ingredient_label il ON i.ingredient_id = il.ingredient_id
                LEFT JOIN label l ON il.label_id = l.label_id
                WHERE 
                    (@name IS NULL OR LOWER (i.name) LIKE @name)
                AND
                    (@label IS NULL OR EXISTS (SELECT * FROM ingredient_label il JOIN label l ON il.label_id = l.label_id WHERE il.ingredient_id = i.ingredient_id AND l.label = @label))
                ORDER BY ii.added, i.ingredient_id
                """,
                (ingredient, label, recipe) =>
                {
                    if (!string.IsNullOrWhiteSpace(label))
                    {
                        ingredient.Labels.Add(label);
                    }

                    ingredient.Recipes.Add(recipe);
                    return ingredient;
                },
                new { name, label },
                splitOn: "label, recipe_id"
            )
            group x by x.IngredientId into g
            select g
        ).Select(g =>
        {
            var ingredient = g.First();

            ingredient.Labels = (from x in g from l in x.Labels select l).Distinct().ToList();
            ingredient.Recipes = (from x in g from r in x.Recipes select r)
                .DistinctBy(x => x.RecipeId)
                .ToList();
            return ingredient;
        });
    }

    public async Task<IEnumerable<IngredientListModel>> GetInventory(
        string? name = null,
        string? label = null
    )
    {
        return (
            from x in await _connection.QueryAsync<
                IngredientListModel,
                string?,
                RecipeListModel,
                IngredientListModel
            >(
                """
                SELECT
                    i.ingredient_id,
                    i.name,
                    ii.added,
                    l.label,
                    r.recipe_id,
                    r.name,
                    r.details
                FROM ingredient i
                JOIN recipe_ingredient ri ON i.ingredient_id = ri.ingredient_id
                JOIN recipe r ON ri.recipe_id = r.recipe_id
                JOIN inventory_ingredient ii ON i.ingredient_id = ii.ingredient_id
                LEFT JOIN ingredient_label il ON i.ingredient_id = il.ingredient_id
                LEFT JOIN label l ON il.label_id = l.label_id
                WHERE
                    (@name IS NULL OR LOWER (i.name) LIKE @name) 
                AND
                    (@label IS NULL OR EXISTS (SELECT * FROM ingredient_label il JOIN label l ON il.label_id = l.label_id WHERE il.ingredient_id = i.ingredient_id AND l.label = @label))
                ORDER BY ii.added, i.ingredient_id
                """,
                (ingredient, label, recipe) =>
                {
                    if (!string.IsNullOrWhiteSpace(label))
                    {
                        ingredient.Labels.Add(label);
                    }

                    ingredient.Recipes.Add(recipe);

                    return ingredient;
                },
                new { name, label },
                splitOn: "label, recipe_id"
            )
            group x by x.IngredientId into g
            select g
        ).Select(g =>
        {
            var ingredient = g.First();
            ingredient.Labels = (from x in g from l in x.Labels select l).Distinct().ToList();
            ingredient.Recipes = (from x in g from r in x.Recipes select r)
                .DistinctBy(x => x.RecipeId)
                .ToList();
            return ingredient;
        });
    }

    public async Task<IEnumerable<IngredientListModel>> GetMissing(
        string? name = null,
        string? label = null
    )
    {
        return (
            from x in await _connection.QueryAsync<
                IngredientListModel,
                string?,
                RecipeListModel,
                IngredientListModel
            >(
                """
                SELECT
                    i.ingredient_id,
                    i.name,
                    l.label,
                    r.recipe_id,
                    r.name,
                    r.details
                FROM ingredient i
                JOIN recipe_ingredient ri ON i.ingredient_id = ri.ingredient_id
                JOIN recipe r ON ri.recipe_id = r.recipe_id
                LEFT JOIN ingredient_label il ON i.ingredient_id = il.ingredient_id
                LEFT JOIN label l ON il.label_id = l.label_id
                WHERE 
                    NOT EXISTS (SELECT * FROM inventory_ingredient ii WHERE ii.ingredient_id = i.ingredient_id)
                AND
                    (@name IS NULL OR LOWER (i.name) LIKE @name)
                AND
                    (@label IS NULL OR EXISTS (SELECT * FROM ingredient_label il JOIN label l ON il.label_id = l.label_id WHERE il.ingredient_id = i.ingredient_id AND l.label = @label))
                ORDER BY i.ingredient_id
                """,
                (ingredient, label, recipe) =>
                {
                    if (!string.IsNullOrWhiteSpace(label))
                    {
                        ingredient.Labels.Add(label);
                    }

                    ingredient.Recipes.Add(recipe);

                    return ingredient;
                },
                new { name, label },
                splitOn: "label, recipe_id"
            )
            group x by x.IngredientId into g
            select g
        ).Select(g =>
        {
            var ingredient = g.First();
            ingredient.Labels = (from x in g from l in x.Labels select l).Distinct().ToList();
            ingredient.Recipes = (from x in g from r in x.Recipes select r)
                .DistinctBy(x => x.RecipeId)
                .ToList();
            return ingredient;
        });
    }

    public async Task<long> CreateInventoryIngredient(long ingredientId)
    {
        const string select = """
            SELECT 
                ingredient_id
            FROM
                inventory_ingredient
            WHERE
                ingredient_id = @ingredientId
            """;

        const string insert = """
            INSERT INTO inventory_ingredient
                (ingredient_id)
            VALUES
                (@ingredientId)
            RETURNING ingredient_id
            """;

        return await _connection.QuerySingleOrDefaultAsync<long?>(select, new { ingredientId })
            ?? await _connection.QuerySingleAsync<long>(insert, new { ingredientId });
    }

    public Task DeleteInventoryIngredient(long ingredientId)
    {
        return _connection.ExecuteAsync(
            """
            DELETE FROM inventory_ingredient
            WHERE ingredient_id = @ingredientId
            """,
            new { ingredientId }
        );
    }

    public async Task<long> AddLabelToIngredient(long ingredientId, long labelId)
    {
        const string select = """
            SELECT
                ingredient_id
            FROM ingredient_label
            WHERE
                ingredient_id = @ingredientId
            AND 
                label_id = @labelId
            """;

        const string insert = """
            INSERT INTO 
                ingredient_label (ingredient_id, label_id) 
            VALUES 
                (@ingredientId, @labelId)
            RETURNING ingredient_id
            """;

        return await _connection.QuerySingleOrDefaultAsync<long?>(
                select,
                new { ingredientId, labelId }
            ) ?? await _connection.QuerySingleAsync<long>(insert, new { ingredientId, labelId });
    }

    public async Task RemoveLabelFromIngredient(long ingredientId, string label)
    {
        var labelId = await _connection.QuerySingleOrDefaultAsync<long?>(
            """
            SELECT 
                label_id
            FROM
                label
            WHERE 
                label = @label
            """,
            new { label }
        );

        if (labelId != null)
        {
            await _connection.ExecuteAsync(
                """
                DELETE FROM
                    ingredient_label
                WHERE 
                    ingredient_id = @ingredientId
                AND 
                    label_id = @labelId
                """,
                new { ingredientId, labelId }
            );
        }
    }

    public Task<IEnumerable<long>> GetIngredientIdsForLabel(string label)
    {
        return _connection.QueryAsync<long>(
            """
            SELECT
                il.ingredient_id
            FROM 
                ingredient_label il
            JOIN 
                label l ON il.label_id = l.label_id
            WHERE 
                l.label = @label
            """,
            new { label }
        );
    }

    public Task DeleteLabel(string label)
    {
        return _connection.QueryAsync<long>(
            """
            DELETE FROM 
                label
            WHERE 
                label = @label
            """,
            new { label }
        );
    }
}
