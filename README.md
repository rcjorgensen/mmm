# :hamburger: :poultry_leg: :spaghetti: :curry: :ramen: :sushi:

# Recipizer

Organize recipes, ingredients and more

# Getting started

## Linux

### Requirements

- .NET 8 or above

### Installing

```sh
sh ./publish.sh
```

After installation set the path to the install dir as an environment variable and add it to the PATH to be able to execute it from anywhere. This can be added to your `.bashrc`, `.zshrc` or similar.

```sh
export R7R_DB_PATH="$HOME/.repizer-database.db"
```

Optionally put the publish directory on your path.

# CLI

```sh
r7r init [-f|--force]
```

## Import recipes, ingredients etc.

The schema of the JSON file is currently undocumented, but should be pretty easy to reverse engineer

```sh
r7r import ./data.json
```

## Examples

### Show recipes

```sh
r7r show-recipes --name %Banana% --take 1

┌────┬─────────────────────────────────┬─────────┬─────────────────────────────────────────────────┐
│ Id │ Name                            │ Details │ Ingredients                                     │
├────┼─────────────────────────────────┼─────────┼─────────────────────────────────────────────────┤
│    │                                 │         │ ┌────┬──────────────────┬────────────┬────────┐ │
│    │                                 │         │ │ Id │ Name             │ Added      │ Labels │ │
│    │                                 │         │ ├────┼──────────────────┼────────────┼────────┤ │
│    │                                 │         │ │ 13 │ Banana           │            │        │ │
│    │                                 │         │ │ 15 │ Peanut butter    │            │        │ │
│    │                                 │         │ │ 16 │ Greek yogurt     │            │        │ │
│    │                                 │         │ │ 17 │ Honey            │            │        │ │
│  2 │ Peanut Butter Banana Baked Oats │ p. 35   │ │ 19 │ Ground cinnamon  │            │        │ │
│    │                                 │         │ │ 20 │ Baking powder    │            │        │ │
│    │                                 │         │ │ 21 │ Walnut           │            │        │ │
│    │                                 │         │ │ 22 │ Dried cranberry  │            │        │ │
│    │                                 │         │ │ 11 │ Salt             │ 2023-10-17 │        │ │
│    │                                 │         │ │ 12 │ Egg              │ 2023-10-17 │        │ │
│    │                                 │         │ │ 14 │ Plant-based milk │ 2023-10-17 │        │ │
│    │                                 │         │ │ 18 │ Oat              │ 2023-10-17 │        │ │
│    │                                 │         │ └────┴──────────────────┴────────────┴────────┘ │
└────┴─────────────────────────────────┴─────────┴─────────────────────────────────────────────────┘
```

# WebApp

```shell
sh ./listen.sh
```

# ZSH completions

Copy [\_r7r](src/r7r/completions/_r7r) to somewhere on your `$fpath` and restart your terminal.

# Roadmap

- [ ] Add more recipes
- [x] Add web app
- [ ] List ingredients with recipes where they are used and order by most used
- [ ] Output a report in markdown
- [ ] Add e2e testing
- [ ] Publish pre-built binary
- [x] Improve serializer multiline support for tables
- [x] Prioritize recipes with perishable ingredients in inventory
- [x] Serialize to markdown
- [x] Add installation script
