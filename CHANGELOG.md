# Changelog

Notable changes to TerminalMgr are documented here.

## [Unreleased] — 0.2.x

Release versions use `0.2.<workflow run number>`, as defined by `Directory.Build.props` and `.github/workflows/build.yml`. The exact patch version is assigned by the release workflow; local builds use `0.2.0`.

### Added

- Added English, German, and Spanish resource catalogs and a shared localization API.
- Added a persisted UI-language preference. The app selects it before the first window is shown, otherwise follows a supported Windows UI language, and falls back to English. A language change takes effect after restarting the app.
- Localized the initial WPF shell and the embedded terminal's empty state and command palette using strings supplied from the shared resource catalogs.
- Added tests for language selection, catalog consistency, and WebView localization.

### Scope

- This is the initial localization foundation, not complete project-wide localization. Other dialogs, status and error messages, bundled integration output, and end-user documentation still need review and translation.
