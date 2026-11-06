using System.Globalization;
using System.Resources;

namespace TerminalMgr.App.Resources
{
    public static class AppLocalization
    {
        public static CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;
        public static string Get(string key) => string.Empty;
    }
}
