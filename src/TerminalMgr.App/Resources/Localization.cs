using System.Globalization;
using System.Collections;
using System.Resources;

namespace TerminalMgr.App.Resources;

public static class AppLocalization
{
    private static readonly ResourceManager Manager = new(
        "TerminalMgr.App.Resources.SharedResources",
        typeof(AppLocalization).Assembly);

    public static CultureInfo Culture { get; private set; } = CultureInfo.GetCultureInfo("en");

    public static void SetCulture(CultureInfo culture)
    {
        Culture = culture;
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }

    public static string Get(string key) =>
        Manager.GetString(key, Culture)
        ?? throw new InvalidOperationException($"Missing localization resource '{key}' for '{Culture.Name}'.");

    public static IReadOnlyDictionary<string, string> GetCatalog(string cultureName)
    {
        CultureInfo culture = cultureName == "en" ? CultureInfo.InvariantCulture : CultureInfo.GetCultureInfo(cultureName);
        ResourceSet resources = Manager.GetResourceSet(culture, createIfNotExists: true, tryParents: false)
            ?? throw new InvalidOperationException($"Localization catalog '{cultureName}' is missing.");
        return resources.Cast<DictionaryEntry>().ToDictionary(
            entry => (string)entry.Key,
            entry => (string)entry.Value!,
            StringComparer.Ordinal);
    }

    public static string SettingsTitle => Get(nameof(SettingsTitle));
    public static string AIProvidersHeading => Get(nameof(AIProvidersHeading));
    public static string UpdatesHeading => Get(nameof(UpdatesHeading));
    public static string SaveButton => Get(nameof(SaveButton));
    public static string CancelButton => Get(nameof(CancelButton));
    public static string StandardButton => Get(nameof(StandardButton));
    public static string WindowTitle => Get(nameof(WindowTitle));
    public static string LoadingTitle => Get(nameof(LoadingTitle));
    public static string LoadingText => Get(nameof(LoadingText));
    public static string FocusModeTooltip => Get(nameof(FocusModeTooltip));
    public static string SettingsTooltip => Get(nameof(SettingsTooltip));
    public static string ServerListTooltip => Get(nameof(ServerListTooltip));
    public static string ClipboardTooltip => Get(nameof(ClipboardTooltip));
    public static string DictationTooltip => Get(nameof(DictationTooltip));
    public static string StripTooltip => Get(nameof(StripTooltip));
    public static string ServerListTitle => Get(nameof(ServerListTitle));
    public static string AddProfileButton => Get(nameof(AddProfileButton));
    public static string ImportProfilesButton => Get(nameof(ImportProfilesButton));
    public static string ConnectButton => Get(nameof(ConnectButton));
    public static string DisconnectButton => Get(nameof(DisconnectButton));
    public static string EditProfileButton => Get(nameof(EditProfileButton));
    public static string AutostartButton => Get(nameof(AutostartButton));
    public static string SessionHintText => Get(nameof(SessionHintText));
    public static string AiSourceLabel => Get(nameof(AiSourceLabel));
    public static string AiPanelTitle => Get(nameof(AiPanelTitle));
    public static string ClipboardTitle => Get(nameof(ClipboardTitle));
    public static string ClipboardHintText => Get(nameof(ClipboardHintText));
    public static string PaletteButton => Get(nameof(PaletteButton));
    public static string PaletteButtonText => Get(nameof(PaletteButtonText));
    public static string DictationStatus => Get(nameof(DictationStatus));
    public static string DictationTitle => Get(nameof(DictationTitle));
    public static string DictationRecordButton => Get(nameof(DictationRecordButton));
    public static string DictationInsertButton => Get(nameof(DictationInsertButton));
    public static string DictationInsertRunButton => Get(nameof(DictationInsertRunButton));
    public static string DictationDiscardButton => Get(nameof(DictationDiscardButton));
    public static string DictationInsertText => Get(nameof(DictationInsertText));
    public static string EmptyStateText => Get(nameof(EmptyStateText));
    public static string LanguageLabel => Get(nameof(LanguageLabel));
    public static string LanguageRestartNotice => Get(nameof(LanguageRestartNotice));
    public static string LanguageRestartNote => Get(nameof(LanguageRestartNote));
    public static string LanguageSystemDefault => Get(nameof(LanguageSystemDefault));
    public static string GeneralHeading => Get(nameof(GeneralHeading));

    public static IReadOnlyDictionary<string, string> WebStrings { get; } = new Dictionary<string, string>
    {
        ["emptyTitle"] = "Web.EmptyTitle",
        ["emptyDescriptionFirst"] = "Web.EmptyDescriptionFirst",
        ["emptyDescriptionSecond"] = "Web.EmptyDescriptionSecond",
        ["emptyShortcut"] = "Web.EmptyShortcut",
        ["paletteLabel"] = "Web.PaletteLabel",
        ["palettePlaceholder"] = "Web.PalettePlaceholder",
        ["paletteHelp"] = "Web.PaletteHelp",
        ["paletteNoItems"] = "Web.PaletteNoItems",
        ["paletteNoResults"] = "Web.PaletteNoResults",
        ["paletteNoRecent"] = "Web.PaletteNoRecent",
        ["paletteResultCountOne"] = "Web.PaletteResultCountOne",
        ["paletteResultCountOther"] = "Web.PaletteResultCountOther",
        ["groupFavorites"] = "Web.GroupFavorites",
        ["groupRecent"] = "Web.GroupRecent",
        ["groupWindows"] = "Web.GroupWindows",
        ["groupClipboard"] = "Web.GroupClipboard",
        ["groupActions"] = "Web.GroupActions"
    };

    public static Dictionary<string, string> GetWebStrings() =>
        WebStrings.ToDictionary(pair => pair.Key, pair => Get(pair.Value), StringComparer.Ordinal);
}
