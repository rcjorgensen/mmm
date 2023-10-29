# r7r :hamburger: :poultry_leg: :spaghetti: :curry: :ramen: :sushi:

CLI for organizing recipes, ingredients and more

# Getting started

## Linux

### Requirements

- .NET 7 or above

### Installing

```sh
sh ./publish.sh
```

After installation set the path to the install dir as an environment variable and add it to the PATH to be able to execute it from anywhere. This can be added to your `.bashrc`, `.zshrc` or similar.

```sh
export R7R_DB_PATH="$HOME/.repizer-database.db"
```

Optionally put the publish directory on your path.

### Intialize database

```sh
r7r init [-f|--force]
```

### Import recipes, ingredients etc.

The schema of the JSON file is currently undocumented, but should be pretty easy to reverse engineer

```sh
r7r import ./data.json
```

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

### Order by missing ingredients

```sh
r7r show-recipes --order-by-missing-ingredients --take 2

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
r7r show-ingredients --name Egg%

┌────┬──────────┬────────────┬────────┐
│ Id │ Name     │ Added      │ Labels │
├────┼──────────┼────────────┼────────┤
│ 58 │ Eggplant │            │        │
│ 12 │ Egg      │ 2023-10-17 │        │
└────┴──────────┴────────────┴────────┘
```

### Add labels to ingredients

```sh
r7r add-label perishable 58 12
```

### Filter by label

```sh
r7r show-ingredients --label perishable

┌────┬──────────┬────────────┬────────────┐
│ Id │ Name     │ Added      │ Labels     │
├────┼──────────┼────────────┼────────────┤
│ 58 │ Eggplant │            │ perishable │
│ 12 │ Egg      │ 2023-10-17 │ perishable │
└────┴──────────┴────────────┴────────────┘
```

### Remove label

```sh
r7r remove-label perishable 58 12
```

### Show missing ingredients

```sh
r7r show-missing-ingredients --take 2

┌────┬─────────┬───────┬────────┐
│ Id │ Name    │ Added │ Labels │
├────┼─────────┼───────┼────────┤
│  3 │ Avocado │       │        │
│  4 │ Tomato  │       │        │
└────┴─────────┴───────┴────────┘
```

### Add ingredients to inventory

```sh
r7r add-to-inventory 3 4

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
r7r remove-from-inventory 3 4

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
r7r show-inventory

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
r7r show-recipes --take 2 --markdown > OUTPUT.md
```

|  Id | Name                   | Details | Ingredients                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| --: | :--------------------- | :------ | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
|   1 | Superior Avocado Toast | p. 32   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">3</td><td align="left">Avocado</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">4</td><td align="left">Tomato</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">5</td><td align="left">Shallot</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">7</td><td align="left">Lime</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">8</td><td align="left">Dill</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">9</td><td align="left">Chives</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">10</td><td align="left">Tahini</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">1</td><td align="left">Bread</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">6</td><td align="left">Garlic</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">12</td><td align="left">Egg</td><td align="left">2023-10-17</td><td align="left"></td></tr></table> |
|   7 | Fully Loaded Omelet    | p. 48   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">3</td><td align="left">Avocado</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">49</td><td align="left">Chorizo</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">50</td><td align="left">Scallion</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">51</td><td align="left">Mushroom</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">52</td><td align="left">Gouda cheese</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">53</td><td align="left">Sour cream</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">54</td><td align="left">Cilantro</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">12</td><td align="left">Egg</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                             |

# ZSH completions

Copy [\_r7r](src/r7r/completions/_r7r) to somewhere on your `$fpath` and restart your terminal.

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
