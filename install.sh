#!/bin/sh

# publish
dotnet publish -c release -r ubuntu.22.04-x64 --self-contained

install_dir=$HOME/.local/bin/recipizer

# create installation dir
mkdir -p $install_dir

# create temp dir
rm -rf temp
mkdir temp

# copy everything to install dir
cp src/rcp/bin/Release/net7.0/ubuntu.22.04-x64/publish/* $install_dir

# copy configuration to install dir
cp src/rcp/appsettings.json $install_dir

# inform about env variable
echo 'INFO: Put the following in your .zshrc or similar: export RECIPIZER_INSTALL_DIR="'$install_dir'"'