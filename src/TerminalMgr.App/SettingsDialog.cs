using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TerminalMgr.App.Resources;
using TerminalMgr.Core;
using TerminalMgr.Windows;

namespace TerminalMgr.App;

/// <summary>Application settings: general, shortcuts, clipboard and dictation.</summary>
public enum ConfigAction { None, Export, Import }

public sealed class SettingsDialog : Window
{
    private const string DefaultMicrophone = "Windows-Standard";
    private readonly AppSettings _original;
    private readonly StackPanel _panel = new() { Margin = new Thickness(24) };
    private readonly CheckBox _startWithWindows, _acceptOsc52, _copyOnSelect, _checkUpdates, _pingStart, _pingEnd, _prepareAtStart;
    private string? _skippedVersion;
    /// <summary>The user pressed "Jetzt nach Updates suchen": the settings are saved and the caller runs the check.</summary>
    public bool CheckForUpdatesNow { get; private set; }
    /// <summary>"Exportieren" or "Importieren" was pressed: the settings are saved and the caller opens the matching dialog.</summary>
    public ConfigAction ConfigAction { get; private set; }
    public event Action? AiSetupRequested;
    private readonly TextBox _pingVolume;
    private readonly TextBox _historyLimit, _dictationShortcut, _copyShortcut, _pasteShortcut, _prompt, _silence, _threshold;
    private readonly ComboBox _microphone, _model, _interimModel, _language, _sessionModifier, _engine, _windowModifier, _uiLanguage;
    private readonly TextBox _nextShortcut, _previousShortcut, _backShortcut, _paletteShortcut;
    private List<Favorite> _favorites;
    public AppSettings? Result { get; private set; }

