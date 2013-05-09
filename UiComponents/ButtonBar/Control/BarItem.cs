using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using ButtonBarsControl.Design.Attributes;
using ButtonBarsControl.Design.Editors;
using ButtonBarsControl.Design.Enums;
using ButtonBarsControl.Design.Generics;
using ButtonBarsControl.Design.Layout;

namespace ButtonBarsControl.Control
{
    /// <summary>
    /// Represents button on <see cref="ButtonBar"/> 
    /// </summary>
    [ToolboxItem(false)]
    public class BarItem : ICloneable
    {
        private readonly AppearanceItem appearance;
        private string caption;
        private bool enabled = true;
        private ItemImageAlignment imageAlignment;
        private int imageIndex;
        private ButtonBar owner;
        private bool selected;
        private ShowBorder showBorder;
        private object tag;
        private string toolTipText;

        /// <summary>
        /// Initializes a new instance of the <see cref="BarItem"/> class. 
        /// </summary>
        public BarItem()
        {
            caption = string.Empty;
            enabled = true;
            Height = 0;
            imageIndex = -1;
            MouseDown = false;
            MouseOver = false;
            owner = null;
            selected = false;
            Top = 0;
            tag = null;
            toolTipText = string.Empty;
            appearance = new AppearanceItem();
            appearance.AppearanceChanged += OnAppearanceChanged;
            showBorder = ShowBorder.Inherit;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarItem"/> class with Specified owner of <see cref="BarItem"/>
        /// </summary>
        /// <param name="owner"></param>
        public BarItem(ButtonBar owner)
        {
            this.owner = owner;
            caption = GetCaption();
            enabled = true;
            Height = 0;
            imageIndex = -1;
            MouseDown = false;
            MouseOver = false;
            selected = false;
            Top = 0;
            tag = null;
            toolTipText = caption;
            appearance = new AppearanceItem();
            appearance.AppearanceChanged += OnAppearanceChanged;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarItem"/> class with Specified Text
        /// </summary>
        /// <param name="text">Text of Button</param>
        public BarItem(string text)
            : this()
        {
            caption = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarItem"/> class with Specified Text
        /// </summary>
        /// <param name="text">Text of Button</param>
        /// <param name="imageIndex"><see cref="ImageIndex"/> of Button.</param>
        public BarItem(string text, int imageIndex)
            : this(text)
        {
            this.imageIndex = imageIndex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarItem"/> class.
        /// </summary>
        /// <param name="text">Text of Button</param>
        /// <param name="imageIndex"><see cref="ImageIndex"/> of Button.</param>
        /// <param name="toolTipText"><see cref="ToolTipText"/> of button.</param>
        public BarItem(string text, int imageIndex, string toolTipText)
            : this(text, imageIndex)
        {
            this.toolTipText = toolTipText;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarItem"/> class.
        /// </summary>
        /// <param name="text">Text of Button</param>
        /// <param name="imageIndex"><see cref="ImageIndex"/> of Button.</param>
        /// <param name="toolTipText"><see cref="ToolTipText"/> of button.</param>
        /// <param name="enabled">Button is enabled or not.</param>
        public BarItem(string text, int imageIndex, string toolTipText, bool enabled)
            : this(text, imageIndex, toolTipText)
        {
            this.enabled = enabled;
        }

        /// <summary>
        /// Gets or Sets caption of <see cref="BarItem"/>
        /// </summary>
        [Localizable(true)]
        public string Caption
        {
            get { return caption; }
            set
            {
                caption = value;
                if (owner != null)
                {
                    owner.RefreshControl();
                }
            }
        }

        /// <summary>
        /// Contains settings related to button Appearance. This overrides global Appearance 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public AppearanceItem Appearance
        {
            get { return appearance; }
        }

        /// <summary>
        /// Gets or Sets Tooltip text of button.
        /// </summary>
        [Localizable(true)]
        public string ToolTipText
        {
            get { return toolTipText; }
            set { toolTipText = value; }
        }

        /// <summary>
        /// Gets or Sets Tag of button. Can be used to hold information.
        /// </summary>
        [TypeConverter(typeof (StringConverter)), Bindable(true), Localizable(false), DefaultValue((string) null)]
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        /// <summary>
        /// Imageindex of button. Uses <see cref="Owner"/> s imagelist to retrieve images.
        /// </summary>
        [DefaultValue(-1)]
        [ImageProperty("Owner.ImageList")]
        [Editor(typeof (ImageListIndexEditor), typeof (UITypeEditor))]
        public int ImageIndex
        {
            get { return imageIndex; }
            set
            {
                imageIndex = value;
                if (owner != null)
                {
                    owner.RefreshControl();
                }
            }
        }

        /// <summary>
        /// Gets or Sets button is enabled or not.
        /// </summary>
        [DefaultValue(true)]
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                if (owner != null)
                {
                    owner.RefreshControl();
                }
            }
        }

        /// <summary>
        /// Owner of button.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ButtonBar Owner
        {
            get { return owner; }
            internal set
            {
                owner = value;
                if (string.IsNullOrEmpty(caption) && string.IsNullOrEmpty(toolTipText))
                {
                    caption = GetCaption();
                    toolTipText = caption;
                }
            }
        }

        internal int Top { get; set; }

        internal int Height { get; set; }

        /// <summary>
        /// Gets or Sets item is selected or not.
        /// </summary>
        [DefaultValue(false)]
        public bool Selected
        {
            get { return selected; }
            set
            {
                if (value == selected)
                    return;
                if (owner == null)
                    return;
                selected = owner.OnSelectItem(this, value);
                owner.RefreshControl();
            }
        }

        /// <summary>
        /// Gets or Sets items image alignment.
        /// </summary>
        public ItemImageAlignment ImageAlignment
        {
            get { return imageAlignment; }
            set
            {
                if (imageAlignment == value)
                    return;
                imageAlignment = value;
                OnAppearanceChanged(this, new GenericEventArgs<AppearanceAction>(AppearanceAction.Recreate));
            }
        }

        /// <summary>
        /// Gets or Sets wether border needs to be shown or not.
        /// </summary>
        public ShowBorder ShowBorder
        {
            get { return showBorder; }
            set
            {
                if (showBorder == value)
                    return;
                showBorder = value;
                OnAppearanceChanged(this, new GenericEventArgs<AppearanceAction>(AppearanceAction.Recreate));
            }
        }

        internal bool MouseDown { get; set; }

        internal bool MouseOver { get; set; }

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
            var myClone = new BarItem(caption, imageIndex, toolTipText, enabled)
                              {Tag = tag, ImageAlignment = imageAlignment, ShowBorder = showBorder};
            myClone.Appearance.Assign(appearance);
            return myClone;
        }

        #endregion

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return caption;
        }

        /// <summary>
        /// This function is called whenever there is change <see cref="Appearance"/>
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="tArgs">Object containing Event data</param>
        protected virtual void OnAppearanceChanged(object sender, GenericEventArgs<AppearanceAction> tArgs)
        {
            if (owner == null)
                return;
            owner.RefreshControl();
        }


        private string GetCaption()
        {
            const string s = "Button ";
            var names = new List<string>();
            for (int i = 0; i < owner.Items.Count; i++)
            {
                names.Add(owner.Items[i].Caption);
            }
            bool found;
            int j = 0;
            do
            {
                j++;
                found = names.IndexOf(s + j) >= 0;
            } while (found);
            return s + j;
        }

        /// <summary>
        /// Resets <see cref="ImageAlignment"/> property
        /// </summary>
        protected void ResetImageAlignment()
        {
            imageAlignment = ItemImageAlignment.Inherit;
        }

        /// <summary>
        /// Resets <see cref="ShowBorder"/> property
        /// </summary>
        protected void ResetShowBorder()
        {
            ShowBorder = ShowBorder.Inherit;
        }

        /// <summary>
        /// Indicates wether <see cref="ImageAlignment"/> needs to be serialized.
        /// </summary>
        /// <returns></returns>
        protected bool ShouldSerializeShowBorder()
        {
            return ShowBorder != ShowBorder.Inherit;
        }
    }
}