# :hamburger: :poultry_leg: :spaghetti: :curry: :ramen: :sushi:

.NET CLI on top of an SQLite database with recipes, ingredients and more

# Getting started

## Ubuntu

### Install dotnet-sdk-7.0

```zsh
sudo apt-get install dotnet-sdk-7.0
```

### Install sqlite3

```zsh
sudo apt-get install sqlite3
```

### Intialize database

```zsh
cd src/Recipizer.Cli
dotnet run -- init [--force]
```

### List recipes

```zsh
cd src/Recipizer.Cli
dotnet run -- recipes --list --match %Banana%

┌────┬─────────────────────────────────┬─────────┐
│ Id │ Name                            │ Details │
├────┼─────────────────────────────────┼─────────┤
│  2 │ Peanut Butter Banana Baked Oats │ p. 35   │
│  3 │ Healthy(ish) Banana Pancakes    │ p. 36   │
└────┴─────────────────────────────────┴─────────┘
```

#### List recipes with ingredients

```zsh
cd src/Recipizer.Cli
dotnet run -- recipes --list --with-ingredients --by-fewest-missing --take 2

┌────┬─────────────────────────────────┬─────────┬────────────────────────────────────────┐
│ Id │ Name                            │ Details │ Ingredients                            │
├────┼─────────────────────────────────┼─────────┼────────────────────────────────────────┤
│    │                                 │         │ ┌────┬──────────────────┬────────────┐ │
│    │                                 │         │ │ Id │ Name             │ Added      │ │
│    │                                 │         │ ├────┼──────────────────┼────────────┤ │
│    │                                 │         │ │ 23 │ Milk             │            │ │
│    │                                 │         │ │ 24 │ Vanilla extract  │            │ │
│    │                                 │         │ │ 11 │ Salt             │ 2023-10-08 │ │
│    │                                 │         │ │ 12 │ Egg              │ 2023-10-08 │ │
│    │                                 │         │ │ 13 │ Banana           │ 2023-10-08 │ │
│  3 │ Healthy(ish) Banana Pancakes    │ p. 36   │ │ 16 │ Greek yogurt     │ 2023-10-08 │ │
│    │                                 │         │ │ 19 │ Ground cinnamon  │ 2023-10-08 │ │
│    │                                 │         │ │ 20 │ Baking powder    │ 2023-10-08 │ │
│    │                                 │         │ │ 25 │ Flour            │ 2023-10-08 │ │
│    │                                 │         │ │ 26 │ Dark brown sugar │ 2023-10-08 │ │
│    │                                 │         │ │ 27 │ Butter           │ 2023-10-08 │ │
│    │                                 │         │ └────┴──────────────────┴────────────┘ │
│    │                                 │         │ ┌────┬──────────────────┬────────────┐ │
│    │                                 │         │ │ Id │ Name             │ Added      │ │
│    │                                 │         │ ├────┼──────────────────┼────────────┤ │
│    │                                 │         │ │ 21 │ Walnut           │            │ │
│    │                                 │         │ │ 22 │ Dried cranberry  │            │ │
│    │                                 │         │ │ 11 │ Salt             │ 2023-10-08 │ │
│    │                                 │         │ │ 12 │ Egg              │ 2023-10-08 │ │
│    │                                 │         │ │ 13 │ Banana           │ 2023-10-08 │ │
│    │                                 │         │ │ 14 │ Plant-based milk │ 2023-10-08 │ │
│  2 │ Peanut Butter Banana Baked Oats │ p. 35   │ │ 15 │ Peanut butter    │ 2023-10-08 │ │
│    │                                 │         │ │ 16 │ Greek yogurt     │ 2023-10-08 │ │
│    │                                 │         │ │ 17 │ Honey            │ 2023-10-08 │ │
│    │                                 │         │ │ 18 │ Oat              │ 2023-10-08 │ │
│    │                                 │         │ │ 19 │ Ground cinnamon  │ 2023-10-08 │ │
│    │                                 │         │ │ 20 │ Baking powder    │ 2023-10-08 │ │
│    │                                 │         │ └────┴──────────────────┴────────────┘ │
└────┴─────────────────────────────────┴─────────┴────────────────────────────────────────┘
```

#### List recipes with missing ingredients

```zsh
cd src/Recipizer.Cli
dotnet run -- recipes --list --match %Avocado% --with-missing-ingredients

┌────┬────────────────────────┬─────────┬─────────────────────┐
│ Id │ Name                   │ Details │ Ingredients missing │
├────┼────────────────────────┼─────────┼─────────────────────┤
│    │                        │         │ ┌────┬─────────┐    │
│    │                        │         │ │ Id │ Name    │    │
│    │                        │         │ ├────┼─────────┤    │
│    │                        │         │ │  4 │ Tomato  │    │
│    │                        │         │ │  5 │ Shallot │    │
│  1 │ Superior Avocado Toast │ p. 32   │ │  7 │ Lime    │    │
│    │                        │         │ │  8 │ Dill    │    │
│    │                        │         │ │  9 │ Chives  │    │
│    │                        │         │ └────┴─────────┘    │
└────┴────────────────────────┴─────────┴─────────────────────┘
```

#### List recipes with ingredients in inventory

