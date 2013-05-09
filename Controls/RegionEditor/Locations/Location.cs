using System;
using System.Xml;
using System.Drawing;
using FiddlerControls.RegionEditor.MapViewer;


namespace FiddlerControls.RegionEditor.Locations
{
	/// <summary>
	/// Summary description for Location.
	/// </summary>
	public class Loc
	{
		public string Name;
        public Color Color = Color.FromArgb(0x08, 0x08, 0x08);
        public Maps Map = Maps.AllMaps;
		public int X = 0;
		public int Y = 0;
		public int Z = 0;
		public bool IsDefined = false;
	
		public Loc()
		{
		}

		public Loc( string name )
		{
			Name = name;
		}

		public Loc( XmlNode xNode )
		{
            Map = (Maps)System.Convert.ToInt16(xNode.Attributes["map"].Value);
			X = System.Convert.ToInt16( xNode.Attributes["x"].Value );
			Y = System.Convert.ToInt16( xNode.Attributes["y"].Value );
			Z = System.Convert.ToInt16( xNode.Attributes["z"].Value );
            //Color = Color.FromArgb(System.Convert.ToInt32(xNode.Attributes["color"].Value));
            Name = xNode.Attributes["name"].Value;
			IsDefined = true;
		}

		public XmlNode GetXmlNode( XmlDocument dom )
		{
			XmlNode child = dom.CreateNode( XmlNodeType.Element, "child", "" );

			XmlAttribute aName = dom.CreateAttribute( "name" );
			aName.Value = Name;
			child.Attributes.Append( aName );

            //XmlAttribute aMap = dom.CreateAttribute( "map" );
            //aMap.Value = ((int)Map).ToString();
            //child.Attributes.Append( aMap );

			XmlAttribute aX = dom.CreateAttribute( "x" );
			aX.Value = X.ToString();
			child.Attributes.Append( aX );

			XmlAttribute aY = dom.CreateAttribute( "y" );
			aY.Value = Y.ToString();
			child.Attributes.Append( aY );

			XmlAttribute aZ = dom.CreateAttribute( "z" );
			aZ.Value = Z.ToString();
			child.Attributes.Append( aZ );

			return child;
		}
	}
}