using System.Runtime.InteropServices;
using System.Text;

namespace ButtonBarsControl.Design.Utility
{
    internal static class UXTHEME
    {
        [DllImport("UxTheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        internal static extern int GetCurrentThemeName(StringBuilder pszThemeFileName, int dwMaxNameChars,
                                                       StringBuilder pszColorBuff, int cchMaxColorChars,
                                                       StringBuilder pszSizeBuff, int cchMaxSizeChars);

        [DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
        internal static extern bool IsThemeActive();
    }
}