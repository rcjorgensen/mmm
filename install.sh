#!/bin/sh

# publish
dotnet publish -c release -r ubuntu.22.04-x64 --self-contained

install_dir=$HOME/.local/bin/mmm

# create installation dir
mkdir -p $install_dir

# copy everything to install dir
cp src/mmm/bin/Release/net7.0/ubuntu.22.04-x64/publish/* $install_dir

# copy configuration to install dir
cp src/mmm/appsettings.json $install_dir

# inform about env variable
echo 'INFO: Make sure the following gets set for your shell session `export MMM_INSTALL_DIR="'$install_dir'"`'
