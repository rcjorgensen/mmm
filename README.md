# :hamburger: :poultry_leg: :spaghetti: :curry: :ramen: :sushi:

CLI for organizing recipes, ingredients and more

# Getting started

## Ubuntu

### Requirements

#### .NET

```sh
sudo apt-get install dotnet-sdk-7.0
```

#### SQLite

```sh
sudo apt-get install sqlite3
```

### Installing

```sh
sh ./install.sh
```

After installation set the path to the install dir as an environment variable e.g. in your .zshrc or similar and add it to the PATH to be able to execute it from anywhere.

```sh
export RECIPIZER_INSTALL_DIR="$HOME/.local/bin/recipizer"
export PATH="$RECIPIZER_INSTALL_DIR:$PATH"
```

### Intialize database

```sh
rpr initialize [-f|--force]
```

### Import recipes, ingredients etc.

The schema of the JSON file is currently undocumented, but should be pretty easy to reverse engineer

```sh
rpr import ./data.json
```

### Show recipes

```sh
rpr show-recipes --name %Banana% --take 1

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

### Order by missing ingredients

```sh
rpr show-recipes --order-by-missing-ingredients --take 2

┌────┬───────────────────────────┬─────────┬───────────────────────────────────────────────┐
│ Id │ Name                      │ Details │ Ingredients                                   │
├────┼───────────────────────────┼─────────┼───────────────────────────────────────────────┤
│    │                           │         │ ┌─────┬───────────────┬────────────┬────────┐ │
│    │                           │         │ │  Id │ Name          │ Added      │ Labels │ │
│    │                           │         │ ├─────┼───────────────┼────────────┼────────┤ │
│    │                           │         │ │  40 │ Onion         │            │        │ │
│    │                           │         │ │  71 │ Red chile     │            │        │ │
│    │                           │         │ │  92 │ Pasta         │            │        │ │
│ 23 │ Spicy Cherry Tomato Pasta │ p. 89   │ │  93 │ Cherry tomato │            │        │ │
│    │                           │         │ │ 108 │ Basil         │            │        │ │
│    │                           │         │ │ 109 │ Parmesan      │            │        │ │
│    │                           │         │ │   2 │ Olive oil     │ 2023-10-17 │        │ │
│    │                           │         │ │   6 │ Garlic        │ 2023-10-17 │        │ │
│    │                           │         │ │  11 │ Salt          │ 2023-10-17 │        │ │
│    │                           │         │ └─────┴───────────────┴────────────┴────────┘ │
│    │                           │         │ ┌────┬──────────────────┬───────┬────────┐    │
│    │                           │         │ │ Id │ Name             │ Added │ Labels │    │
│    │                           │         │ ├────┼──────────────────┼───────┼────────┤    │
│    │                           │         │ │ 13 │ Banana           │       │        │    │
│    │                           │         │ │ 28 │ Ginger           │       │        │    │
│  4 │ Personal Power Smoothie   │ p. 41   │ │ 29 │ Spinach          │       │        │    │
│    │                           │         │ │ 30 │ Frozen pineapple │       │        │    │
│    │                           │         │ │ 31 │ Cashew           │       │        │    │
│    │                           │         │ │ 32 │ Coconut water    │       │        │    │
│    │                           │         │ │ 33 │ Mint             │       │        │    │
│    │                           │         │ └────┴──────────────────┴───────┴────────┘    │
└────┴───────────────────────────┴─────────┴───────────────────────────────────────────────┘
```

### Show ingredients

```sh
rpr show-ingredients --name Egg%

┌────┬──────────┬────────────┬────────┐
│ Id │ Name     │ Added      │ Labels │
├────┼──────────┼────────────┼────────┤
│ 58 │ Eggplant │            │        │
│ 12 │ Egg      │ 2023-10-17 │        │
└────┴──────────┴────────────┴────────┘
```

### Add labels to ingredients

```sh
rpr add-label perishable 58 12
```

### Filter by label

```sh
rpr show-ingredients --label perishable

┌────┬──────────┬────────────┬────────────┐
│ Id │ Name     │ Added      │ Labels     │
├────┼──────────┼────────────┼────────────┤
│ 58 │ Eggplant │            │ perishable │
│ 12 │ Egg      │ 2023-10-17 │ perishable │
└────┴──────────┴────────────┴────────────┘
```

### Remove label

```sh
rpr remove-label perishable 58 12
```

### Show missing ingredients

```sh
rpr show-missing-ingredients --take 2

