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

using System.Windows.Forms;
using Ultima;

namespace UoFiddler
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
            this.Icon = FiddlerControls.Options.GetFiddlerIcon();

            checkBoxAltDesign.Checked = FiddlerControls.Options.DesignAlternative;
            checkBoxCacheData.Checked = Files.CacheData;

			checkBoxQuintMaps.Checked = (Map.Dangeon.Width == 7168 && Map.Sosaria.Width == 12288);	
            if (!checkBoxQuintMaps.Checked) {
				checkBoxPreAlphaMapSize.Checked = (Map.Britania.Width == 1024);
				checkBoxNewMapSize.Checked = (Map.Felucca.Width == 7168 && Map.Trammel.Width == 7168);
                checkBoxuseDiff.Checked = Map.UseDiff;   
            }
            numericUpDownItemSizeWidth.Value = FiddlerControls.Options.ArtItemSizeWidth;
            numericUpDownItemSizeHeight.Value = FiddlerControls.Options.ArtItemSizeHeight;
            checkBoxItemClip.Checked = FiddlerControls.Options.ArtItemClip;
            checkBoxUseHash.Checked = Files.UseHashFile;
            map0Nametext.Text = FiddlerControls.Options.MapNames[0];
            map1Nametext.Text = FiddlerControls.Options.MapNames[1];
            map2Nametext.Text = FiddlerControls.Options.MapNames[2];
            map3Nametext.Text = FiddlerControls.Options.MapNames[3];
            map4Nametext.Text = FiddlerControls.Options.MapNames[4];
            map5Nametext.Text = FiddlerControls.Options.MapNames[5];
            cmdtext.Text = FiddlerControls.Options.MapCmd;
            argstext.Text = FiddlerControls.Options.MapArgs;
        }

        private void checkBoxQuintMaps_CheckedChanged(object sender, System.EventArgs e)
        {
			checkBoxPreAlphaMapSize.Enabled = !checkBoxQuintMaps.Checked;
            checkBoxNewMapSize.Enabled = !checkBoxQuintMaps.Checked;
            checkBoxuseDiff.Enabled = !checkBoxQuintMaps.Checked;
            map3Nametext.Enabled = !checkBoxQuintMaps.Checked;
            map4Nametext.Enabled = !checkBoxQuintMaps.Checked;
            map5Nametext.Enabled = !checkBoxQuintMaps.Checked;
            /*if (checkBoxQuintMaps.Checked) {
				checkBoxPreAlphaMapSize.Checked = false;
                checkBoxNewMapSize.Checked = false;
                checkBoxuseDiff.Checked = false;
            } else {
				checkBoxPreAlphaMapSize.Checked = (Map.Felucca.Width == 1024);
                checkBoxNewMapSize.Checked = (Map.Felucca.Width == 7168);
                checkBoxuseDiff.Checked = Map.UseDiff;
            }*/
            }

		private void checkBoxNewMapSize_CheckedChanged(object sender, System.EventArgs e)
            {
			checkBoxPreAlphaMapSize.Enabled = !checkBoxNewMapSize.Checked;
			checkBoxQuintMaps.Enabled = !checkBoxNewMapSize.Checked;
			map5Nametext.Enabled = checkBoxNewMapSize.Checked;
		}

		private void checkBoxPreAlphaMapSize_CheckedChanged(object sender, System.EventArgs e)
		{
			checkBoxQuintMaps.Enabled = !checkBoxPreAlphaMapSize.Checked;
			checkBoxNewMapSize.Enabled = !checkBoxPreAlphaMapSize.Checked;
			checkBoxuseDiff.Enabled = !checkBoxPreAlphaMapSize.Checked;
			map1Nametext.Enabled = !checkBoxPreAlphaMapSize.Checked;
			map2Nametext.Enabled = !checkBoxPreAlphaMapSize.Checked;
			map3Nametext.Enabled = !checkBoxPreAlphaMapSize.Checked;
			map4Nametext.Enabled = !checkBoxPreAlphaMapSize.Checked;
			map5Nametext.Enabled = !checkBoxPreAlphaMapSize.Checked;
			/*if (checkBoxQuintMaps.Checked) {
				checkBoxNewMapSize.Checked = false;
				checkBoxuseDiff.Checked = false;
				checkBoxPreAlphaMapSize.Checked = false;
			} else {
				checkBoxQuintMaps.Checked = (Map.Sosaria.Width == 12288);
                checkBoxNewMapSize.Checked = (Map.Felucca.Width == 7168);
                checkBoxuseDiff.Checked = Map.UseDiff;
			}*/
        }

        private void OnClickApply(object sender, System.EventArgs e)
        {
            if (checkBoxAltDesign.Checked != FiddlerControls.Options.DesignAlternative)
            {
                FiddlerControls.Options.DesignAlternative = checkBoxAltDesign.Checked;
                UoFiddler.ChangeDesign();
                PluginInterface.Events.FireDesignChangeEvent();
            }

            //Files.CacheData = checkBoxQuintMaps.Checked;
            if (checkBoxQuintMaps.Checked != ((Map.Dangeon.Width == 7168) & (Map.Sosaria.Width == 12288))) {
                if (checkBoxQuintMaps.Checked) {
                    Map.Dangeon.Width  = 7168;
                    Map.Dangeon.Height = 4096;
                    Map.Sosaria.Width  = 12288;
                    Map.Sosaria.Height = 8192;
                    UoFiddler.ChangeMapSize();
                    Map.UseDiff = false;
                    FiddlerControls.Events.FireMapDiffChangeEvent();
                }
            }
            if (!checkBoxQuintMaps.Checked)
            {
                Files.CacheData = checkBoxCacheData.Checked;
                if (checkBoxNewMapSize.Checked != ((Map.Felucca.Width == 7168) & (Map.Trammel.Width == 7168)) ||
					checkBoxPreAlphaMapSize.Checked != (Map.Britania.Width == 1024) || Map.Trammel.Width == 12288) {
                    if (checkBoxNewMapSize.Checked) {
                        Map.Felucca.Width  = 7168;
                        Map.Felucca.Height = 4096;
                        Map.Trammel.Width  = 7168;
                        Map.Trammel.Height = 4096;
                    } else if (checkBoxPreAlphaMapSize.Checked) {
						Map.Britania.Width  = 1024;
						Map.Britania.Height = 1024;
					} else {
                        Map.Felucca.Width  = 6144;
                        Map.Felucca.Height = 4096;
                        Map.Trammel.Width  = 6144;
                        Map.Trammel.Height = 4096;
                    }
                    UoFiddler.ChangeMapSize();
                }
                if (checkBoxuseDiff.Checked != Map.UseDiff)
                {
                    Map.UseDiff = checkBoxuseDiff.Checked;
                    FiddlerControls.Events.FireMapDiffChangeEvent();
                }
            }
            if ((numericUpDownItemSizeWidth.Value != FiddlerControls.Options.ArtItemSizeWidth)
                || (numericUpDownItemSizeHeight.Value != FiddlerControls.Options.ArtItemSizeHeight))
            {
                FiddlerControls.Options.ArtItemSizeWidth = (int)numericUpDownItemSizeWidth.Value;
                FiddlerControls.Options.ArtItemSizeHeight = (int)numericUpDownItemSizeHeight.Value;
                UoFiddler.ReloadItemTab();
            }
            if (checkBoxItemClip.Checked != FiddlerControls.Options.ArtItemClip)
            {
                FiddlerControls.Options.ArtItemClip = checkBoxItemClip.Checked;
                UoFiddler.ReloadItemTab();
            }
            Files.UseHashFile = checkBoxUseHash.Checked;

            if ((map0Nametext.Text != FiddlerControls.Options.MapNames[0])
                || (map1Nametext.Text != FiddlerControls.Options.MapNames[1])
                || (map2Nametext.Text != FiddlerControls.Options.MapNames[2])
                || (map3Nametext.Text != FiddlerControls.Options.MapNames[3])
                || (map4Nametext.Text != FiddlerControls.Options.MapNames[4])
                || (map5Nametext.Text != FiddlerControls.Options.MapNames[5]))
            {
                FiddlerControls.Options.MapNames[0] = map0Nametext.Text;
                FiddlerControls.Options.MapNames[1] = map1Nametext.Text;
                FiddlerControls.Options.MapNames[2] = map2Nametext.Text;
                FiddlerControls.Options.MapNames[3] = map3Nametext.Text;
                FiddlerControls.Options.MapNames[4] = map4Nametext.Text;
                FiddlerControls.Options.MapNames[5] = map5Nametext.Text;
                FiddlerControls.Events.FireMapNameChangeEvent();
            }
            FiddlerControls.Options.MapCmd = cmdtext.Text;
            FiddlerControls.Options.MapArgs = argstext.Text;
        }

		

		
    }
}
