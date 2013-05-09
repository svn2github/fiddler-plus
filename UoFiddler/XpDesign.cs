using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GuiControls;

namespace UoFiddler
{
    public class XpDesign
    {
        public static ControlPanel ControlPanel = null;
        private static UoFiddler   UoFiddler  = null;
        private static TabControl  TabControl = null;
        private static ToolStrip   ToolStrip  = null; 

        public XpDesign(UoFiddler uoFiddler, TabControl tabControl, ToolStrip toolMenu)
        {
            UoFiddler  = uoFiddler;
            TabControl = tabControl;
            ToolStrip  = toolMenu;

            XpDesign.ControlPanel = new ControlPanel(uoFiddler);
            HideTabHeaders(tabControl);
            //TabControl.Resize += new EventHandler(TabControl_Resize);

            string[] files1 = {
                                 "xp.style.glyph1.png",
                                 "xp.style.glyph2.png",
                                 "xp.style.glyph3.png",
                                 "xp.style.glyph4.png"
                              };

            string[] files2 = {
                                 "fiddler.png",
                                 "centred.png",
                                 "runuo.png",
                                 "console.png",

                                 "mul.files.animations.png",
                                 "mul.files.animdata.png",
                                 "mul.files.items.png",
                                 "mul.files.tiledata.png",
                                 "mul.files.landtiles.png",
                                 "mul.files.textures.png",
                                 "mul.files.multis.png",
                                 "mul.files.gumps.png",
                                 "mul.files.sounds.png",
                                 "mul.files.light.png",
                                 "mul.files.hues.png",
                                 "mul.files.fonts.png",
                                 "mul.files.cliloc.png",
                                 "mul.files.speech.png",
                                 "mul.files.skills.png",
                                 "mul.files.skillgrp.png",
                                 "mul.files.map.png",
                                 "mul.files.radarcolor.png",
                                 "mul.files.multimap.png",

                                 "thebox.region.editor.ico"
                              };
            ControlPanel.ImageSet  = Resources.BuildImageSet ("Icons", files1);
            ControlPanel.ImageList = Resources.BuildImageList("Icons", files2);


            ControlPanelGroup group = ControlPanel.AddGroup("Общие", -1);
            group.AddItem(  0, "Главная", "",  "StartPage",   new ItemClickEventHandler(OnSelectItem));
            group.AddItem(  2, "Веб сайт", "", "WebBrowser", new ItemClickEventHandler(OnSelectItem));
            //group.AddItem(  2, "Настройки", "", "Options", new ItemClickEventHandler(OnSelectItem));

            group = ControlPanel.AddGroup("Сервер", -1);//"Администрирование", -1);
            
            //group.AddItem(2, "test 01", "");
            group.AddItem(  3, "Терминал", "", "cmd", new ItemClickEventHandler(OnSelectItem));
            group.AddItem(  1, "CentrEd+", "", "centred", new ItemClickEventHandler(OnSelectItem));
            group.AddItem(  23, "Регионы", "", "regions", new ItemClickEventHandler(OnSelectItem));
            group.SelectItem(null);

            group = ControlPanel.AddGroup("Клиент", -1);//"Файлы клиента", -1);
            //ItemClickEventHandler handler = new ItemClickEventHandler(SelectMulFilesGroup);

            group.AddItem( 20, "Карта", "Просмотр и операции над картами", "map", new ItemClickEventHandler(OnSelectItem));
            group.AddItem(  7, "Тайлы", "Просмотр и редактирование тайлов, текстур и их метаданых", "TileDatas", new ItemClickEventHandler(OnSelectItem));
            group.AddItem(  4, "Анимация", "Просмотр и редактирование анимации и предметов экипировки", "Animation", new ItemClickEventHandler(OnSelectItem));


            //group.AddItem( 21, "Radar Color", "", "RadarCol", new ItemClickEventHandler(OnSelectItem));
            //group.AddItem(  5, "Anim Data", "", "AnimData", new ItemClickEventHandler(OnSelectItem));
            //group.AddItem(  6, "Items", "", "Items", new ItemClickEventHandler(OnSelectItem));
            //group.AddItem(  8, "Land Tiles", "", "LandTiles", new ItemClickEventHandler(OnSelectItem));
            //group.AddItem(  9, "Textures", "", "Texture", new ItemClickEventHandler(OnSelectItem));
            group.AddItem(  10, "Multis", "", "Multis", new ItemClickEventHandler(OnSelectItem));

            
                                    
            group.AddItem( 11, "Gumps", "", "Gumps", new ItemClickEventHandler(OnSelectItem));
            group.AddItem( 12, "Sounds", "", "Sounds", new ItemClickEventHandler(OnSelectItem));
            group.AddItem( 13, "Light", "", "Light", new ItemClickEventHandler(OnSelectItem));
            group.AddItem( 14, "Hue", "", "Hue", new ItemClickEventHandler(OnSelectItem));
            group.AddItem( 15, "Fonts", "", "fonts", new ItemClickEventHandler(OnSelectItem));
            group.AddItem( 16, "CliLoc", "", "CliLoc", new ItemClickEventHandler(OnSelectItem));
            group.AddItem( 17, "Speech", "", "speech", new ItemClickEventHandler(OnSelectItem));
            group.AddItem( 18, "Skills", "", "Skills", new ItemClickEventHandler(OnSelectItem));
            group.AddItem( 19, "Skill Grp", "", "SkillGrp", new ItemClickEventHandler(OnSelectItem));      
            //group.AddItem( 22, "Multi Map", "",  "multimap", new ItemClickEventHandler(OnSelectItem));
            group.SelectItem(null);

            //uoFiddler.Width = 1264;
            //uoFiddler.Height = 948;
			//TabControl.SelectTab(TabControl.TabPages.IndexOfKey("map"));
            //TabControl.SelectTab(TabControl.TabPages.IndexOfKey("TileDatas"));
            //TabControl.SelectTab(TabControl.TabPages.IndexOfKey("CentrEd"));
            //TabControl.SelectTab(TabControl.TabPages.IndexOfKey("CliLoc"));
            //TabControl.SelectTab(TabControl.TabPages.IndexOfKey("Equipment"));
            Application.DoEvents();
        }

