using System;

namespace FiddlerControls.RegionEditor.Locations
{
	/// <summary>
	/// Summary description for LocOptions.
	/// </summary>
	public class LocOptions
	{
		public bool DrawStatics = true;
		public bool AlwaysOnTop = false;
		public string UOFolder = "";
        public FiddlerControls.RegionEditor.MapViewer.Maps Map = FiddlerControls.RegionEditor.MapViewer.Maps.Sosaria;
		public string GoCommand = "[Go";
		public int ZoomLevel = 0;
		public System.Drawing.Point MapCenter = System.Drawing.Point.Empty;

		public LocOptions()
		{
		}
	}
}
