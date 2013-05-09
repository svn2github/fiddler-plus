using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using ButtonBarsControl.Design.Enums;
using ButtonBarsControl.Design.Generics;

namespace ButtonBarsControl.Design.Layout
{
    /// <summary>
    /// Class responsible for holding information of Text Appearance
    /// </summary>
    [Serializable, Editor(typeof (AppearenceTextEditor), typeof (UITypeEditor)),
     TypeConverter(typeof (GenericConverter<AppearenceText>))]
    public class AppearenceText : ICloneable, IXmlSerializable
    {
        private StringAlignment alignment = StringAlignment.Center;
        private Font font;
        private StringAlignment lineAlignment = StringAlignment.Near;
        private StringTrimming trimming = StringTrimming.EllipsisCharacter;
        private float xshift;
        private float yshift;

        /// <summary>
        /// Create new instance of the <see cref="AppearenceText"/>.
        /// </summary>
        public AppearenceText()
        {
            Reset();
        }

        /// <summary>
        /// Gets or Sets Line Alignment of text.
        /// </summary>
        public StringAlignment LineAlignment
        {
            get { return lineAlignment; }
            set
            {
                if (lineAlignment == value)
                    return;
                lineAlignment = value;
                OnAppearenceChanged(new GenericEventArgs<AppearanceAction>(AppearanceAction.Repaint));
            }
        }

        /// <summary>
        /// Gets or Sets Image Alignment.
        /// </summary>
        public StringAlignment Alignment
        {
            get { return alignment; }
            set
            {
                if (alignment == value)
                    return;
                alignment = value;
                OnAppearenceChanged(new GenericEventArgs<AppearanceAction>(AppearanceAction.Repaint));
            }
        }

        /// <summary>
        /// Gets or Sets Text Trimming characteristics
        /// </summary>
        public StringTrimming Trimming
        {
            get { return trimming; }
            set
            {
                if (trimming == value)
                    return;
                trimming = value;
                OnAppearenceChanged(new GenericEventArgs<AppearanceAction>(AppearanceAction.Repaint));
            }
        }

        /// <summary>
        /// Gets or Sets X-displacement for the Shadow Text
        /// </summary>
        public float Xshift
        {
            get { return xshift; }
            set
            {
                if (xshift == value)
                    return;
                xshift = value;
                OnAppearenceChanged(new GenericEventArgs<AppearanceAction>(AppearanceAction.Repaint));
            }
        }

        /// <summary>
        /// Gets or Sets Y displacement of the Shadow Text
        /// </summary>
        public float Yshift
        {
            get { return yshift; }
            set
            {
                if (yshift == value)
                    return;
                yshift = value;
                OnAppearenceChanged(new GenericEventArgs<AppearanceAction>(AppearanceAction.Repaint));
            }
        }

        /// <summary>
        /// Gets or Sets Font of the Text.
        /// </summary>
        [NotifyParentProperty(true)]
        public Font Font
        {
            get { return font; }
            set
            {
                if (font == value)
                    return;
                font = value;
                OnAppearenceChanged(new GenericEventArgs<AppearanceAction>(AppearanceAction.Repaint));
            }
        }

