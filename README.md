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

### Add labels to ingredients

```sh
rpr add-label perishable 58


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
rpr show-recipes --markdown > OUTPUT.md
```

|  Id | Name                                | Details | Ingredients                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         |
| --: | :---------------------------------- | :------ | :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
|   1 | Superior Avocado Toast              | p. 32   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">3</td><td align="left">Avocado</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">4</td><td align="left">Tomato</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">5</td><td align="left">Shallot</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">7</td><td align="left">Lime</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">8</td><td align="left">Dill</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">9</td><td align="left">Chives</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">10</td><td align="left">Tahini</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">1</td><td align="left">Bread</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">6</td><td align="left">Garlic</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">12</td><td align="left">Egg</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      |
|   7 | Fully Loaded Omelet                 | p. 48   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">3</td><td align="left">Avocado</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">49</td><td align="left">Chorizo</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">50</td><td align="left">Scallion</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">51</td><td align="left">Mushroom</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">52</td><td align="left">Gouda cheese</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">53</td><td align="left">Sour cream</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">54</td><td align="left">Cilantro</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">12</td><td align="left">Egg</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  |
|   8 | Bacon, Avo, And Egg Breakfast Tacos | p. 51   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">3</td><td align="left">Avocado</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">7</td><td align="left">Lime</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">16</td><td align="left">Greek yogurt</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">27</td><td align="left">Butter</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">54</td><td align="left">Cilantro</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">55</td><td align="left">Bacon</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">56</td><td align="left">Tortilla</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">57</td><td align="left">Red pepper flake</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">12</td><td align="left">Egg</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
|  17 | My Ultimate Chicken Taco Salad      | p. 71   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">3</td><td align="left">Avocado</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">7</td><td align="left">Lime</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">16</td><td align="left">Greek yogurt</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">50</td><td align="left">Scallion</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">54</td><td align="left">Cilantro</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">56</td><td align="left">Tortilla</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">61</td><td align="left">Chicken</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">86</td><td align="left">Taco seasoning</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">87</td><td align="left">Baby romaine lettuce</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">88</td><td align="left">Corn</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">89</td><td align="left">Roma tomato</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">6</td><td align="left">Garlic</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">62</td><td align="left">Black pepper</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                            |
|  10 | My Triple-Decker Chicken Sandwich   | p. 56   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">4</td><td align="left">Tomato</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">8</td><td align="left">Dill</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">10</td><td align="left">Tahini</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">16</td><td align="left">Greek yogurt</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">61</td><td align="left">Chicken</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">63</td><td align="left">Lemon</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">64</td><td align="left">Arugula</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">1</td><td align="left">Bread</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">6</td><td align="left">Garlic</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">62</td><td align="left">Black pepper</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  |
|  13 | Tastiest Chicken Lunch Wrap         | p. 63   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">4</td><td align="left">Tomato</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">16</td><td align="left">Greek yogurt</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">54</td><td align="left">Cilantro</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">56</td><td align="left">Tortilla</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">60</td><td align="left">Ground tumeric</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">61</td><td align="left">Chicken</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">63</td><td align="left">Lemon</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">65</td><td align="left">Garlic powder</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">69</td><td align="left">Smoked paprika</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">72</td><td align="left">Mayonnaise</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">73</td><td align="left">Red cabbage</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">74</td><td align="left">Pickled red onion</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">6</td><td align="left">Garlic</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">62</td><td align="left">Black pepper</td><td align="left">2023-10-17</td><td align="left"></td></tr></table> |
|  22 | Leftover Chicken Salad              | p. 86   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">4</td><td align="left">Tomato</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">16</td><td align="left">Greek yogurt</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">22</td><td align="left">Dried cranberry</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">48</td><td align="left">Parsley</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">50</td><td align="left">Scallion</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">61</td><td align="left">Chicken</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">72</td><td align="left">Mayonnaise</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">77</td><td align="left">Capers</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">79</td><td align="left">Dijon mustard</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">106</td><td align="left">Burger bun</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">107</td><td align="left">Lettuce</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">6</td><td align="left">Garlic</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">62</td><td align="left">Black pepper</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                           |
|  16 | Crispy Chickpea Sweet Potato Salad  | p. 68   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">7</td><td align="left">Lime</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">10</td><td align="left">Tahini</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">16</td><td align="left">Greek yogurt</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">17</td><td align="left">Honey</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">48</td><td align="left">Parsley</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">54</td><td align="left">Cilantro</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">59</td><td align="left">Canned chickpea</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">65</td><td align="left">Garlic powder</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">68</td><td align="left">Ground cumin</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">69</td><td align="left">Smoked paprika</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">83</td><td align="left">Ground ginger</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">84</td><td align="left">Sweet potato</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">85</td><td align="left">Corn salad</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">6</td><td align="left">Garlic</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                 |
|  15 | The Gourmet Egg Salad Bagel         | p. 67   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">8</td><td align="left">Dill</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">9</td><td align="left">Chives</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">16</td><td align="left">Greek yogurt</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">48</td><td align="left">Parsley</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">72</td><td align="left">Mayonnaise</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">79</td><td align="left">Dijon mustard</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">80</td><td align="left">White vinegar</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">81</td><td align="left">Celery</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">82</td><td align="left">Sesame seed bagel</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">12</td><td align="left">Egg</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">62</td><td align="left">Black pepper</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                                                                                                                                                                     |
|   2 | Peanut Butter Banana Baked Oats     | p. 35   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">13</td><td align="left">Banana</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">15</td><td align="left">Peanut butter</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">16</td><td align="left">Greek yogurt</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">17</td><td align="left">Honey</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">19</td><td align="left">Ground cinnamon</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">20</td><td align="left">Baking powder</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">21</td><td align="left">Walnut</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">22</td><td align="left">Dried cranberry</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">12</td><td align="left">Egg</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">14</td><td align="left">Plant-based milk</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">18</td><td align="left">Oat</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                                                                                                                                                                                                                                                                           |
|   3 | Healthy(ish) Banana Pancakes        | p. 36   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">13</td><td align="left">Banana</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">16</td><td align="left">Greek yogurt</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">19</td><td align="left">Ground cinnamon</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">20</td><td align="left">Baking powder</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">23</td><td align="left">Milk</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">24</td><td align="left">Vanilla extract</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">25</td><td align="left">Flour</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">26</td><td align="left">Dark brown sugar</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">27</td><td align="left">Butter</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">12</td><td align="left">Egg</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
|   4 | Personal Power Smoothie             | p. 41   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">13</td><td align="left">Banana</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">28</td><td align="left">Ginger</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">29</td><td align="left">Spinach</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">30</td><td align="left">Frozen pineapple</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">31</td><td align="left">Cashew</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">32</td><td align="left">Coconut water</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">33</td><td align="left">Mint</td><td align="left"></td><td align="left"></td></tr></table>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            |
|   5 | Swiss Bircher Muesli                | p. 44   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">15</td><td align="left">Peanut butter</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">16</td><td align="left">Greek yogurt</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">17</td><td align="left">Honey</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">19</td><td align="left">Ground cinnamon</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">22</td><td align="left">Dried cranberry</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">34</td><td align="left">Apple juice</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">35</td><td align="left">Apple</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">36</td><td align="left">Almond flake</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">37</td><td align="left">Raspberry</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">38</td><td align="left">Blueberry</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">39</td><td align="left">Blackberry</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">14</td><td align="left">Plant-based milk</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">18</td><td align="left">Oat</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                                                                                                                                                                   |
|  11 | Healthy Buffalo Chicken Pita        | p. 59   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">16</td><td align="left">Greek yogurt</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">17</td><td align="left">Honey</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">44</td><td align="left">Paprika</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">50</td><td align="left">Scallion</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">52</td><td align="left">Gouda cheese</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">54</td><td align="left">Cilantro</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">61</td><td align="left">Chicken</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">65</td><td align="left">Garlic powder</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">66</td><td align="left">Hot sauce</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">67</td><td align="left">Pita bread</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">62</td><td align="left">Black pepper</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                                                                                                                                                                             |
|  12 | Chickpea Wrap                       | p. 60   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">16</td><td align="left">Greek yogurt</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">48</td><td align="left">Parsley</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">54</td><td align="left">Cilantro</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">56</td><td align="left">Tortilla</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">59</td><td align="left">Canned chickpea</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">63</td><td align="left">Lemon</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">65</td><td align="left">Garlic powder</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">68</td><td align="left">Ground cumin</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">69</td><td align="left">Smoked paprika</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">70</td><td align="left">Dried thyme</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">71</td><td align="left">Red chile</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">6</td><td align="left">Garlic</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">62</td><td align="left">Black pepper</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                               |
|  21 | Creamy Dreamy Butter Chicken        | p. 83   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">16</td><td align="left">Greek yogurt</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">27</td><td align="left">Butter</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">28</td><td align="left">Ginger</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">40</td><td align="left">Onion</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">46</td><td align="left">Canned tomato</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">54</td><td align="left">Cilantro</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">61</td><td align="left">Chicken</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">65</td><td align="left">Garlic powder</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">69</td><td align="left">Smoked paprika</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">104</td><td align="left">Garam masala</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">105</td><td align="left">Cream</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">6</td><td align="left">Garlic</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                                                                    |
|  19 | Fresh Summer Quinoa Salad           | p. 76   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">17</td><td align="left">Honey</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">21</td><td align="left">Walnut</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">22</td><td align="left">Dried cranberry</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">33</td><td align="left">Mint</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">35</td><td align="left">Apple</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">63</td><td align="left">Lemon</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">64</td><td align="left">Arugula</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">79</td><td align="left">Dijon mustard</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">80</td><td align="left">White vinegar</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">94</td><td align="left">Cucumber</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">96</td><td align="left">Quinoa</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         |
|  25 | Healthier "Fried" Chicken Wings     | p. 92   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">20</td><td align="left">Baking powder</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">25</td><td align="left">Flour</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">65</td><td align="left">Garlic powder</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">69</td><td align="left">Smoked paprika</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">70</td><td align="left">Dried thyme</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">83</td><td align="left">Ground ginger</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">114</td><td align="left">Chicken wing</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">115</td><td align="left">Cornstarch</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">116</td><td align="left">Ground white pepper</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">117</td><td align="left">Onion powder</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">118</td><td align="left">Ground mustard</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">119</td><td align="left">Cayenne pepper</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                                                                                                                                                    |
|  24 | My Creamy Veggie-Packed Soup        | p. 90   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">27</td><td align="left">Butter</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">40</td><td align="left">Onion</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">48</td><td align="left">Parsley</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">81</td><td align="left">Celery</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">99</td><td align="left">Frozen pea</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">105</td><td align="left">Cream</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">110</td><td align="left">Carrot</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">111</td><td align="left">Parsnip</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">112</td><td align="left">Potato</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">113</td><td align="left">Veggie stock</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">1</td><td align="left">Bread</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">6</td><td align="left">Garlic</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">62</td><td align="left">Black pepper</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                                                                              |
|  26 | Easy Chicken Congee                 | p. 95   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">28</td><td align="left">Ginger</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">50</td><td align="left">Scallion</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">54</td><td align="left">Cilantro</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">61</td><td align="left">Chicken</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">97</td><td align="left">Rice</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">100</td><td align="left">Soy sauce</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">103</td><td align="left">Sesame oil</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">115</td><td align="left">Cornstarch</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">120</td><td align="left">Chicken bouillon</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">6</td><td align="left">Garlic</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       |
|   6 | Middle Eastern-Style Shakshuka      | p. 47   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">40</td><td align="left">Onion</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">41</td><td align="left">Bell pepper</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">42</td><td align="left">Green chile</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">43</td><td align="left">Cumin seed</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">44</td><td align="left">Paprika</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">45</td><td align="left">Chili powder</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">46</td><td align="left">Canned tomato</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">47</td><td align="left">Feta cheese</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">48</td><td align="left">Parsley</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">1</td><td align="left">Bread</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">6</td><td align="left">Garlic</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">12</td><td align="left">Egg</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                                                         |
|   9 | My Special Shakshuka                | p. 52   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">40</td><td align="left">Onion</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">41</td><td align="left">Bell pepper</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">42</td><td align="left">Green chile</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">43</td><td align="left">Cumin seed</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">44</td><td align="left">Paprika</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">46</td><td align="left">Canned tomato</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">47</td><td align="left">Feta cheese</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">48</td><td align="left">Parsley</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">58</td><td align="left">Eggplant</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">59</td><td align="left">Canned chickpea</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">60</td><td align="left">Ground tumeric</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">6</td><td align="left">Garlic</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">12</td><td align="left">Egg</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                          |
|  20 | The Leftover Fried Rice             | p. 80   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">40</td><td align="left">Onion</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">41</td><td align="left">Bell pepper</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">50</td><td align="left">Scallion</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">61</td><td align="left">Chicken</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">97</td><td align="left">Rice</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">98</td><td align="left">MSG</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">99</td><td align="left">Frozen pea</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">100</td><td align="left">Soy sauce</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">101</td><td align="left">Oyster sauce</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">102</td><td align="left">Rice vinegar</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">103</td><td align="left">Sesame oil</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">6</td><td align="left">Garlic</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">12</td><td align="left">Egg</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                                                                                 |
|  23 | Spicy Cherry Tomato Pasta           | p. 89   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">40</td><td align="left">Onion</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">71</td><td align="left">Red chile</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">92</td><td align="left">Pasta</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">93</td><td align="left">Cherry tomato</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">108</td><td align="left">Basil</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">109</td><td align="left">Parmesan</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">6</td><td align="left">Garlic</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                |
|  18 | Mediterranean Pasta Salad           | p. 75   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">47</td><td align="left">Feta cheese</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">48</td><td align="left">Parsley</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">63</td><td align="left">Lemon</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">76</td><td align="left">Red onion</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">77</td><td align="left">Capers</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">90</td><td align="left">Red wine vinegar</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">91</td><td align="left">Thyme</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">92</td><td align="left">Pasta</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">93</td><td align="left">Cherry tomato</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">94</td><td align="left">Cucumber</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">95</td><td align="left">Olives</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">62</td><td align="left">Black pepper</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                                                                         |
|  14 | Tuna Melt Quesadilla                | p. 64   | <table><tr><th align="right">Id</th><th align="left">Name</th><th align="left">Added</th><th align="left">Labels</th></tr><tr><td align="right">48</td><td align="left">Parsley</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">56</td><td align="left">Tortilla</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">72</td><td align="left">Mayonnaise</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">75</td><td align="left">Water-packed tuna</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">76</td><td align="left">Red onion</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">77</td><td align="left">Capers</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">78</td><td align="left">Cheddar cheese</td><td align="left"></td><td align="left"></td></tr><tr><td align="right">2</td><td align="left">Olive oil</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">6</td><td align="left">Garlic</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">11</td><td align="left">Salt</td><td align="left">2023-10-17</td><td align="left"></td></tr><tr><td align="right">62</td><td align="left">Black pepper</td><td align="left">2023-10-17</td><td align="left"></td></tr></table>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          |

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
