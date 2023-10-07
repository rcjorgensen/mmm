SELECT 
    r.recipe_id, 
    r.name, 
    r.details, 
    i.ingredient_id, 
    i.name, 
    EXISTS (SELECT * FROM inventory_ingredient ii WHERE ii.ingredient_id = i.ingredient_id) AS in_inventory
FROM recipe r
    JOIN recipe_ingredient ri 
        ON r.recipe_id = ri.recipe_id
    JOIN ingredient i 
        ON ri.ingredient_id = i.ingredient_id