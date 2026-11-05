# Internationalization implementation guide

This document is the implementation brief for internationalizing TerminalMgr. It describes the current state, the product scope, and the acceptance criteria. Treat it as the source of truth for an i18n implementation unless the repository owner changes the language requirements.

## Current state

- The product is a .NET 8 WPF desktop application. Its terminal page and command palette are HTML/CSS/JavaScript served inside WebView2; see `src/TerminalMgr.App/Terminal/`.
- Most user-facing copy is hard-coded in German in WPF XAML and C# (including programmatically built dialogs), and in the embedded HTML/JavaScript.
- `Terminal/index.html` declares `lang="de"` and contains German empty-state copy.
- `Resources/SharedResources.resx`, `.de.resx`, and `.es.resx` are not a reliable localization catalog: each currently has the same 1,135 `STR_<hash>` entries, including build/runtime identifiers, untranslated German, and mojibake. `SharedResources.Designer.cs` contains only three constants, `Resources.cs` is a stub, and the current UI does not use this catalog consistently. Do not build on these hashed entries or assume the German and Spanish files are valid translations. Replace or regenerate them as appropriate.
- The app already distinguishes some domain language settings (for example speech recognition language) from UI language. Keep those settings separate.
- The README and most product documentation are written in German. Some docs describe user-facing workflows; others are internal/technical. Decide and document the intended treatment rather than mechanically translating every repository string.

## Language baseline

Use English (`en`) as the neutral/source language, with German (`de`) and Spanish (`es`) as the initial translated locales, matching the existing resource filenames. Use the user's explicit UI-language preference when set; otherwise use the operating-system UI culture when supported, then fall back to English. A culture such as `es-MX` should fall back to `es`, then `en`.

Expose a language choice in the app's settings, with language names shown in their own language (English, Deutsch, Español). Persist only the language identifier in the existing user settings; never persist translated labels. Decide whether a language change applies immediately or after restart, implement that behavior consistently for both WPF and WebView2, and make it clear in the UI if restart is required. Do not add a locale beyond `en`, `de`, and `es` without an explicit product decision.

## Scope

Localize all TerminalMgr-authored text a user sees, including:

- WPF window titles, labels, buttons, menus, dialogs, tooltips, accessibility/automation names, validation, status text, notifications, confirmations, errors, empty/loading states, and dynamically composed messages.
- The embedded terminal page's own empty state, command palette, search placeholder, group names, result count, keyboard help, and empty/no-results messages.
- User-facing application output from bundled setup or integration flows where it is authored by TerminalMgr (for example setup success/failure messages).
- User documentation and onboarding material intended for end users, including the README's user-facing installation and feature guidance. Technical contributor docs may remain in their current language unless the repository owner requests otherwise; clearly identify that boundary.

Do not translate or alter:

- Terminal contents, SSH/tmux output, remote commands, shell snippets, command-line flags, keyboard key names, protocols, serialized/configuration identifiers, persisted JSON property names, enum values, telemetry/log identifiers, API fields, or third-party library content.
- Proper names and product/technology names (TerminalMgr, Windows, OpenSSH, tmux, WebView2, Claude, etc.), except for surrounding explanatory copy.
- Resource text that is not actually a user-visible string. Do not create catalog entries for XAML markup, framework names, code identifiers, CSS/HTML attributes, or build metadata.

Keep the product's functionality and technical meaning unchanged. Translation must not alter command syntax, shortcuts, privacy/security claims, or behavior described to users.

## Implementation requirements