    public SettingsDialog(AppSettings settings, IReadOnlyList<NavigationPosition>? recentWindows = null)
    {
        _original = settings;
        _favorites = [.. settings.Favorites];
        var dictation = settings.Dictation.Normalized();
        var shortcuts = settings.Shortcuts.Normalized();
        Style = (Style)FindResource(typeof(Window));
        Title = AppLocalization.SettingsTitle;
        Width = 620; Height = 860; MinHeight = 420; WindowStartupLocation = WindowStartupLocation.CenterOwner;
        Content = new ScrollViewer { Content = _panel, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };

        Heading(AppLocalization.Get("GeneralHeading"));
        _uiLanguage = Combo(AppLocalization.LanguageLabel,
            [AppLocalization.LanguageSystemDefault, "English", "Deutsch", "Español"],
            settings.UiLanguage switch { "en" => "English", "de" => "Deutsch", "es" => "Español", _ => AppLocalization.LanguageSystemDefault });
        _panel.Children.Add(new TextBlock {
            Text = AppLocalization.LanguageRestartNote, TextWrapping = TextWrapping.Wrap,
            Foreground = Brushes.LightSlateGray, FontSize = 11, Margin = new Thickness(0, -8, 0, 8)
        });
        _startWithWindows = new CheckBox { Content = AppLocalization.Get("StartWithWindows"), IsChecked = settings.StartWithWindows };
        _panel.Children.Add(_startWithWindows);

        Heading(AppLocalization.AIProvidersHeading);
        _panel.Children.Add(new TextBlock { Text = AppLocalization.Get("AIProvidersDescription"), TextWrapping = TextWrapping.Wrap });
        var aiSetup = new Button { Content = AppLocalization.Get("AIProvidersSetup"), HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(0, 8, 0, 0) };
        aiSetup.Click += (_, _) => AiSetupRequested?.Invoke();
        _panel.Children.Add(aiSetup);

        Heading(AppLocalization.UpdatesHeading);
        _skippedVersion = settings.Update.SkippedVersion;
        _checkUpdates = new CheckBox { Content = AppLocalization.Get("CheckOnStart"), IsChecked = settings.Update.CheckOnStart,
            ToolTip = AppLocalization.Get("CheckOnStartTooltip") };
        _panel.Children.Add(_checkUpdates);
        var updateButtons = new WrapPanel();
        var checkNow = new Button { Content = AppLocalization.Get("CheckNow"), ToolTip = AppLocalization.Get("CheckNowTooltip") };
        checkNow.Click += (_, _) => { CheckForUpdatesNow = true; Save(); };
        var resetSkipped = new Button { Content = _skippedVersion is null ? AppLocalization.Get("NoSkippedVersion") : string.Format(AppLocalization.Get("ResetSkippedVersion"), _skippedVersion), IsEnabled = _skippedVersion is not null };
        resetSkipped.Click += (_, _) => { _skippedVersion = null; resetSkipped.Content = AppLocalization.Get("NoSkippedVersion"); resetSkipped.IsEnabled = false; };
        updateButtons.Children.Add(checkNow);
        updateButtons.Children.Add(resetSkipped);
        _panel.Children.Add(updateButtons);

        Heading(AppLocalization.Get("ConfigurationHeading"));
        _panel.Children.Add(new TextBlock { Text = AppLocalization.Get("ConfigurationDescription"),
            TextWrapping = TextWrapping.Wrap, Foreground = Brushes.LightSlateGray, FontSize = 11, Margin = new Thickness(0, 0, 0, 6) });
        var configButtons = new WrapPanel();
        var export = new Button { Content = AppLocalization.Get("ExportConfig"), ToolTip = AppLocalization.Get("ExportConfigTooltip") };
        export.Click += (_, _) => { ConfigAction = ConfigAction.Export; Save(); };
        var import = new Button { Content = AppLocalization.Get("ImportConfig"), ToolTip = AppLocalization.Get("ImportConfigTooltip") };
        import.Click += (_, _) => { ConfigAction = ConfigAction.Import; Save(); };
        configButtons.Children.Add(export);
        configButtons.Children.Add(import);
        _panel.Children.Add(configButtons);

        Heading(AppLocalization.Get("ShortcutsHeading"));
        _panel.Children.Add(new TextBlock { Text = AppLocalization.Get("ShortcutsDescription"),
            TextWrapping = TextWrapping.Wrap, Foreground = Brushes.LightSlateGray, FontSize = 11, Margin = new Thickness(0, 0, 0, 8) });
        _dictationShortcut = ShortcutField(AppLocalization.Get("DictationStartStop"), dictation.Shortcut, new DictationOptions().Shortcut);
        _copyShortcut = ShortcutField(AppLocalization.Get("CopySelection"), shortcuts.Copy, new ShortcutOptions().Copy);
        _pasteShortcut = ShortcutField(AppLocalization.Get("PasteClipboard"), shortcuts.Paste, new ShortcutOptions().Paste);
        _sessionModifier = Combo(AppLocalization.Get("SessionModifier"), ShortcutRules.SessionModifiers.Select(m => m + "+1 … 9"), shortcuts.SessionModifier + "+1 … 9");

        _windowModifier = Combo(AppLocalization.Get("WindowModifier"), ShortcutRules.WindowModifiers.Select(m => m + "+0 … 9"), shortcuts.WindowModifier + "+0 … 9");
        _windowModifier.ToolTip = AppLocalization.Get("WindowModifierTooltip");
        _nextShortcut = ShortcutField(AppLocalization.Get("NextWindow"), shortcuts.NextWindow, new ShortcutOptions().NextWindow);
        _previousShortcut = ShortcutField(AppLocalization.Get("PreviousWindow"), shortcuts.PreviousWindow, new ShortcutOptions().PreviousWindow);
        _backShortcut = ShortcutField(AppLocalization.Get("BackShortcut"), shortcuts.Back, new ShortcutOptions().Back);
        _paletteShortcut = ShortcutField(AppLocalization.Get("PaletteShortcut"), shortcuts.Palette, new ShortcutOptions().Palette);
        var favoritesButton = new Button { Content = string.Format(AppLocalization.Get("FavoritesButton"), _favorites.Count), ToolTip = AppLocalization.Get("FavoritesButtonTooltip"), HorizontalAlignment = HorizontalAlignment.Left };
        favoritesButton.Click += (_, _) =>
        {
            var dialog = new FavoritesDialog(_favorites, _original.Profiles, recentWindows ?? []) { Owner = this };
            if (dialog.ShowDialog() == true && dialog.Result is { } result) { _favorites = result; favoritesButton.Content = string.Format(AppLocalization.Get("FavoritesButton"), _favorites.Count); }
        };
        _panel.Children.Add(favoritesButton);

        Heading(AppLocalization.Get("ClipboardHeading"));
        _historyLimit =        Field(AppLocalization.Get("HistoryLimitLabel"), settings.ClipHistoryLimit.ToString());
        _acceptOsc52 = new CheckBox { Content = AppLocalization.Get("AcceptOsc52Label"), IsChecked = settings.AcceptOsc52,
            ToolTip = AppLocalization.Get("AcceptOsc52Tooltip") };
        _panel.Children.Add(_acceptOsc52);
        _copyOnSelect = new CheckBox { Content = AppLocalization.Get("CopyOnSelectLabel"), IsChecked = settings.CopyOnSelect,
            ToolTip = AppLocalization.Get("CopyOnSelectTooltip") };
        _panel.Children.Add(_copyOnSelect);

        Heading(AppLocalization.Get("DictationHeading"));
        _engine = Combo(AppLocalization.Get("EngineLabel"), SpeechEngineIds.All.Select(SpeechEngineIds.Label), SpeechEngineIds.Label(dictation.Engine));
        _engine.ToolTip = AppLocalization.Get("EngineTooltip");
        _microphone = Combo(AppLocalization.Get("MicrophoneLabel"), MicrophoneNames(), dictation.Microphone is { Length: > 0 } m ? m : DefaultMicrophone);
        _model = Combo(AppLocalization.Get("ModelLabel"), WhisperModels.All.Select(ModelLabel), ModelLabel(WhisperModels.Get(dictation.Model)));
        _interimModel = Combo(AppLocalization.Get("FastModelLabel"), WhisperModels.All.Select(ModelLabel), ModelLabel(WhisperModels.Get(dictation.InterimModel)));
        _language = Combo(AppLocalization.Get("LanguageLabel"), ["Deutsch", "Englisch", "Automatisch erkennen"], dictation.Language switch { "en" => "Englisch", "auto" => "Automatisch erkennen", _ => "Deutsch" });
        _prompt = Field(AppLocalization.Get("PromptLabel"), dictation.InitialPrompt);
        _prompt.AcceptsReturn = true; _prompt.TextWrapping = TextWrapping.Wrap; _prompt.Height = 70; _prompt.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        _silence = Field(AppLocalization.Get("SilenceLabel"), dictation.SilenceSeconds.ToString(CultureInfo.CurrentCulture));
        _pingStart = new CheckBox { Content = AppLocalization.Get("PingStartLabel"), IsChecked = dictation.PingStart, Margin = new Thickness(0, 10, 0, 0) };
        _pingEnd = new CheckBox { Content = AppLocalization.Get("PingEndLabel"), IsChecked = dictation.PingEnd };
        _panel.Children.Add(_pingStart);
        _panel.Children.Add(_pingEnd);
        _pingVolume = Field(AppLocalization.Get("PingVolumeLabel"), dictation.PingVolume.ToString(CultureInfo.CurrentCulture));
        _prepareAtStart = new CheckBox { Content = AppLocalization.Get("PrepareAtStartLabel"), IsChecked = dictation.PrepareAtStart, Margin = new Thickness(0, 8, 0, 0) };
        _panel.Children.Add(_prepareAtStart);
        _threshold = Field(AppLocalization.Get("ThresholdLabel"), dictation.LevelThreshold.ToString(CultureInfo.CurrentCulture));

        _panel.Children.Add(new TextBlock {
            Text = $"TerminalMgr {AppInfo.Version} · {AppLocalization.Get("ClipboardEntriesLabel")}: {AppPaths.DataDirectory()}",
            TextWrapping = TextWrapping.Wrap, Foreground = Brushes.LightSlateGray, FontSize = 11, Margin = new Thickness(0, 18, 0, 0) });
        var buttons = new WrapPanel { Margin = new Thickness(0, 12, 0, 0) };
        _panel.Children.Add(buttons);
        var save = new Button { Content = AppLocalization.SaveButton, IsDefault = true };
        save.Click += (_, _) => Save();
        buttons.Children.Add(save);
        buttons.Children.Add(new Button { Content = AppLocalization.CancelButton, IsCancel = true });
    }

