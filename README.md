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
