using System;
using System.Collections.Generic;
using System.Drawing;

namespace tevton.SyntaxHighlight {
    [Serializable()]
    public class SyntaxHighlightDictionary: List<SyntaxHighlightItem> {
        private Font font = new Font(FontFamily.GenericMonospace, 10);
        /// <summary>
        /// Font to use for highlighting
        /// </summary>
        public Font Font {
            get { return font; }
            set { font = value; }
        }
        private Color foregroundColor = Color.Black;
        private Color backgroundColor = Color.Transparent;
        /// <summary>
        /// Default color for font foreground
        /// </summary>
        public Color ForegroundColor {
            get { return foregroundColor; }
            set { foregroundColor = value; }
        }
        /// <summary>
        /// Default color for text background
        /// </summary>
        public Color BackgroundColor {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        public void CreateSnapshot(string text) {
            foreach(SyntaxHighlightItem snapshot in this) {
                snapshot.FindAllSegments(text, false);
            }
            for(int i = 0; i < this.Count - 1; i++) {
                for(int j = i + 1; j < this.Count; j++) {
                    this[j].AllSegments.RemoveOverlappingSegments(this[i].AllSegments);
                }
            }
        }

        public SyntaxHighlightItem ItemByName(string name, StringComparison comparison) {
            foreach(SyntaxHighlightItem si in this) {
                if(si.Name.Equals(name, comparison))
                    return si;
            }
            return null;
        }

        public SyntaxHighlightItem ItemByName(string name) {
            return ItemByName(name, StringComparison.Ordinal);
        }
    }
}
