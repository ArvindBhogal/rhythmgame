#!/bin/bash

cd -- "$(dirname "$BASH_SOURCE")"
chmod +x Alpha\ 1.0.app/Contents/MacOS/RhythmGame
xattr -r -d com.apple.quarantine Alpha\ 1.0.app/
