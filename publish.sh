#!/bin/sh

set -xe

dotnet publish src/r7r -c release --self-contained