    private void Heading(string text) => _panel.Children.Add(new TextBlock {
        Text = text, FontSize = 16, FontWeight = FontWeights.SemiBold, Margin = new Thickness(0, _panel.Children.Count == 0 ? 0 : 18, 0, 8) });

    private TextBox Field(string label, string value)
    {
        _panel.Children.Add(new TextBlock { Text = label, TextWrapping = TextWrapping.Wrap });
        var box = new TextBox { Text = value };
        _panel.Children.Add(box);
        return box;
    }

    /// <summary>A read-only box that records the pressed combination, a reset button and a note when the key is also used in the terminal.</summary>
    private TextBox ShortcutField(string label, string value, string standard)
    {
        _panel.Children.Add(new TextBlock { Text = label });
        var box = new TextBox { Text = value, IsReadOnly = true, IsReadOnlyCaretVisible = false, Margin = new Thickness(0, 4, 0, 2) };
        var hint = new TextBlock { TextWrapping = TextWrapping.Wrap, Foreground = Brushes.Goldenrod, FontSize = 11, Margin = new Thickness(0, 0, 0, 10) };
        void ShowHint() => hint.Text = Shortcut.TryParse(box.Text, out var s) ? ShortcutRules.TerminalHint(s) ?? "" : "";
        box.TextChanged += (_, _) => ShowHint();
        box.PreviewKeyDown += CaptureShortcut;
        var reset = new Button { Content = AppLocalization.StandardButton, ToolTip = standard, Margin = new Thickness(6, 0, 0, 0) };
        reset.Click += (_, _) => box.Text = standard;
        var row = new DockPanel { LastChildFill = true };
        DockPanel.SetDock(reset, Dock.Right);
        row.Children.Add(reset);
        row.Children.Add(box);
        _panel.Children.Add(row);
        _panel.Children.Add(hint);
        ShowHint();
        return box;
    }

