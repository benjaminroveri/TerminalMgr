using System.Globalization;

namespace TerminalMgr.Core;

public static class UiLanguage
{
    public static IReadOnlyList<string> Supported { get; } = ["en", "de", "es"];

    public static CultureInfo ResolveCulture(string? preference, string? systemCulture)
    {
        static string? Match(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            string language = name.Split('-')[0];
            return Supported.FirstOrDefault(
                supported => string.Equals(supported, language, StringComparison.OrdinalIgnoreCase));
        }

        string? selected = Match(preference);
        if (selected is null && !string.IsNullOrWhiteSpace(systemCulture))
            selected = Match(systemCulture);

        return CultureInfo.GetCultureInfo(selected ?? "en");
    }
}