1. **Establish one authoritative catalog.** Use stable, descriptive keys (for example `Settings.Save`, `Palette.NoResults`, `Errors.SettingsLoadFailed`), not hashes or source text as keys. Keep English in the neutral resource file and translated values in culture-specific files. Remove stale generated entries only after confirming they are no longer required. Prefer the .NET resource system already in the project for WPF; generate strongly typed accessors through the supported SDK/build tooling rather than hand-maintaining duplicate constants.
2. **Localize before first render.** Resolve the selected/system culture before the startup window is displayed. Apply the same effective UI culture to WPF resources, date/number formatting where appropriate, and WebView2's own TerminalMgr-authored UI. Avoid separate, divergent copies of web strings: use a single maintained source/catalog and pass the web UI's localized strings from the host or generate a web catalog from it.
3. **Support dynamic copy.** Replace hard-coded strings in both XAML and C#. Avoid static bindings that cannot update if the language is changed live. Any runtime language switch must refresh every open surface, or explicitly require/relaunch the app.
4. **Preserve formatting and culture boundaries.** Use UI culture for localized messages and formatting, and invariant culture for machine-readable data, file formats, protocol values, and command construction. Use named/positional format arguments safely; translations must preserve required placeholders. Use locale-aware pluralization where a message changes with a count; do not concatenate translated sentence fragments.
5. **Handle fallback and missing strings deliberately.** A missing `de` or `es` value must fall back to English, not show a key/hash or an empty string. Do not silently hide lookup errors. Keep translator comments for ambiguous context and placeholders where the resource format supports them.
6. **Keep layouts resilient.** Translated text can be substantially longer or use different scripts. Check resizing, wrapping, clipping, minimum window sizes, keyboard navigation, and accessibility labels in all supported locales. Avoid fixed-width assumptions where labels are rendered.
7. **Keep user data stable.** Switching UI language must not rewrite profiles, shortcuts, user-entered names/text, saved config files, command history, or any persisted key/value identifiers.

## Work plan

1. Audit user-facing strings across `src/TerminalMgr.App/` (all WPF `.xaml` and `.cs` files, especially `MainWindow.xaml`, `MainWindow.*.cs`, and the dialogs/panels), the non-vendor files under `src/TerminalMgr.App/Terminal/`, bundled TerminalMgr setup messages, and user-facing sections of `README.md` and `docs/`.
2. Define stable resource keys and a small shared localization API. Record the English source text and context before adding `de` and `es` translations. Exclude machine strings and third-party/vendor assets from the audit.
3. Add a persisted UI-language preference and resolve effective culture at startup with the fallback behavior above. Add the settings control and apply the chosen update/restart behavior.
4. Localize the WPF and WebView2 surfaces, including dynamically created messages, accessibility properties, and failure/empty states. Ensure the host-to-web localization boundary has no duplicate catalogs.
5. Translate the agreed end-user documentation and provide a clear link between equivalent localized documents. Leave developer docs alone unless they contain user instructions or are explicitly included in the agreed scope.
6. Add automated checks and tests, then review each supported locale in the running UI for missing strings, layout issues, and correct language selection/fallback.

## Acceptance criteria

- On a clean install, UI language selection is deterministic: explicit preference wins, supported OS UI language is next, English is the fallback.
- English, German, and Spanish cover every in-scope user-facing string in WPF, the embedded web UI, and the agreed end-user documentation. No hashed keys, untranslated UI copy, mojibake, or missing-resource placeholders are visible.
- The language choice is persisted and does not change domain settings such as dictation language or any user data. The behavior on changing language is consistent and explained.
- Dynamic messages preserve values, punctuation, and format placeholders in each locale; counts and date/number formatting are appropriate to the UI culture.
- Machine-oriented values and terminal/remote output remain unchanged and culture-invariant where required.
- Automated tests check catalog key parity, non-empty translations, format-placeholder parity, culture fallback, and language preference behavior. Tests cover important WPF/web localization integration points without depending on visual text alone.
- The solution builds and the existing relevant .NET and web tests pass. Perform a manual visual pass in each supported locale; record any platform-dependent limitation instead of claiming an unperformed check.

## Handoff checklist

- [ ] Confirm scope for end-user documentation and whether the initial locale set remains `en`, `de`, `es`.
- [ ] Replace the unusable hashed resource data with a clean, traceable catalog.
- [ ] Add language selection, persistence, startup culture selection, and fallback tests.
- [ ] Complete the WPF and embedded web UI audit and localization.
- [ ] Translate agreed user documentation and verify all three locales.
- [ ] Run builds/tests and visually verify expanded layouts and accessibility names.