    private ComboBox Combo(string label, IEnumerable<string> items, string selected)
    {
        _panel.Children.Add(new TextBlock { Text = label, TextWrapping = TextWrapping.Wrap });
        var box = new ComboBox { Margin = new Thickness(0, 6, 0, 14), Padding = new Thickness(8), ItemsSource = items.ToList() };
        box.SelectedItem = box.Items.Cast<string>().FirstOrDefault(i => i == selected) ?? box.Items.Cast<string>().FirstOrDefault();
        _panel.Children.Add(box);
        return box;
    }

    private static string ModelLabel(WhisperModel model) =>
        $"{model.Name} · {model.SizeText} · {model.Description}" + (WhisperModels.IsInstalled(model) ? " · geladen" : "");

    private static IEnumerable<string> MicrophoneNames()
    {
        var names = new List<string> { DefaultMicrophone };
        try { names.AddRange(AudioDevices.Names()); } catch (Exception) { /* no audio subsystem: only the default entry */ }
        return names;
    }

    private void CaptureShortcut(object sender, KeyEventArgs e)
    {
        e.Handled = true;
        var pressed = ShortcutKeys.FromEvent(e);
        if (pressed is null) return; // only a modifier so far
        if (!Shortcut.TryParse(pressed.ToString(), out var shortcut))
        {
            Warn("Bitte mit Ctrl oder Alt kombinieren (nur F1 bis F12 gehen allein), damit normales Tippen nicht blockiert wird.");
            return;
        }
        ((TextBox)sender).Text = shortcut.ToString();
    }

