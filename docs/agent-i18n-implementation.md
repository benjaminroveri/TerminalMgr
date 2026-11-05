# Agent i18n Implementation Guide

## Overview
This implementation provides a comprehensive localization framework for the TerminalMgr application that supports English, German, and Spanish languages.

## Implementation Details

### Core Localization Class
The `AppLocalization` class in `src/TerminalMgr.App/Resources/Localization.cs` provides:

1. **Culture Management**: Proper culture handling with fallback behavior
2. **Resource Access**: Gets localized strings by key with proper error handling
3. **Web String Support**: Specialized handling for web interface strings

### Resource Files
- `SharedResources.resx`: English (primary) resource file
- `SharedResources.de.resx`: German translations
- `SharedResources.es.resx`: Spanish translations

### Implementation Requirements

1. **Localization Keys**: Use descriptive names, not hashes or source text
2. **Fallback Behavior**: Implement proper fallback to English when resources missing
3. **Culture Integration**: Support culture-specific translations
4. **Resource Management**: Handle all user-facing strings properly

### Usage Pattern
In UI components, use:
```csharp
AppLocalization.WindowTitle
AppLocalization.ServerListTitle
AppLocalization.LanguageLabel
```

### Integration Points
1. **MainWindow.xaml**: UI element text binding
2. **SettingsDialog**: Settings panel localization
3. **Dialogs**: All dialog text and labels
4. **Web Interfaces**: Palette and web UI elements

This framework provides a complete solution that can be used by agents to implement localization across the entire TerminalMgr application.