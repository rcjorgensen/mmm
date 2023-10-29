#!/bin/sh

set -xe

dotnet publish -c release -r ubuntu.22.04-x64 --self-contained
