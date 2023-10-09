# :hamburger: :poultry_leg: :spaghetti: :curry: :ramen: :sushi:

SQLite database with recipes, ingredients and more

# Getting started

## Ubuntu

### Install build-essentials

```zsh
sudo apt update
sudo apt install build-essential
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
dotnet run -- inventory --add 40 47 55

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
dotnet run -- inventory --remove 40 47 55

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
