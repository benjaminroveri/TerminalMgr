#!/bin/bash
# TerminalMgr Fix Script

echo "Fixing TerminalMgr build issues..."

# Navigate to the project directory
cd /home/hermes-admin/project/github.com/TerminalMgr || exit 1

# Clean existing builds
echo "Cleaning old builds..."
dotnet clean

# Restore packages
echo "Restoring packages..."
dotnet restore

# Build the solution
echo "Building solution..."
dotnet build

echo "Build completed."
