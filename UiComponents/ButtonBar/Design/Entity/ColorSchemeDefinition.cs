using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ButtonBarsControl.Design.Enums;
using ButtonBarsControl.Design.Utility;
using Microsoft.Win32;

namespace ButtonBarsControl.Design
{
    internal class ColorSchemeDefinition : IDisposable
    {
        private static ColorSchemeDefinition blue;
        private static ColorSchemeDefinition classic;
        private static ColorSchemeDefinition @default;
        private static ColorSchemeDefinition oliveGreen;
        private static ColorSchemeDefinition royale;
        private static ColorSchemeDefinition silver;
        private static ColorSchemeDefinition vS2005;
        private readonly ColorScheme baseColorScheme;
        private ColorScheme colorScheme;
        private Hashtable colorTable;


        protected ColorSchemeDefinition(ColorScheme baseColorScheme)
        {
            this.baseColorScheme = baseColorScheme;
            Initialize();
            SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;
        }

        public virtual ColorPair BarBackStyle
        {
            get { return (ColorPair) colorTable[ColorIndex.BarBackStyle]; }
        }

        public virtual ColorPair BackStyle
        {
            get { return (ColorPair) colorTable[ColorIndex.BackStyle]; }
        }

        public virtual ColorPair ClickStyle
        {
            get { return (ColorPair) colorTable[ColorIndex.ClickStyle]; }
        }

        public virtual Color FocusedBorder
        {
            get { return (Color) colorTable[ColorIndex.FocusedBorder]; }
        }

        public virtual Color BarFocusedBorder
        {
            get { return (Color) colorTable[ColorIndex.BarFocusedBorder]; }
        }

        public virtual Color BarNormalBorder
        {
            get { return (Color) colorTable[ColorIndex.BarNormalBorder]; }
        }

        public virtual Color SelectedBorder
        {
            get { return (Color) colorTable[ColorIndex.SelectedBorder]; }
        }

        public virtual Color DisabledMask
        {
            get { return (Color) colorTable[ColorIndex.DisabledMask]; }
        }

        public virtual int GradientMode
        {
            get { return 90; }
        }

        public virtual Color HoverBorder
        {
            get { return (Color) colorTable[ColorIndex.HoverBorder]; }
        }

        public virtual Color HoverForeGround
        {
            get { return (Color) colorTable[ColorIndex.HoverForeGround]; }
        }

        public virtual ColorPair HoverStyle
        {
            get { return (ColorPair) colorTable[ColorIndex.HoverStyle]; }
        }

        public virtual Color NormalBorder
        {
            get { return (Color) colorTable[ColorIndex.NormalBorder]; }
        }

        public virtual Color NormalForeGround
        {
            get { return (Color) colorTable[ColorIndex.NormalForeGround]; }
        }

        public virtual Color SelectedForeGround
        {
            get { return (Color) colorTable[ColorIndex.SelectedForeGround]; }
        }

        public virtual ColorPair SelectedHoverStyle
        {
            get { return (ColorPair) colorTable[ColorIndex.SelectedHoverStyle]; }
        }

        public virtual ColorPair SelectedStyle
        {
            get { return (ColorPair) colorTable[ColorIndex.SelectedStyle]; }
        }

        public virtual ColorPair DisabledStyle
        {
            get { return (ColorPair) colorTable[ColorIndex.DisabledStyle]; }
        }

        public virtual Color DisabledBorder
        {
            get { return (Color) colorTable[ColorIndex.DisabledBorder]; }
        }

