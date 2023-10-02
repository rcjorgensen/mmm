CREATE VIEW recipe_and_source AS 
SELECT 
    r.name AS recipe, 
    rs.name AS source, 
    r.page
FROM recipe r
JOIN 
    recipe_source rs
ON
    r.recipe_source_id = rs.recipe_source_id;
    