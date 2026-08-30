#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"

# Running net462 tests on Linux needs mono plus an SDK that ships
# TestHostNetFramework; the user-local SDK does, the distro one does not.
if [ -x "$HOME/.dotnet/dotnet" ]; then
    export DOTNET_ROOT="$HOME/.dotnet"
    export PATH="$HOME/.dotnet:$PATH"
fi

dotnet test tests/PlayniteLibraryServer.Tests.csproj "$@"