┌────┬─────────┬───────┬────────┐
│ Id │ Name    │ Added │ Labels │
├────┼─────────┼───────┼────────┤
│  3 │ Avocado │       │        │
│  4 │ Tomato  │       │        │
└────┴─────────┴───────┴────────┘
```

### Add ingredients to inventory

```sh
rpr add-to-inventory 3 4

┌────┬──────────────────┬────────────┬────────┐
│ Id │ Name             │ Added      │ Labels │
├────┼──────────────────┼────────────┼────────┤
│  1 │ Bread            │ 2023-10-17 │        │
│  2 │ Olive oil        │ 2023-10-17 │        │
│  3 │ Avocado*         │ 2023-10-17 │        │
│  4 │ Tomato*          │ 2023-10-17 │        │
│  6 │ Garlic           │ 2023-10-17 │        │
│ 11 │ Salt             │ 2023-10-17 │        │
│ 12 │ Egg              │ 2023-10-17 │        │
│ 14 │ Plant-based milk │ 2023-10-17 │        │
│ 18 │ Oat              │ 2023-10-17 │        │
│ 62 │ Black pepper     │ 2023-10-17 │        │
└────┴──────────────────┴────────────┴────────┘
```

### Remove ingredients from inventory

```sh
rpr remove-from-inventory 3 4

┌────┬──────────────────┬────────────┬────────┐
│ Id │ Name             │ Added      │ Labels │
├────┼──────────────────┼────────────┼────────┤
│  1 │ Bread            │ 2023-10-17 │        │
│  2 │ Olive oil        │ 2023-10-17 │        │
│  6 │ Garlic           │ 2023-10-17 │        │
│ 11 │ Salt             │ 2023-10-17 │        │
│ 12 │ Egg              │ 2023-10-17 │        │
│ 14 │ Plant-based milk │ 2023-10-17 │        │
│ 18 │ Oat              │ 2023-10-17 │        │
│ 62 │ Black pepper     │ 2023-10-17 │        │
└────┴──────────────────┴────────────┴────────┘
```

#### List ingredients in inventory

```sh
rpr show-inventory

┌────┬──────────────────┬────────────┬────────┐
│ Id │ Name             │ Added      │ Labels │
├────┼──────────────────┼────────────┼────────┤
│  1 │ Bread            │ 2023-10-17 │        │
│  2 │ Olive oil        │ 2023-10-17 │        │
│  6 │ Garlic           │ 2023-10-17 │        │
│ 11 │ Salt             │ 2023-10-17 │        │
│ 12 │ Egg              │ 2023-10-17 │        │
│ 14 │ Plant-based milk │ 2023-10-17 │        │
│ 18 │ Oat              │ 2023-10-17 │        │
│ 62 │ Black pepper     │ 2023-10-17 │        │
└────┴──────────────────┴────────────┴────────┘
```

### Output as markdown - experimental

This is an experimental feature that will only work in some cases.

It might be helpful later on for creating reports with multiple tables.

```sh
rpr show-recipes --take 2 --markdown > OUTPUT.md
```

|  Id | Name                   | Details | Ingredients                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| --: | :--------------------- | :------ | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
|   1 | Superior Avocado Toast | p. 32   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">3</td><td align="left">Avocado</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">4</td><td align="left">Tomato</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">5</td><td align="left">Shallot</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">7</td><td align="left">Lime</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">8</td><td align="left">Dill</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">9</td><td align="left">Chives</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">10</td><td align="left">Tahini</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">1</td><td align="left">Bread</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">6</td><td align="left">Garlic</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">12</td><td align="left">Egg</td><td align="left">2023-10-17</td><td align="left"></td></tr></table> |
|   7 | Fully Loaded Omelet    | p. 48   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">3</td><td align="left">Avocado</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">49</td><td align="left">Chorizo</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">50</td><td align="left">Scallion</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">51</td><td align="left">Mushroom</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">52</td><td align="left">Gouda cheese</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">53</td><td align="left">Sour cream</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">54</td><td align="left">Cilantro</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">12</td><td align="left">Egg</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                             |

# Autocompletions for zsh - WIP

Create a file `_rpr` somewhere on the `$fpath` and paste the following

```zsh
#compdef _rpr rpr