        /// <summary>
        /// Indicates current object is Empty or not.
        /// </summary>
        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                return !ShouldSerializeXshift() &&
                       !ShouldSerializeYshift() && !ShouldSerializeAlignment() && !ShouldSerializeFont() &&
                       !ShouldSerializeLineAlignment() && !ShouldSerializeTrimming();
            }
        }

        #region ICloneable Members

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public object Clone()
        {
            var appearence = new AppearenceText
                                 {
                                     Xshift = Xshift,
                                     Yshift = Yshift,
                                     Trimming = Trimming,
                                     Alignment = Alignment,
                                     LineAlignment = LineAlignment,
                                     Font = Font
                                 };
            return appearence;
        }

        #endregion

        /// <summary>
        /// Occurs when properties related to drawing has been modified.
        /// </summary>
        public event GenericEventHandler<AppearanceAction> AppearenceChanged;

        /// <summary>
        /// Get wether default values of the object has been modified.
        /// </summary>
        /// <returns>Returns true if default values has been modified for current object.</returns>
        public virtual bool DefaultChanged()
        {
            return (Xshift != 0f) ||
                   (Yshift != 0f) ||
                   (trimming != StringTrimming.EllipsisCharacter) ||
                   (alignment != StringAlignment.Center) ||
                   (lineAlignment != StringAlignment.Center);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            var app = obj as AppearenceText;
            return ((app != null) && (app.xshift == xshift)) && (app.yshift == yshift);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Fires <see cref="AppearenceChanged"/> event.
        /// </summary>
        /// <param name="e">Object containg event data.</param>
        protected virtual void OnAppearenceChanged(GenericEventArgs<AppearanceAction> e)
        {
            if (AppearenceChanged != null)
            {
                AppearenceChanged(this, e);
            }
        }

        /// <summary>
        /// Compares and indicates supplied <see cref="AppearenceText"/> are same or not. This is done based on value comparison of properties.
        /// </summary>
        /// <param name="p1">First <see cref="AppearenceText "/> to compare</param>
        /// <param name="p2">Second <see cref="AppearenceText"/> to compare</param>
        /// <returns>Returns true if they are equal.</returns>
        public static bool operator ==(AppearenceText p1, AppearenceText p2)
        {
            if (ReferenceEquals(p1, null))
            {
                return ReferenceEquals(p2, null);
            }
            return p1.Equals(p2);
        }

        /// <summary>
        /// Compares and indicates supplied <see cref="AppearenceText"/> are different or not. This is done based on value comparison of properties.
        /// </summary>
        /// <param name="p1">First <see cref="AppearenceText "/> to compare</param>
        /// <param name="p2">Second <see cref="AppearenceText"/> to compare</param>
        /// <returns>Returns true if they are not equal.</returns>
        public static bool operator !=(AppearenceText p1, AppearenceText p2)
        {
            return !(p1 == p2);
        }

        /// <summary>
        /// Resets current object to default values.
        /// </summary>
        public virtual void Reset()
        {
            ResetXshift();
            ResetYshift();
            ResetTrimming();
            ResetAlignment();
            ResetLineAlignment();
            ResetFont();
        }

        private void ResetLineAlignment()
        {
            lineAlignment = StringAlignment.Center;
        }

        private void ResetAlignment()
        {
            alignment = StringAlignment.Center;
        }

        private void ResetXshift()
        {
            xshift = 0f;
        }

        private void ResetYshift()
        {
            yshift = 0f;
        }

        private void ResetTrimming()
        {
            trimming = StringTrimming.EllipsisCharacter;
        }

        private void ResetFont()
        {
            font = new Font("Microsoft Sans Serif", 8.25f);
        }

        private bool ShouldSerializeXshift()
        {
            return (xshift != 0f);
        }

        private bool ShouldSerializeYshift()
        {
            return (yshift != 0f);
        }

        private bool ShouldSerializeTrimming()
        {
            return (trimming != StringTrimming.EllipsisCharacter);
        }

        private bool ShouldSerializeAlignment()
        {
            return (alignment != StringAlignment.Center);
        }

        private bool ShouldSerializeLineAlignment()
        {
            return (lineAlignment != StringAlignment.Center);
        }

        private bool ShouldSerializeFont()
        {
            return !(font.Equals(new Font("Microsoft Sans Serif", 8.25f)));
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return "AppearenceText";
        }

        /// <summary>
        /// Copies values of supplied <see cref="AppearenceText"/> to current object.
        /// </summary>
        /// <param name="appearence"><see cref="AppearenceText"/> object whose value is to be copied.</param>
        public void Assign(AppearenceText appearence)
        {
            alignment = appearence.Alignment;
            font = appearence.Font;
            lineAlignment = appearence.LineAlignment;
            trimming = Trimming;
            xshift = appearence.Xshift;
            yshift = appearence.Yshift;
        }

        #region Implementation of IXmlSerializable

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        public XmlSchema GetSchema()
        {
            return new XmlSchema();
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized. </param>
        public void ReadXml(XmlReader reader)
        {
            var doc = new XmlDocument();
            doc.Load(reader);

            if (doc.GetElementsByTagName("LineAlignment").Count > 0)
                LineAlignment =
                    (StringAlignment)
                    Enum.Parse(typeof (StringAlignment), doc.GetElementsByTagName("LineAlignment")[0].InnerText);
            if (doc.GetElementsByTagName("Alignment").Count > 0)
                Alignment =
                    (StringAlignment)
                    Enum.Parse(typeof (StringAlignment), doc.GetElementsByTagName("Alignment")[0].InnerText);
            if (doc.GetElementsByTagName("Trimming").Count > 0)
                Trimming =
                    (StringTrimming)
                    Enum.Parse(typeof (StringTrimming), doc.GetElementsByTagName("Trimming")[0].InnerText);
            if (doc.GetElementsByTagName("Xshift").Count > 0)
                Xshift = Convert.ToSingle(doc.GetElementsByTagName("Xshift")[0].InnerText);
            if (doc.GetElementsByTagName("Yshift").Count > 0)
                Yshift = Convert.ToSingle(doc.GetElementsByTagName("Yshift")[0].InnerText);
            if (doc.GetElementsByTagName("Font").Count > 0)
            {
                Font =
                    (Font) TypeDescriptor.GetConverter(Font).ConvertFrom(doc.GetElementsByTagName("Font")[0].InnerText);
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized. </param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("LineAlignment", LineAlignment.ToString());
            writer.WriteElementString("Alignment", Alignment.ToString());
            writer.WriteElementString("Trimming", Trimming.ToString());
            writer.WriteElementString("Xshift", Xshift.ToString());
            writer.WriteElementString("Yshift", Yshift.ToString());
            writer.WriteElementString("Font",
                                      TypeDescriptor.GetConverter(Font).ConvertTo(Font, typeof (string)).ToString());
        }

        #endregion

        #region Nested type: AppearenceTextEditor

        private class AppearenceTextEditor : UITypeEditor
        {
            /// <summary>
            /// Indicates whether the specified context supports painting a representation of an object's value within the specified context.
            /// </summary>
            /// <returns>
            /// true if <see cref="M:System.Drawing.Design.UITypeEditor.PaintValue(System.Object,System.Drawing.Graphics,System.Drawing.Rectangle)"/> is implemented; otherwise, false.
            /// </returns>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that can be used to gain additional context information. </param>
            public override bool GetPaintValueSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            /// <summary>
            /// Paints a representation of the value of an object using the specified <see cref="T:System.Drawing.Design.PaintValueEventArgs"/>.
            /// </summary>
            /// <param name="e">A <see cref="T:System.Drawing.Design.PaintValueEventArgs"/> that indicates what to paint and where to paint it. </param>
            public override void PaintValue(PaintValueEventArgs e)
            {
                base.PaintValue(e);
                var app = e.Value as AppearenceText;
                if (app == null)
                    return;
                e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                var format = new StringFormat
                                 {
                                     Trimming = StringTrimming.EllipsisCharacter,
                                     Alignment = StringAlignment.Center,
                                     LineAlignment = StringAlignment.Center
                                 };

                var font = new Font("Microsoft Sans Serif", 8f);
                RectangleF rect = e.Bounds;
                var brush = new SolidBrush(SystemColors.ControlText);
                e.Graphics.DrawString("ab", font, brush, rect, format);
                rect.X -= 1f;
                rect.Y -= 1f;
                brush = new SolidBrush(SystemColors.GrayText);
                e.Graphics.DrawString("ab", font, brush, rect, format);
                brush.Dispose();
            }
        }

        #endregion
    }
}