```zsh
cd src/Recipizer.Cli
dotnet run -- recipes --list --take 2 --with-inventory-ingredients

┌────┬───────────────────────────────────┬─────────┬────────────────────────────────────┐
│ Id │ Name                              │ Details │ Ingredients in inventory           │
├────┼───────────────────────────────────┼─────────┼────────────────────────────────────┤
│    │                                   │         │ ┌────┬───────────┬────────────┐    │
│    │                                   │         │ │ Id │ Name      │ Added      │    │
│    │                                   │         │ ├────┼───────────┼────────────┤    │
│    │                                   │         │ │  1 │ Bread     │ 2023-10-08 │    │
│    │                                   │         │ │  2 │ Olive oil │ 2023-10-08 │    │
│    │                                   │         │ │  3 │ Avocado   │ 2023-10-08 │    │
│  1 │ Superior Avocado Toast            │ p. 32   │ │  6 │ Garlic    │ 2023-10-08 │    │
│    │                                   │         │ │ 10 │ Tahini    │ 2023-10-08 │    │
│    │                                   │         │ │ 11 │ Salt      │ 2023-10-08 │    │
│    │                                   │         │ │ 12 │ Egg       │ 2023-10-08 │    │
│    │                                   │         │ └────┴───────────┴────────────┘    │
│    │                                   │         │ ┌────┬──────────────┬────────────┐ │
│    │                                   │         │ │ Id │ Name         │ Added      │ │
│    │                                   │         │ ├────┼──────────────┼────────────┤ │
│    │                                   │         │ │  1 │ Bread        │ 2023-10-08 │ │
│    │                                   │         │ │  2 │ Olive oil    │ 2023-10-08 │ │
│    │                                   │         │ │  6 │ Garlic       │ 2023-10-08 │ │
│ 10 │ My Triple-Decker Chicken Sandwich │ p. 56   │ │ 10 │ Tahini       │ 2023-10-08 │ │
│    │                                   │         │ │ 11 │ Salt         │ 2023-10-08 │ │
│    │                                   │         │ │ 16 │ Greek yogurt │ 2023-10-08 │ │
│    │                                   │         │ │ 63 │ Black pepper │ 2023-10-08 │ │
│    │                                   │         │ └────┴──────────────┴────────────┘ │
└────┴───────────────────────────────────┴─────────┴────────────────────────────────────┘
```

### List ingredients

```zsh
cd src/Recipizer.Cli
dotnet run -- ingredients --list --match %Egg%

┌────┬──────────┐
│ Id │ Name     │
├────┼──────────┤
│ 12 │ Egg      │
│ 59 │ Eggplant │
└────┴──────────┘
```

### Manage inventory

#### List ingredients missing from inventory

```zsh
cd src/Recipizer.Cli
dotnet run -- ingredients --missing

┌────┬───────────────────┐
│ Id │ Name              │
├────┼───────────────────┤
│  4 │ Tomato            │
│  5 │ Shallot           │
│  7 │ Lime              │
│  8 │ Dill              │
│ 67 │ Hot sauce         │
│ 68 │ Pita bread        │
└────┴───────────────────┘
```

#### Add ingredients to inventory

```zsh
cd src/Recipizer.Cli
dotnet run -- ingredients --add --to-inventory 40 47 55

┌────┬──────────────────┬────────────┐
│ Id │ Name             │ Added      │
├────┼──────────────────┼────────────┤
│ 34 │ Apple juice      │ 2023-10-08 │
│ 38 │ Blueberry        │ 2023-10-08 │
│ 39 │ Blackberry       │ 2023-10-08 │
│ 40 │ Onion*           │ 2023-10-09 │
│ 44 │ Paprika          │ 2023-10-08 │
│ 47 │ Feta cheese*     │ 2023-10-09 │
│ 55 │ Bacon*           │ 2023-10-09 │
│ 63 │ Black pepper     │ 2023-10-08 │
└────┴──────────────────┴────────────┘
```

#### Remove ingredients from inventory

```zsh
cd src/Recipizer.Cli
dotnet run -- ingredients --remove --from-inventory 40 47 55

┌────┬──────────────────┬────────────┐
│ Id │ Name             │ Added      │
├────┼──────────────────┼────────────┤
│ 34 │ Apple juice      │ 2023-10-08 │
│ 38 │ Blueberry        │ 2023-10-08 │
│ 39 │ Blackberry       │ 2023-10-08 │
│ 44 │ Paprika          │ 2023-10-08 │
│ 63 │ Black pepper     │ 2023-10-08 │
└────┴──────────────────┴────────────┘
```

#### List ingredients in inventory

```zsh
cd src/Recipizer.Cli
dotnet run -- ingredients --inventory

┌────┬──────────────────┬────────────┐
│ Id │ Name             │ Added      │
├────┼──────────────────┼────────────┤
│ 34 │ Apple juice      │ 2023-10-08 │
│ 38 │ Blueberry        │ 2023-10-08 │
│ 39 │ Blackberry       │ 2023-10-08 │
│ 44 │ Paprika          │ 2023-10-08 │
│ 63 │ Black pepper     │ 2023-10-08 │
└────┴──────────────────┴────────────┘
```

# Roadmap

- Add more recipes
- Prioritize recipes with perishable ingredients in inventory
- List ingredients with recipes where they are used and order by most used
