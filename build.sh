#!/bin/bash
# Build script for TerminalMgr project

# Set permissions
chmod +x "$0"

# Clean previous builds
echo "Cleaning previous builds..."
dotnet clean

# Restore packages
echo "Restoring packages..."
dotnet restore

# Build the solution
echo "Building solution..."
dotnet build

echo "Build complete."
