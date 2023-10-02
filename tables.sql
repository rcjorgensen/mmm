CREATE TABLE recipe_source (
    recipe_source_id INTEGER PRIMARY KEY,
    name TEXT NOT NULL UNIQUE
);

CREATE TABLE recipe (
    recipe_id INTEGER PRIMARY KEY,
    name TEXT NOT NULL UNIQUE,
    recipe_source_id INTEGER,
    page INTEGER,
    FOREIGN KEY (recipe_source_id)
        REFERENCES recipe_source (recipe_source_id)
);

CREATE TABLE ingredient (
    ingredient_id INTEGER PRIMARY KEY,
    singular_name TEXT NOT NULL UNIQUE,
    plural_name TEXT
);

CREATE TABLE inventory_ingredient (
    ingredient_id INTEGER NOT NULL UNIQUE,
    added TEXT DEFAULT CURRENT_DATE,
    quantity INTEGER,
    unit TEXT,
    FOREIGN KEY (ingredient_id)
        REFERENCES ingredient (ingredient_id)
);

CREATE TABLE recipe_ingredient (
    recipe_id INTEGER NOT NULL,
    ingredient_id INTEGER NOT NULL,
    quantity INTEGER,
    unit TEXT,
    details TEXT,
    PRIMARY KEY (recipe_id, ingredient_id),
    FOREIGN KEY (recipe_id)
        REFERENCES recipe (recipe_id),
    FOREIGN KEY (ingredient_id)
        REFERENCES ingredient (ingredient_id)
);