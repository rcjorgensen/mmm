# :hamburger: :poultry_leg: :spaghetti: :curry: :ramen: :sushi:

SQLite database with recipes, ingredients and more

# Getting started

## Ubuntu

### Install build-essentials

```zsh
sudo apt update
sudo apt install build-essential
```

### Install SQLite from source

```zsh
mkdir temp
cd temp
wget https://www.sqlite.org/2023/sqlite-autoconf-3430100.tar.gz
tar xvfz sqlite-autoconf-3430100.tar.gz
cd sqlite-autoconf-3430100
./configure
sudo make
sudo make install
cd ../..
sudo rm -r temp
```

### Intialize database

```zsh
cd src/Recipizer.Cli
dotnet run init recipizer.db ../sql/tables.sql ../../data/recipes.json
```

### List ingredients

```zsh
cd src/Recipizer.Cli
dotnet run list ingredients
```

Or with filter:

```zsh
cd src/Recipizer.Cli
dotnet run list ingredients %Brown%

# Id|Name
# -------
# 26|Dark brown sugar
```

### Add ingredient to inventory

```zsh
cd src/Recipizer.Cli
dotnet run list ingredients %salt% # first get ID

# Id|Name
# -------
# 11|Salt

dotnet run inventory add 11
```

Or add multiple:

```zsh
dotnet run inventory add 11,12,27
```

### List ingredients in inventory

```zsh
cd src/Recipizer.Cli
dotnet run list inventory

# Id|Name|Added
# -------------
# 11|Salt|2023-10-05
# 12|Egg|2023-10-04
```

### Remove ingredient from inventory

```zsh
cd src/Recipizer.Cli
dotnet run list ingredients %salt% # first get ID

# Id|Name
# -------
# 11|Salt

dotnet run inventory remove 11
```

Or remove multiple:

```zsh
dotnet run inventory remove 11,12,27
```
