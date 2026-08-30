#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"

dotnet build src/PlayniteLibraryServer.csproj -c Release

OUT_DIR="dist"
rm -rf "$OUT_DIR"
mkdir -p "$OUT_DIR"

BUILD_DIR="src/bin/Release/net462"
cp "$BUILD_DIR/PlayniteLibraryServer.dll" "$OUT_DIR/"
cp "src/extension.yaml" "$OUT_DIR/"

VERSION=$(grep '^Version:' src/extension.yaml | awk '{print $2}')
(cd "$OUT_DIR" && zip -r "../PlayniteLibraryServer_${VERSION}.pext" .)

echo "Built PlayniteLibraryServer_${VERSION}.pext"
