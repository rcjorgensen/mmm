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
