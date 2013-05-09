/***************************************************************************
 *
 * $Author: Turley
 * 
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/

using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

namespace FiddlerControls
{
    public sealed class Options
    {
        private static bool m_UseArtListInItemShow = true;
        private static int m_ArtItemSizeWidth = 48;
        private static int m_ArtItemSizeHeight = 48;
        private static bool m_ArtItemClip = true;
        private static bool m_DesignAlternative = false;
        private static string m_MapCmd = ".goforce";
        // {1} x {2} y {3} z {4} mapid {5} mapname
        private static string m_MapArgs = "{1} {2} {3} {4}";
        private static string[] m_MapNames = { "Felucca", "Trammel", "Ilshenar", "Malas", "Tokuno", "Ter Mur" };
        private static string m_Account = String.Empty;
        private static string m_Password = String.Empty;
        private static bool m_Telnet_active = true;
        private static bool m_Telnet_conl = true;
        private static int m_Telnet_logr = 128;
        private static List<string> m_PluginsToLoad;
        private static Dictionary<string, bool> m_LoadedUltimaClass = new Dictionary<string, bool>()
        {
            #region Инициализация массива 
            {"Animations",false},
            {"Animdata", false},
            {"Art", false},
            {"ASCIIFont", false},
            {"UnicodeFont", false},
            {"Gumps", false},
            {"Hues", false},
            {"Light", false},
            {"Map", false},
            {"Multis", false},
            {"Skills", false},
            {"Sound", false},
            {"Speech", false},
            {"StringList", false},
            {"Texture", false},
            {"TileData", false},
            {"RadarColor",false},
            {"AnimationEdit",false},
            {"SkillGrp",false}
            #endregion
        };

        private static Dictionary<string, bool> m_ChangedUltimaClass = new Dictionary<string, bool>()
        {
            #region Инициализация массива 
            {"Animations",false},
            {"Animdata", false},
            {"Art", false},
            {"ASCIIFont", false},
            {"UnicodeFont", false},
            {"Gumps", false},
            {"Hues", false},
            {"Light", false},
            {"Map", false},
            {"Multis", false},
            {"Skills", false},
            {"Sound", false},
            {"Speech", false},
            {"CliLoc", false},
            {"Texture", false},
            {"TileData", false},
            {"RadarColor",false},
            {"SkillGrp",false}
            #endregion
        };

        private static Dictionary<int, bool> m_ChangedViewStates = new Dictionary<int, bool>()
        {
            #region Инициализация массива 
            {0, true},
            {1, true},
            {2, true},
            {3, true},
            {4, true},
            {5, true},
            {6, true},
            {7, true},
            {8, true},
            {9, true},
            {10, true},
            {11, true},
            {12, true},
            {13, true},
            {14, true},
            {15, true},
            {16, true},
            {17, true},
            {18, true},
            {19, true},
            {20, true}
            #endregion
        };

        #region UOFiddler
        /// <summary>
        /// Отображать список тайлов иконками
        /// </summary>
        public static bool UseArtListInTileShow
        {
            get { return m_UseArtListInItemShow; }
            set { m_UseArtListInItemShow = value; }
        }

        /// <summary>
        /// Definies Element Width in ItemShow
        /// </summary>
        public static int ArtItemSizeWidth
        {
            get { return m_ArtItemSizeWidth; }
            set { m_ArtItemSizeWidth = value; }
        }

        /// <summary>
        /// Definies Element Height in ItemShow
        /// </summary>
        public static int ArtItemSizeHeight
        {
            get { return m_ArtItemSizeHeight; }
            set { m_ArtItemSizeHeight = value; }
        }

        /// <summary>
        /// Definies if Element should be clipped or shrinked in ItemShow
        /// </summary>
        public static bool ArtItemClip
        {
            get { return m_ArtItemClip; }
            set { m_ArtItemClip = value; }
        }

        /// <summary>
        /// Definies if alternative Controls should be used 
        /// </summary>
        public static bool DesignAlternative
        {
            get { return m_DesignAlternative; }
            set { m_DesignAlternative = value; }
        }

        /// <summary>
        /// Definies the cmd to Send Client to Loc
        /// </summary>
        public static string MapCmd
        {
            get { return m_MapCmd; }
            set { m_MapCmd = value; }
        }

        /// <summary>
        /// Definies the args for Send Client
        /// </summary>
        public static string MapArgs
        {
            get { return m_MapArgs; }
            set { m_MapArgs = value; }
        }

        /// <summary>
        /// Definies the MapNames
        /// </summary>
        public static string[] MapNames
        {
            get { return m_MapNames; }
            set { m_MapNames = value; }
        }
        #endregion

        #region UOFiddler+ 
        /// <summary>
        /// Игровой аккаунт с правами не ниже GM
        /// </summary>
        public static string Account
        {
            get { return m_Account; }
            set { m_Account = value; }
        }

        /// <summary>
        /// Пароль от игрового аккаунта
        /// </summary>
        public static string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }

        /// <summary>
        /// Telnet активен (/автоматически подключаться к терминалу при запуске Fiddler+)
        /// </summary>
        public static bool Telnet_active
        {
            get { return m_Telnet_active; }
            set { m_Telnet_active = value; }
        }

        /// <summary>
        /// При подключении к терминалу автоматически включать перенаправление консоли
        /// </summary>
        public static bool Telnet_conl
        {
            get { return m_Telnet_conl; }
            set { m_Telnet_conl = value; }
        }
        
        /// <summary>
        /// Количество строчек получаемых из консоли сервера при первом подключении к терминалу 
        /// </summary>
        public static int Telnet_logr
        {
            get { return m_Telnet_logr; }
            set { m_Telnet_logr = value; }
        } 
        #endregion

        /// <summary>
        /// Definies which Plugins to load on startup
        /// </summary>
        public static List<string> PluginsToLoad
        {
            get { return m_PluginsToLoad; }
            set { m_PluginsToLoad = value; }
        }

        /// <summary>
        /// Definies which muls are loaded
        /// <para>
        /// <list type="bullet">
        /// <item>Animations</item>
        /// <item>Animdata</item>
        /// <item>Art</item>
        /// <item>ASCIIFont</item>
        /// <item>Gumps</item>
        /// <item>Hues</item>
        /// <item>Light</item>
        /// <item>Map</item>
        /// <item>Multis</item>
        /// <item>Skills</item>
        /// <item>Sound</item>
        /// <item>Speech</item>
        /// <item>StringList</item>
        /// <item>Texture</item>
        /// <item>TileData</item>
        /// <item>UnicodeFont</item>
        /// <item>RadarColor</item>
        /// <item>AnimationEdit</item>
        /// </list>
        /// </para>
        /// </summary>
        public static Dictionary<string, bool> LoadedUltimaClass
        {
            get { return m_LoadedUltimaClass; }
            set { m_LoadedUltimaClass = value; }
        }

        /// <summary>
        /// Definies which muls have unsaved changes
        /// <para>
        /// <list type="bullet">
        /// <item>Animations</item>
        /// <item>Animdata</item>
        /// <item>Art</item>
        /// <item>ASCIIFont</item>
        /// <item>Gumps</item>
        /// <item>Hues</item>
        /// <item>Light</item>
        /// <item>Map</item>
        /// <item>Multis</item>
        /// <item>Skills</item>
        /// <item>Sound</item>
        /// <item>Speech</item>
        /// <item>StringList</item>
        /// <item>Texture</item>
        /// <item>TileData</item>
        /// <item>UnicodeFont</item>
        /// <item>RadarColor</item>
        /// </list>
        /// </para>
        /// </summary>
        public static Dictionary<string, bool> ChangedUltimaClass
        {
            get { return Options.m_ChangedUltimaClass; }
            set { Options.m_ChangedUltimaClass = value; }
        }

        /// <summary>
        /// Definies which tabs have been enabled and disabled
        /// </summary>
        public static Dictionary<int, bool> ChangedViewState
        {
            get { return Options.m_ChangedViewStates; }
            set { Options.m_ChangedViewStates = value; }
        }

        public static Icon GetFiddlerIcon()
        {
            return new Icon(System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream("UoFiddler.UOFiddler.ico"));
        }

        public static string AppDataPath { get; set; }
        public static string OutputPath { get; set; }
        public static string ExtractedPath { get; set; }
        public static string ApplicationPath { get; set; }

        public static string ProfileName { get; set; }

        static Options()
        {
            AppDataPath = Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "UoFiddler");
            ApplicationPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            OutputPath = Path.Combine(ApplicationPath, "Output");
            ExtractedPath = Path.Combine(ApplicationPath, "Extracted");
            // AppDomain.CurrentDomain.SetupInformation.ApplicationBase
        }
    }
}
