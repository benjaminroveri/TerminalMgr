using System.Text.Json;
using System.Globalization;
using System.IO;
using System.Windows;
using TerminalMgr.App.Resources;
using TerminalMgr.Core;

namespace TerminalMgr.App;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        var store = new SettingsStore(AppPaths.DataDirectory());
        AppSettings settings;
        try
        {
            settings = store.Load();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
        {
            settings = new AppSettings();
        }

        AppLocalization.SetCulture(UiLanguage.ResolveCulture(settings.UiLanguage, CultureInfo.CurrentUICulture.Name));
        base.OnStartup(e);
        var window = new MainWindow();
        MainWindow = window;
        window.Show();
    }
}
