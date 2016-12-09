#!/bin/bash

CURRENT_DIR="$(dirname $0)"

nuget restore "$CURRENT_DIR"/Newq.sln
xbuild /p:Configuration=Release Newq.sln
nuget pack "$CURRENT_DIR"/src/Newq/newq.nuspec -Prop Configuration=Release