function _rpr {
  local line

  _arguments -C \
    "--help[Show help information]" \
    "1: :(add-ingredient add-label remove-label add-recipe add-to-inventory import initialize show-ingredients show-inventory show-missing-ingredients show-recipes remove-from-inventory remove-recipe help version)" \
    "*::arg:->args"

  case $line[1] in
    add-ingredient)
      _rpr_add_ingredient
    ;;
    add-label)
      _rpr_add_label
    ;;
    remove-label)
      _rpr_remove_label
    ;;
    add-recipe)
      _rpr_add_recipe
    ;;
    add-to-inventory)
      _rpr_add_to_inventory
    ;;
    import)
      _rpr_import
    ;;
    initialize)
      _rpr_initialize
    ;;
    show-ingredients)
      _rpr_show_ingredients
    ;;
    show-inventory)
      _rpr_show_inventory
    ;;
    show-missing-ingredients)
      _rpr_show_missing_ingredients
    ;;
    show-recipes)
      _rpr_show_recipes
    ;;
    remove-from-inventory)
      _rpr_remove_from_inventory
    ;;
    remove-recipe)
      _rpr_remove_recipe
    ;;
  esac
}

function _rpr_add_ingredient {
  _arguments \
    "--help[Show help information]" \
    "1: :()"
}

function _rpr_add_label {
  _arguments \
    "--help[Show help information]" \
    "1: :()" \
    "2: :()"
}

function _rpr_remove_label {
  _arguments \
    "--help[Show help information]"
}

function _rpr_add_recipe {
  _arguments \
    "--help[Show help information]" \
    "-n[The name of the recipe]" \
    "--name[The name of the recipe]" \
    "-d[Details of the recipe]" \
    "--details[Details of the recipe]"
}

function _rpr_add_to_inventory {
  _arguments \
    "--help[Show help information]"
}

function _rpr_import {
  _arguments \
    "--help[Show help information]" \
    "1: :_files"
}

function _rpr_initialize {
  _arguments \
    "--help[Show help information]" \
    "-f[Overwrite existing database]" \
    "--force[Overwrite existing database]"
}

function _rpr_show_ingredients {
  _arguments \
    "--help[Show help information]" \
    "-n[Filter by name of ingredient, supports % as wildcard]" \
    "--name[Filter by name of ingredient, supports % as wildcard]" \
    "-l[Filter by label]" \
    "--label[Filter by label]" \
    "-t[Limit the number of ingredients shown]" \
    "--take[Limit the number of ingredients shown]"
}

function _rpr_show_inventory {
  _arguments \
    "--help[Show help information]" \
    "-n[Filter by name of ingredient, supports % as wildcard]" \
    "--name[Filter by name of ingredient, supports % as wildcard]" \
    "-l[Filter by label]" \
    "--label[Filter by label]" \
    "-t[Limit the number of ingredients shown]" \
    "--take[Limit the number of ingredients shown]"
}

function _rpr_show_missing_ingredients {
  _arguments \
    "--help[Show help information]" \
    "-n[Filter by name of ingredient, supports % as wildcard]" \
    "--name[Filter by name of ingredient, supports % as wildcard]" \
    "-l[Filter by label]" \
    "--label[Filter by label]" \
    "-t[Limit the number of ingredients shown]" \
    "--take[Limit the number of ingredients shown]"
}

function _rpr_show_recipes {
  _arguments \
    "--help[Show help information]" \
    "-n[Filter by name of ingredient, supports % as wildcard]" \
    "--name[Filter by name of ingredient, supports % as wildcard]" \
    "-t[Limit the number of ingredients shown]" \
    "--take[Limit the number of ingredients shown]" \
    "-m[Order by missing ingredients]" \
    "--order-by-missing-ingredients[Order by missing ingredients]" \
    "-a[Order by total ingredients]" \
    "--order-by-total-ingredients[Order by total ingredients]" \
    "-i[Order by ingredients in inventory]" \
    "--order-by-in-inventory[Order by ingredients in inventory]" \
    "-l[Order by label]" \
    "--order-by-label[Order by label]" \
    "--markdown[Use markdown format (experimental)]"
}

function _rpr_remove_from_inventory {
  _arguments \
    "--help[Show help information]"
}

function _rpr_remove_recipe {
  _arguments \
    "--help[Show help information]" \
    "-i[The ID of the recipe to remove] \
    "--id[The ID of the recipe to remove]
}
```

# Roadmap

- [ ] Add more recipes
- [ ] List ingredients with recipes where they are used and order by most used
- [ ] Output a report in markdown
- [ ] Add e2e testing
- [ ] Publish pre-built binary
- [x] Improve serializer multiline support for tables
- [x] Prioritize recipes with perishable ingredients in inventory
- [x] Serialize to markdown
- [x] Add installation script
