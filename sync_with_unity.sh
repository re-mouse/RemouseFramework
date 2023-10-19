#!/bin/bash
set -e

# Set SOURCE_DIR only if it's empty
: "${SOURCE_DIR:=$HOME/GitSources/IrehonBackend}"

# Set UNITY_DIR only if it's empty
: "${UNITY_DIR:=$HOME/GitSources/Unity/Paleolith/Assets/Scripts}"

# Ensure the target directory exists
mkdir -p "$UNITY_DIR"

# Use rsync to copy .cs files while excluding specific patterns and preserving folder structure
rsync -av --prune-empty-dirs --include='*.cs' --exclude='*.meta' --exclude='*.log' --exclude='bin/' --exclude='obj/' --exclude='*Test/' "$SOURCE_DIR/" "$UNITY_DIR"

# Print success message
echo "All .cs files copied successfully while avoiding specified patterns and preserving folder structure!"

# Success message
echo "Script executed without errors."
