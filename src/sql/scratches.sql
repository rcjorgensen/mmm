select i.name as ingredient from recipe r
join recipe_ingredient ri on r.recipe_id = ri.recipe_id
join ingredient i on ri.ingredient_id = i.ingredient_id
where r.name = 'Superior Avocado Toast';