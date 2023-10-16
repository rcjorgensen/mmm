#!/bin/sh

# publish
dotnet publish -c release -r ubuntu.22.04-x64 --self-contained

# create installation dir
mkdir -p $HOME/.local/bin/recipizer

# create temp dir
rm -rf temp
mkdir temp

# copy everything to temp dir
cp src/Recipizer.Cli/bin/Release/net7.0/ubuntu.22.04-x64/publish/* temp

# rename binary
mv temp/Recipizer.Cli temp/recipizer

# copy configuration and data files
cp data/recipes.json temp
cp src/Recipizer.Cli/appsettings.json temp

# copy everything to install dir
cp temp/* $HOME/.local/bin/recipizer

# clean up temp dir
rm -r temp

# inform about env variable
echo 'make sure the following environment variable is set: export RECIPIZER_INSTALL_DIR="$HOME/.local/bin/recipizer"'