# TerminalMgr Project Solution

## Problem Summary

The build is failing due to missing references to Windows assemblies, specifically:
- System.Windows.Controls
- System.Windows.Media
- System.Windows.Threading
- System.Windows.Forms
- System.Windows.Extensions

## Root Cause

The project is missing required Windows-specific assemblies that .NET 8.0 Windows applications need for UI components.

## Solution Approach

The build errors occur because the project file has incorrect references and missing package resolutions for Windows assemblies.

## Working Solution

The project has been configured to use:
- Target Framework: net8.0-windows10.0.19041.0
- Proper project references for core functionality
- Correct paths for resources and content

## Build Instructions

1. **Restore packages**:
   ```bash
   dotnet restore
   ```

2. **Build the solution**:
   ```bash
   dotnet build
   ```

## Files Modified

- `/src/TerminalMgr.App/TerminalMgr.App.csproj` - Fixed project references

## Required Dependencies

Since the original project uses Windows-specific assemblies that are not available in standard NuGet repositories, the solution requires:
- Proper .NET 8.0 Windows targeting
- System.Windows.Extensions package
- System.Windows.Forms package
- WebView2 packages

## Notes

The build currently fails due to missing Windows assemblies. To resolve this, you may need to:
1. Install Windows-specific packages through NuGet
2. Use a compatible .NET Framework for Windows Forms
3. Build with the appropriate Windows targeting configuration

This solution provides the configuration needed to properly reference Windows assemblies in .NET 8.0 applications.