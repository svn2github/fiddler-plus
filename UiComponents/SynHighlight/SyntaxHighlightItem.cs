using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace tevton.SyntaxHighlight {
    [Serializable()]
    public class SyntaxHighlightItem {
        private string name;
        /// <summary>
        /// Distinct name identifying entity. E.g. "keywords", "comments" etc.
        /// </summary>
        public string Name {
            get { return name; }
            set { name = value; }
        }
        private string[] definition;
        /// <summary>
        /// Regex array defining patterns for highlighting entity
        /// </summary>
        public string[] Definition {
            get { return definition; }
            set { definition = value; }
        }

        private Color foregroundColor = Color.Black;
        private Color backgroundColor = Color.White;

        [System.Xml.Serialization.XmlIgnore]//Color is complex type, won't serialize
        public Color ForegroundColor {
            get { return foregroundColor; }
            set { foregroundColor = value; }
        }
        [System.Xml.Serialization.XmlIgnore]
        public Color BackgroundColor {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }
        //introducing int properties instead that will serialize nicely.
        public int ForegroundColorArgb {
            get { return ForegroundColor.ToArgb(); }
            set { ForegroundColor = Color.FromArgb(value); }
        }

        public int BackgroundColorArgb {
            get { return BackgroundColor.ToArgb(); }
            set { BackgroundColor = Color.FromArgb(value); }
        }

        private FontStyle fontStyle = FontStyle.Regular;
        public FontStyle FontStyle {
            get { return fontStyle; }
            set { fontStyle = value; }
        }
        
        private RegexOptions options = new RegexOptions();
        public RegexOptions Options {
            get { return options; }
            set { options = value; }
        }

        [NonSerialized]
        private SyntaxHighlightSegmentList allSegments = null;
        [System.Xml.Serialization.XmlIgnore]
        public SyntaxHighlightSegmentList AllSegments {
            get { return allSegments; }
            set { allSegments = value; }
        }

        public SyntaxHighlightItem(string name, string[] definition, FontStyle fontStyle,
            Color foregroundColor, Color backgroundColor, RegexOptions options) {
            this.Name = name;
            this.Definition = definition;
            this.FontStyle = fontStyle;
            this.ForegroundColor = foregroundColor;
            this.BackgroundColor = backgroundColor;
            this.Options = options;
        }

        public SyntaxHighlightItem(string name, string[] definition, FontStyle fontStyle,
            Color foregroundColor, Color backgroundColor) :
            this(name, definition, fontStyle, foregroundColor, backgroundColor, new RegexOptions()) { }

        public SyntaxHighlightItem(string name, string[] definition, FontStyle fontStyle) :
            this(name, definition, fontStyle, Color.Black, Color.Transparent) { }

        public SyntaxHighlightItem(string name, string[] definition, FontStyle fontStyle,
            RegexOptions options)
            : this(name, definition, fontStyle) {
            Options = options;
        }

        public SyntaxHighlightItem() { }

        public virtual SyntaxHighlightSegmentList FindAllSegments(string text) {
            SyntaxHighlightSegmentList res = new SyntaxHighlightSegmentList();
            foreach(string def in Definition) {
                Regex regex = new Regex(def, Options);
                MatchCollection matches = regex.Matches(text);
                foreach(Match match in matches){
                    res.Add(new SyntaxHighlightSegment(match.Index, match.Index + match.Length));
                }
            }
            res.RemoveOverlappingSegments();
            return res;
        }

        public virtual SyntaxHighlightSegmentList FindAllSegments(string text, bool cached) {
            if(AllSegments == null || !cached) {
                AllSegments = FindAllSegments(text);
            }
            return AllSegments;
        }
    }
}