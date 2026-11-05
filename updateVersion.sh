#!/bin/bash

set -euo pipefail

printf 'new_release_version=%s\n' "$1" >> "$GITHUB_OUTPUT"
sed -i "s#<PackageVersion>.*#<PackageVersion>$1</PackageVersion>#" "$2"
sed -i "s#<Version>.*#<Version>$1</Version>#" "$2"