        public virtual Color DisabledForeGround
        {
            get { return (Color) colorTable[ColorIndex.DisabledForeGround]; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ColorScheme BaseColorScheme
        {
            get { return baseColorScheme; }
        }

        public ColorScheme ColorScheme
        {
            get { return colorScheme; }
        }

        private static string CurrentVisualStyleColorScheme
        {
            get
            {
                if (!UXTHEME.IsThemeActive())
                {
                    return null;
                }
                var builder = new StringBuilder(255);
                var builder2 = new StringBuilder(255);
                UXTHEME.GetCurrentThemeName(builder, builder.Capacity, builder2, builder2.Capacity, null, 0);
                return builder2.ToString();
            }
        }

        private static string CurrentVisualStyleThemeFileName
        {
            get
            {
                if (!IsThemeActive)
                {
                    return null;
                }
                var builder = new StringBuilder(255);
                var builder2 = new StringBuilder(255);
                UXTHEME.GetCurrentThemeName(builder, builder.Capacity, builder2, builder2.Capacity, null, 0);
                return builder.ToString();
            }
        }

        public static ColorScheme DefaultColorScheme
        {
            get
            {
                const ColorScheme colorScheme1 = ColorScheme.Classic;
                if (!IsThemeActive)
                {
                    return colorScheme1;
                }
                string themeFile = Path.GetFileName(CurrentVisualStyleThemeFileName);
                string currentTheme = CurrentVisualStyleColorScheme;
                if (string.Compare(themeFile, "LUNA.MSSTYLES", true) != 0)
                {
                    if (string.Compare(themeFile, "Aero.msstyles", true) != 0)
                    {
                        return colorScheme1;
                    }
                    return ColorScheme.Classic;
                }
                if (!string.IsNullOrEmpty(currentTheme))
                {
                    if (string.Compare(themeFile, "HOMESTEAD", true) != 0)
                    {
                        return ColorScheme.OliveGreen;
                    }
                    if (string.Compare(themeFile, "METALLIC", true) != 0)
                    {
                        return ColorScheme.Silver;
                    }
                }
                return ColorScheme.Blue;
            }
        }

        internal static bool IsThemeActive
        {
            get
            {
                if (Environment.OSVersion.Version >= new Version(5, 1))
                {
                    while (OSFeature.Feature.GetVersionPresent(OSFeature.Themes) != null)
                    {
                        return UXTHEME.IsThemeActive();
                    }
                }
                return false;
            }
        }

        public static ColorSchemeDefinition VS2005
        {
            get
            {
                if (vS2005 == null)
                {
                    vS2005 = new ColorSchemeDefinition(ColorScheme.VS2005);
                }
                return vS2005;
            }
        }

        public static ColorSchemeDefinition Classic
        {
            get
            {
                if (classic == null)
                {
                    classic = new ColorSchemeDefinition(ColorScheme.Classic);
                }
                return classic;
            }
        }

        public static ColorSchemeDefinition Default
        {
            get
            {
                if (@default == null)
                {
                    @default = new ColorSchemeDefinition(ColorScheme.Default);
                }
                return @default;
            }
        }

        public static ColorSchemeDefinition Blue
        {
            get
            {
                if (blue == null)
                {
                    blue = new ColorSchemeDefinition(ColorScheme.Blue);
                }
                return blue;
            }
        }

        public static ColorSchemeDefinition OliveGreen
        {
            get
            {
                if (oliveGreen == null)
                {
                    oliveGreen = new ColorSchemeDefinition(ColorScheme.OliveGreen);
                }
                return oliveGreen;
            }
        }

        public static ColorSchemeDefinition Royale
        {
            get
            {
                if (royale == null)
                {
                    royale = new ColorSchemeDefinition(ColorScheme.Royale);
                }
                return royale;
            }
        }

        public static ColorSchemeDefinition Silver
        {
            get
            {
                if (silver == null)
                {
                    silver = new ColorSchemeDefinition(ColorScheme.Silver);
                }
                return silver;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            SystemEvents.UserPreferenceChanged -= OnUserPreferenceChanged;
            colorTable = null;
        }

        #endregion

        private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.Color)
            {
                Initialize();
            }
        }

        private void InitializeCommon()
        {
            colorTable[ColorIndex.ClickStyle] = new ColorPair(Color.FromArgb(251, 147, 64), Color.FromArgb(255, 208, 99));
            colorTable[ColorIndex.SelectedHoverStyle] = new ColorPair(Color.FromArgb(241, 164, 34),
                                                                      Color.FromArgb(251, 231, 156));
            colorTable[ColorIndex.SelectedStyle] = new ColorPair(Color.FromArgb(251, 231, 156),
                                                                 Color.FromArgb(240, 163, 34));
            colorTable[ColorIndex.HoverStyle] = new ColorPair(Color.FromArgb(255, 254, 216),
                                                              Color.FromArgb(255, 216, 106));
        }

        private void InitializeSilver()
        {
            colorTable[ColorIndex.BarBackStyle] = new ColorPair(Color.FromArgb(222, 223, 234),
                                                                Color.FromArgb(152, 150, 179));
            colorTable[ColorIndex.BackStyle] = new ColorPair(Color.FromArgb(222, 223, 234),
                                                             Color.FromArgb(152, 150, 179));
            colorTable[ColorIndex.FocusedBorder] = Color.FromArgb(124, 124, 148);
            colorTable[ColorIndex.SelectedBorder] = Color.FromArgb(124, 124, 148);
            colorTable[ColorIndex.HoverBorder] = Color.FromArgb(124, 124, 148);
            colorTable[ColorIndex.HoverForeGround] = Color.DarkGray;
            colorTable[ColorIndex.NormalBorder] = Color.FromArgb(124, 124, 148);
            colorTable[ColorIndex.NormalForeGround] = Color.Gray;
            colorTable[ColorIndex.SelectedForeGround] = Color.Gray;
            colorTable[ColorIndex.DisabledStyle] = new ColorPair(Color.Transparent, Color.Transparent);
            colorTable[ColorIndex.DisabledBorder] = Color.FromArgb(197, 199, 199);
            colorTable[ColorIndex.DisabledForeGround] = Color.Gray;
            colorTable[ColorIndex.BarNormalBorder] = Color.FromArgb(124, 124, 148);
            colorTable[ColorIndex.BarFocusedBorder] = Color.FromArgb(124, 124, 148);
            colorTable[ColorIndex.DisabledMask] = Color.FromArgb(222, 223, 234);
        }

        private void InitializeRoyale()
        {
            colorTable[ColorIndex.BarBackStyle] = new ColorPair(Color.FromArgb(233, 236, 248),
                                                                Color.FromArgb(218, 221, 225));
            colorTable[ColorIndex.BackStyle] = new ColorPair(Color.FromArgb(233, 236, 248),
                                                             Color.FromArgb(218, 221, 225));
            colorTable[ColorIndex.FocusedBorder] = Color.FromArgb(111, 112, 116);
            colorTable[ColorIndex.SelectedBorder] = Color.FromArgb(111, 112, 116);
            colorTable[ColorIndex.HoverBorder] = Color.FromArgb(111, 112, 116);
            colorTable[ColorIndex.HoverForeGround] = Color.DarkGray;
            colorTable[ColorIndex.NormalBorder] = Color.FromArgb(111, 112, 116);
            colorTable[ColorIndex.NormalForeGround] = Color.Gray;
            colorTable[ColorIndex.SelectedForeGround] = Color.Gray;
            colorTable[ColorIndex.DisabledStyle] = new ColorPair(Color.Transparent, Color.Transparent);
            colorTable[ColorIndex.DisabledBorder] = Color.FromArgb(219, 224, 231);
            colorTable[ColorIndex.DisabledForeGround] = Color.Gray;
            colorTable[ColorIndex.BarNormalBorder] = Color.FromArgb(111, 112, 116);
            colorTable[ColorIndex.BarFocusedBorder] = Color.FromArgb(111, 112, 116);
            colorTable[ColorIndex.DisabledMask] = Color.FromArgb(233, 236, 248);
        }

        private void InitializeOliveGreen()
        {
            colorTable[ColorIndex.BarBackStyle] = new ColorPair(Color.FromArgb(232, 238, 205),
                                                                Color.FromArgb(179, 194, 142));
            colorTable[ColorIndex.BackStyle] = new ColorPair(Color.FromArgb(232, 238, 205),
                                                             Color.FromArgb(179, 194, 142));
            colorTable[ColorIndex.FocusedBorder] = Color.FromArgb(96, 128, 88);
            colorTable[ColorIndex.SelectedBorder] = Color.FromArgb(96, 128, 88);
            colorTable[ColorIndex.HoverBorder] = Color.FromArgb(96, 128, 88);
            colorTable[ColorIndex.HoverForeGround] = Color.DarkGreen;
            colorTable[ColorIndex.NormalBorder] = Color.FromArgb(96, 128, 88);
            colorTable[ColorIndex.NormalForeGround] = Color.DarkGreen;
            colorTable[ColorIndex.SelectedForeGround] = Color.DarkOliveGreen;
            colorTable[ColorIndex.DisabledStyle] = new ColorPair(Color.Transparent, Color.Transparent);
            colorTable[ColorIndex.DisabledBorder] = Color.FromArgb(217, 217, 167);
            colorTable[ColorIndex.DisabledForeGround] = Color.Olive;
            colorTable[ColorIndex.BarNormalBorder] = Color.FromArgb(96, 128, 88);
            colorTable[ColorIndex.BarFocusedBorder] = Color.FromArgb(96, 128, 88);
            colorTable[ColorIndex.DisabledMask] = Color.FromArgb(232, 238, 205);
        }

        private void InitializeBlue()
        {
            colorTable[ColorIndex.BarBackStyle] = new ColorPair(Color.FromArgb(0xdd, 0xec, 0xfe),
                                                                Color.FromArgb(0x81, 0xa9, 0xe2));
            colorTable[ColorIndex.BackStyle] = new ColorPair(Color.FromArgb(0xdd, 0xec, 0xfe),
                                                             Color.FromArgb(0x81, 0xa9, 0xe2));
            colorTable[ColorIndex.FocusedBorder] = Color.Blue;
            colorTable[ColorIndex.SelectedBorder] = Color.Blue;
            colorTable[ColorIndex.HoverBorder] = Color.FromArgb(0xb3, 0xb0, 0xd0);
            colorTable[ColorIndex.HoverForeGround] = Color.Blue;
            colorTable[ColorIndex.NormalBorder] = Color.FromArgb(0xb3, 0xb0, 0xd0);
            colorTable[ColorIndex.NormalForeGround] = Color.Black;
            colorTable[ColorIndex.SelectedForeGround] = Color.DarkBlue;
            colorTable[ColorIndex.DisabledStyle] = new ColorPair(Color.Transparent, Color.Transparent);
            colorTable[ColorIndex.DisabledBorder] = Color.FromArgb(0xb3, 0xb0, 0xd0);
            colorTable[ColorIndex.DisabledForeGround] = Color.Gray;
            colorTable[ColorIndex.BarNormalBorder] = Color.FromArgb(0xb3, 0xb0, 0xd0);
            colorTable[ColorIndex.BarFocusedBorder] = Color.FromArgb(0xb3, 0xb0, 0xd0);
            colorTable[ColorIndex.DisabledMask] = Color.FromArgb(0xdd, 0xec, 0xfe);
        }

        private void InitializeVS2005()
        {
            Color controlLight = SystemColors.ControlLight;
            Color highlight = SystemColors.Highlight;
            Color controlText = SystemColors.ControlText;
            Color inactiveCaptionText = SystemColors.InactiveCaptionText;
            Color gradientInactiveCaption = SystemColors.GradientInactiveCaption;
            Color inactiveCaption = SystemColors.InactiveCaption;

            colorTable[ColorIndex.BarBackStyle] = new ColorPair(controlLight, controlLight);
            colorTable[ColorIndex.BackStyle] = new ColorPair(controlLight, controlLight);
            colorTable[ColorIndex.ClickStyle] = new ColorPair(inactiveCaption, inactiveCaption);
            colorTable[ColorIndex.FocusedBorder] = highlight;
            colorTable[ColorIndex.SelectedBorder] = highlight;
            colorTable[ColorIndex.HoverBorder] = highlight;
            colorTable[ColorIndex.HoverForeGround] = controlText;
            colorTable[ColorIndex.HoverStyle] = new ColorPair(inactiveCaptionText, inactiveCaptionText);
            colorTable[ColorIndex.NormalBorder] = highlight;
            colorTable[ColorIndex.NormalForeGround] = controlText;
            colorTable[ColorIndex.SelectedForeGround] = controlText;
            colorTable[ColorIndex.SelectedHoverStyle] = new ColorPair(inactiveCaptionText, inactiveCaptionText);
            colorTable[ColorIndex.SelectedStyle] = new ColorPair(gradientInactiveCaption, gradientInactiveCaption);
            colorTable[ColorIndex.DisabledStyle] = new ColorPair(controlLight, controlLight);
            colorTable[ColorIndex.DisabledBorder] = highlight;
            colorTable[ColorIndex.DisabledForeGround] = SystemColors.GrayText;
            colorTable[ColorIndex.BarNormalBorder] = highlight;
            colorTable[ColorIndex.BarFocusedBorder] = highlight;
            colorTable[ColorIndex.DisabledMask] = controlLight;
        }

        private void InitializeClassic()
        {
            Color control = SystemColors.Control;
            Color controlText = SystemColors.ControlText;
            Color buttonFace = SystemColors.ButtonFace;
            Color buttonShadow = SystemColors.ButtonShadow;

            colorTable[ColorIndex.BarBackStyle] = new ColorPair(control, control);
            colorTable[ColorIndex.BackStyle] = new ColorPair(buttonFace, buttonFace);
            colorTable[ColorIndex.ClickStyle] = new ColorPair(buttonFace, buttonFace);
            colorTable[ColorIndex.FocusedBorder] = buttonShadow;
            colorTable[ColorIndex.SelectedBorder] = controlText;
            colorTable[ColorIndex.HoverBorder] = buttonShadow;
            colorTable[ColorIndex.HoverForeGround] = controlText;
            colorTable[ColorIndex.HoverStyle] = new ColorPair(buttonFace, buttonFace);
            colorTable[ColorIndex.NormalBorder] = buttonShadow;
            colorTable[ColorIndex.NormalForeGround] = controlText;
            colorTable[ColorIndex.SelectedForeGround] = controlText;
            colorTable[ColorIndex.SelectedHoverStyle] = new ColorPair(buttonFace, buttonFace);
            colorTable[ColorIndex.SelectedStyle] = new ColorPair(buttonFace, buttonFace);
            colorTable[ColorIndex.DisabledStyle] = new ColorPair(buttonFace, buttonFace);
            colorTable[ColorIndex.DisabledBorder] = buttonShadow;
            colorTable[ColorIndex.DisabledForeGround] = SystemColors.GrayText;
            colorTable[ColorIndex.BarNormalBorder] = controlText;
            colorTable[ColorIndex.BarFocusedBorder] = controlText;
            colorTable[ColorIndex.DisabledMask] = control;
        }

        public static ColorSchemeDefinition GetColorScheme(ColorScheme _colorScheme)
        {
            switch (_colorScheme)
            {
                case ColorScheme.Classic:
                    return Classic;

                case ColorScheme.Blue:
                    return Blue;

                case ColorScheme.OliveGreen:
                    return OliveGreen;

                case ColorScheme.Royale:
                    return Royale;

                case ColorScheme.Silver:
                    return Silver;

                case ColorScheme.VS2005:
                    return VS2005;
            }
            return Default;
        }

        private void Initialize()
        {
            colorScheme = baseColorScheme;
            if (colorScheme == ColorScheme.Default)
            {
                colorScheme = DefaultColorScheme;
            }
            colorTable = new Hashtable();
            InitializeCommon();
            switch (colorScheme)
            {
                case ColorScheme.Blue:
                    InitializeBlue();
                    break;

                case ColorScheme.OliveGreen:
                    InitializeOliveGreen();
                    break;

                case ColorScheme.Royale:
                    InitializeRoyale();
                    break;

                case ColorScheme.Silver:
                    InitializeSilver();
                    break;

                case ColorScheme.VS2005:
                    InitializeVS2005();
                    break;

                case ColorScheme.Classic:
                    InitializeClassic();
                    break;
                case ColorScheme.Default:
                    break;
            }
        }

        public static Color CalculateColor(Color color1, Color color2, float percentage)
        {
            var r = (byte) (color1.R - ((color1.R - color2.R)*percentage));
            var g = (byte) (color1.G - ((color1.G - color2.G)*percentage));
            var b = (byte) (color1.B - ((color1.B - color2.B)*percentage));
            return Color.FromArgb(r, g, b);
        }

        #region Nested type: ColorIndex

        private enum ColorIndex
        {
            BarBackStyle = 0,
            BackStyle = 1,
            ClickStyle = 2,
            FocusedBorder = 3,
            SelectedBorder = 4,
            HoverBorder = 5,
            HoverForeGround = 6,
            HoverStyle = 7,
            NormalBorder = 8,
            NormalForeGround = 9,
            SelectedForeGround = 10,
            SelectedHoverStyle = 11,
            SelectedStyle = 12,
            DisabledStyle = 13,
            DisabledBorder = 14,
            DisabledForeGround = 15,
            BarFocusedBorder = 16,
            BarNormalBorder = 17,
            DisabledMask = 18
        }

        #endregion
    }
}