        private void OnSelectItem(object sender, ItemClickEventArgs e)
        {
            ToolStrip.Visible = false;
            switch ((string)e.Item.BarItem.Tag)
            {
                case "WebBrowser" : //int index = TabControl.TabPages.IndexOfKey("WebBrowser");
                                    //WebBrowser webBrowser = TabControl.TabPages[index].Browser;
                                    TabControl.SelectTab(TabControl.TabPages.IndexOfKey("WebBrowser"));
                                    //UoFiddler.Browser.Url = new Uri("http://www.uoquint.ru"); 
                                    //UoFiddler.Browser.
                                    break;
                case "StartPage" : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("Start"));
                                   ToolStrip.Visible = true;                                         break;
                case "Options"   : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("Options"));  break;

                case "cmd"      : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("cmd"));       break;
                case "centred"  : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("CentrEd"));   break;
                case "regions"  : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("regions"));   break;

                case "Animation": TabControl.SelectTab(TabControl.TabPages.IndexOfKey("Animation")); break;
                case "AnimData" : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("AnimData"));  break;
                case "Items"    : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("Items"));     break;
                case "TileDatas": TabControl.SelectTab(TabControl.TabPages.IndexOfKey("TileDatas")); break;
                case "LandTiles": TabControl.SelectTab(TabControl.TabPages.IndexOfKey("LandTiles")); break;
                case "Texture"  : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("Texture"));   break;
                case "Multis"   : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("Multis"));    break;
                case "Gumps"    : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("Gumps"));     break;
                case "Sounds"   : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("Sounds"));    break;
                case "Light"    : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("Light"));     break;
                case "Hue"      : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("Hue"));       break;
                case "fonts"    : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("fonts"));     break;
                case "CliLoc"   : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("CliLoc"));    break;
                case "speech"   : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("speech"));    break;
                case "Skills"   : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("Skills"));    break;
                case "SkillGrp" : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("SkillGrp"));  break;
                case "map"      : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("map"));       break;
                case "RadarCol" : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("RadarCol"));  break;
                case "multimap" : TabControl.SelectTab(TabControl.TabPages.IndexOfKey("multimap"));  break;
            }
            //MessageBox.Show("есть");
        }
        
        private void HideTabHeaders(TabControl tabControl)
        {
            tabControl.Appearance = TabAppearance.FlatButtons;
            tabControl.ItemSize = new Size(0, 1);
            tabControl.SizeMode = TabSizeMode.Fixed;
        }

        private void TabControl_Resize(object sender, System.EventArgs e)
        {
            TabPage tabPage = TabControl.SelectedTab;
            Rectangle rect = new Rectangle(tabPage.Left, tabPage.Top, tabPage.Width, tabPage.Height);
            TabControl.Region = new Region(rect);
        }

    }
}