    private static bool TryNumber(string text, out double value) =>
        double.TryParse(text.Trim().Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value);

    private void Save()
    {
        if (!int.TryParse(_historyLimit.Text.Trim(), out int limit) || limit is < 1 or > 1000)
        { Warn("Der Verlauf muss zwischen 1 und 1000 Einträge groß sein."); return; }
        if (!TryNumber(_silence.Text, out double silence) || silence is < 0.5 or > 10)
        { Warn("Die Stilledauer muss zwischen 0,5 und 10 Sekunden liegen."); return; }
        if (!TryNumber(_threshold.Text, out double threshold) || threshold is < 0.002 or > 0.2)
        { Warn("Die Pegelschwelle muss zwischen 0,002 und 0,2 liegen."); return; }
        if (!int.TryParse(_pingVolume.Text.Trim(), out int pingVolume) || pingVolume is < 0 or > 100)
        { Warn("Die Lautstärke der Signaltöne muss zwischen 0 und 100 liegen."); return; }
        string microphone = (string)_microphone.SelectedItem;
        var candidate = _original with {
            UiLanguage = _uiLanguage.SelectedIndex switch { 1 => "en", 2 => "de", 3 => "es", _ => null },
            ClipHistoryLimit = limit,
            StartWithWindows = _startWithWindows.IsChecked == true,
            AcceptOsc52 = _acceptOsc52.IsChecked == true,
            CopyOnSelect = _copyOnSelect.IsChecked == true,
            Update = new UpdateOptions { CheckOnStart = _checkUpdates.IsChecked == true, SkippedVersion = _skippedVersion },
            Shortcuts = new ShortcutOptions {
                Copy = _copyShortcut.Text, Paste = _pasteShortcut.Text,
                SessionModifier = ShortcutRules.SessionModifiers[Math.Max(0, _sessionModifier.SelectedIndex)],
                WindowModifier = ShortcutRules.WindowModifiers[Math.Max(0, _windowModifier.SelectedIndex)],
                NextWindow = _nextShortcut.Text, PreviousWindow = _previousShortcut.Text, Back = _backShortcut.Text, Palette = _paletteShortcut.Text
            }.Normalized(),
            Favorites = _favorites,
            Dictation = (_original.Dictation with {
                Shortcut = _dictationShortcut.Text,
                Microphone = microphone == DefaultMicrophone ? null : microphone,
                Engine = SpeechEngineIds.All[Math.Max(0, _engine.SelectedIndex)],
                Model = WhisperModels.All[Math.Max(0, _model.SelectedIndex)].Name,
                InterimModel = WhisperModels.All[Math.Max(0, _interimModel.SelectedIndex)].Name,
                Language = _language.SelectedIndex switch { 1 => "en", 2 => "auto", _ => "de" },
                InitialPrompt = _prompt.Text.Trim(),
                SilenceSeconds = silence,
                LevelThreshold = threshold,
                PingStart = _pingStart.IsChecked == true, PingEnd = _pingEnd.IsChecked == true, PingVolume = pingVolume,
                PrepareAtStart = _prepareAtStart.IsChecked == true
            }).Normalized()
        };
        if (ShortcutRules.FindConflict(candidate) is { } conflict) { Warn(conflict); return; }
        Result = candidate;
        DialogResult = true;
    }

    private void Warn(string message) => MessageBox.Show(this, message, Title, MessageBoxButton.OK, MessageBoxImage.Information);
}
