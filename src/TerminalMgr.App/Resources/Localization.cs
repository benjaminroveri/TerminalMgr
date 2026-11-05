using System.Globalization;
using System.Collections;
using System.Resources;

namespace TerminalMgr.App.Resources;

using TerminalMgr.App.Resources;

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
    public static string StartWithWindows => Get(nameof(StartWithWindows));
    public static string AIProvidersDescription => Get(nameof(AIProvidersDescription));
    public static string AIProvidersSetup => Get(nameof(AIProvidersSetup));
    public static string CheckOnStart => Get(nameof(CheckOnStart));
    public static string CheckOnStartTooltip => Get(nameof(CheckOnStartTooltip));
    public static string CheckNow => Get(nameof(CheckNow));
    public static string CheckNowTooltip => Get(nameof(CheckNowTooltip));
    public static string NoSkippedVersion => Get(nameof(NoSkippedVersion));
    public static string ResetSkippedVersion => Get(nameof(ResetSkippedVersion));
    public static string ConfigurationHeading => Get(nameof(ConfigurationHeading));
    public static string ConfigurationDescription => Get(nameof(ConfigurationDescription));
    public static string ExportConfig => Get(nameof(ExportConfig));
    public static string ExportConfigTooltip => Get(nameof(ExportConfigTooltip));
    public static string ImportConfig => Get(nameof(ImportConfig));
    public static string ImportConfigTooltip => Get(nameof(ImportConfigTooltip));
    public static string ShortcutsHeading => Get(nameof(ShortcutsHeading));
    public static string ShortcutsDescription => Get(nameof(ShortcutsDescription));
    public static string DictationStartStop => Get(nameof(DictationStartStop));
    public static string CopySelection => Get(nameof(CopySelection));
    public static string PasteClipboard => Get(nameof(PasteClipboard));
    public static string SessionModifier => Get(nameof(SessionModifier));
    public static string WindowModifier => Get(nameof(WindowModifier));
    public static string WindowModifierTooltip => Get(nameof(WindowModifierTooltip));
    public static string NextWindow => Get(nameof(NextWindow));
    public static string PreviousWindow => Get(nameof(PreviousWindow));
    public static string BackShortcut => Get(nameof(BackShortcut));
    public static string PaletteShortcut => Get(nameof(PaletteShortcut));
    public static string FavoritesButton => Get(nameof(FavoritesButton));
    public static string FavoritesButtonTooltip => Get(nameof(FavoritesButtonTooltip));
    public static string ClipboardHeading => Get(nameof(ClipboardHeading));
    public static string HistoryLimitLabel => Get(nameof(HistoryLimitLabel));
    public static string AcceptOsc52Label => Get(nameof(AcceptOsc52Label));
    public static string AcceptOsc52Tooltip => Get(nameof(AcceptOsc52Tooltip));
    public static string CopyOnSelectLabel => Get(nameof(CopyOnSelectLabel));
    public static string CopyOnSelectTooltip => Get(nameof(CopyOnSelectTooltip));
    public static string DictationHeading => Get(nameof(DictationHeading));
    public static string EngineLabel => Get(nameof(EngineLabel));
    public static string EngineTooltip => Get(nameof(EngineTooltip));
    public static string MicrophoneLabel => Get(nameof(MicrophoneLabel));
    public static string ModelLabel => Get(nameof(ModelLabel));
    public static string FastModelLabel => Get(nameof(FastModelLabel));
    public static string PromptLabel => Get(nameof(PromptLabel));
    public static string SilenceLabel => Get(nameof(SilenceLabel));
    public static string PingStartLabel => Get(nameof(PingStartLabel));
    public static string PingEndLabel => Get(nameof(PingEndLabel));
    public static string PingVolumeLabel => Get(nameof(PingVolumeLabel));
    public static string PrepareAtStartLabel => Get(nameof(PrepareAtStartLabel));
    public static string ThresholdLabel => Get(nameof(ThresholdLabel));
    public static string ClipboardEntriesLabel => Get(nameof(ClipboardEntriesLabel));
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
        public static string ConfigCheck37 => Get("ConfigCheck_37");
        public static string WhisperModels17 => Get("WhisperModels_17");
        public static string Profile29 => Get("Profile_29");
        public static string Navigation85 => Get("Navigation_85");
        public static string TmuxWindows57 => Get("TmuxWindows_57");
        public static string TmuxWindows58 => Get("TmuxWindows_58");
        public static string ConfigFile132 => Get("ConfigFile_132");
        public static string ConfigFile137 => Get("ConfigFile_137");
        public static string ConfigFile142 => Get("ConfigFile_142");
        public static string ConfigFile143 => Get("ConfigFile_143");
        public static string ConfigFile144 => Get("ConfigFile_144");
        public static string ConfigFile149 => Get("ConfigFile_149");
        public static string ConfigFile155 => Get("ConfigFile_155");
        public static string ConfigFile157 => Get("ConfigFile_157");
        public static string UpdateSwap34 => Get("UpdateSwap_34");
        public static string ReconnectPolicy36 => Get("ReconnectPolicy_36");
        public static string TmuxStripStream41 => Get("TmuxStripStream_41");
        public static string AiAccountSource25 => Get("AiAccountSource_25");
        public static string FavoritesDialog44 => Get("FavoritesDialog_44");
        public static string WindowsSpeechEngine92 => Get("WindowsSpeechEngine_92");
}

    public static string terminal.spec_187 => Get("terminal.spec_187");

    public static string terminal.spec_191 => Get("terminal.spec_191");

    public static string terminal.spec_213 => Get("terminal.spec_213");
public static string palette.spec_27 => Get("palette.spec_27");

    public static string palette.spec_28 => Get("palette.spec_28");

    public static string palette.spec_30 => Get("palette.spec_30");

    public static string palette.spec_73 => Get("palette.spec_73");

    public static string palette.spec_75 => Get("palette.spec_75");

    public static string palette.spec_86 => Get("palette.spec_86");

    public static string palette.spec_150 => Get("palette.spec_150");

    public static string Program_872 => Get("Program_872");

    public static string ConfigCheck_44 => Get("ConfigCheck_44");

    public static string UpdateInfo_90 => Get("UpdateInfo_90");

    public static string Program_301 => Get("Program_301");

    public static string Program_376 => Get("Program_376");

    public static string Program_474 => Get("Program_474");

    public static string Program_782 => Get("Program_782");

    public static string Program_1197 => Get("Program_1197");

    public static string Program_1276 => Get("Program_1276");

    public static string Program_1326 => Get("Program_1326");

    public static string Program_1404 => Get("Program_1404");

    public static string Program_1417 => Get("Program_1417");

    public static string Program_1581 => Get("Program_1581");

    public static string SpeechBench_65 => Get("SpeechBench_65");

    public static string Program_1836 => Get("Program_1836");

    public static string Program_384 => Get("Program_384");

    public static string Program_1583 => Get("Program_1583");

    public static string ConfigCheck_37 => Get("ConfigCheck_37");

    public static string ConfigCheck_124 => Get("ConfigCheck_124");

    public static string ConfigCheck_35 => Get("ConfigCheck_35");
