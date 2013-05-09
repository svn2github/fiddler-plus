using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using ButtonBarsControl.Design.Attributes;
using ButtonBarsControl.Design.Editors;
using ButtonBarsControl.Design.Entity;
using ButtonBarsControl.Design.Enums;
using ButtonBarsControl.Design.Generics;
using Appearance=ButtonBarsControl.Design.Layout.Appearance;

namespace ButtonBarsControl.Control
{
    partial class ButtonBar
    {
        #region Public properties

        /// <summary>
        /// Imagelist containing images for buttons.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(null)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public ImageList ImageList
        {
            get { return imageList; }
            set
            {
                imageList = value;
                RefreshControl();
            }
        }

        /// <summary>
        /// Property used to Get or Set which theme should be used. See also <see cref="Design.Entity.ThemeProperty"/>
        /// </summary>
        [Category("Appearance")]
        [TypeConverter(typeof (GenericConverter<ThemeProperty>))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ThemeProperty ThemeProperty
        {
            get { return themeProperty; }
        }

        /// <summary>
        /// Collections of buttons.
        /// </summary>
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("The collection of list items for the control.")]
        public GenericCollection<BarItem> Items
        {
            get { return items; }
        }

        /// <summary>
        /// Indicates wether to use shortcut keys or not.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool UseMnemonic
        {
            get { return useMnemonic; }
            set
            {
                if (useMnemonic != value)
                {
                    useMnemonic = value;
                    RefreshControl();
                }
            }
        }

        /// <summary>
        /// Width of buttons. Control resizes itself to button.
        /// </summary>
        [Category("Appearance")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue(120)]
        public int ButtonWidth
        {
            get { return buttonWidth; }
            set
            {
                if (buttonWidth != value)
                {
                    buttonWidth = value;
                    RefreshControl();
                }
            }
        }

        /// <summary>
        /// Get current selected button.
        /// </summary>
        [Category("Appearance")]
        [TypeConverter(typeof (ReadOnlyConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BarItem SelectedItem
        {
            get
            {
                BarItem barItem = null;
                foreach (BarItem item in items)
                {
                    if (item.Selected)
                    {
                        barItem = item;
                        break;
                    }
                }
                return barItem;
            }
        }

        /// <summary>
        /// <see cref="Design.Layout.Appearance"/> object containing Global Appearance information.
        /// </summary>
        [Category("Appearance")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [TypeConverter(typeof (GenericConverter<Appearance>))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof (AppearanceEditor), typeof (UITypeEditor))]
        public Appearance Appearance
        {
            get { return appearance; }
        }

        /// <summary>
        /// Gets or Sets wether borders will be shown or not.
        /// </summary>
        [Category("Appearance")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue(true)]
        public bool ShowBorders
        {
            get { return showBorders; }
            set
            {
                if (showBorders == value)
                    return;
                showBorders = value;
                RefreshControl();
            }
        }

        //[Category("209, 212, 215")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [TypeConverter(typeof (ReadOnlyConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        internal Appearance CurrentAppearance
        {
            get { return currentAppearance; }
        }

        /// <summary>
        /// Gets or Sets Background Image transparency.
        /// </summary>
        [Category("Appearance")]
        [Editor(typeof (RangeEditor), typeof (UITypeEditor))]
        [MinMax(0, 100)]
        [DefaultValue(90)]
        public int ImageTransparency
        {
            get { return imageTransparency; }
            set
            {
                if (imageTransparency == value)
                    return;
                imageTransparency = value;
                RefreshControl();
            }
        }

        /// <summary>
        /// Gets or Sets Disabled State Transparency.
        /// </summary>
        [Category("Appearance")]
        [Editor(typeof (RangeEditor), typeof (UITypeEditor))]
        [MinMax(0, 100)]
        [DefaultValue(20)]
        public int DisableTransparency
        {
            get { return disableTransparency; }
            set
            {
                if (disableTransparency == value)
                    return;
                disableTransparency = value;
                RefreshControl();
            }
        }

        /// <summary>
        /// Gets or Sets Global image alignment of button images.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(ImageAlignment.Top)]
        public ImageAlignment ImageAlignment
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

#pragma warning disable 0809
        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Drawing.Color"/> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor"/> property.
        /// </returns>
        /// <filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [Obsolete("This property is not relevent.")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color BackColor
#pragma warning restore 0809
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

#pragma warning disable 0809
        /// <summary>
        /// Gets or sets a value indicating whether the container enables the user to scroll to any controls placed outside of its visible boundaries.
        /// </summary>
        /// <returns>
        /// true if the container enables auto-scrolling; otherwise, false. The default value is false. 
        /// </returns>
        /// <filterpriority>1</filterpriority>
        [Obsolete("This property is not relevent.")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool AutoScroll
        {
#pragma warning restore 0809
            get
            {
                return base.AutoScroll;
            }
            set
            {
                base.AutoScroll = value;
            }
        }

        /// <summary>
        /// Gets or sets the text associated with this control. Thsi property is not relevent with this control.
        /// </summary>
        /// <returns>
        /// The text associated with this control.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        [Browsable(false)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private Graphics Graphics
        {
            get
            {
                if (graphics == null)
                    CreateMemoryBitmap();
                return graphics;
            }
        }

        /// <summary>
        /// Gets or Sets distance between buttons.
        /// </summary>
        [Category("Appearance")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue(4)]
        public int Spacing
        {
            get { return spacing; }
            set
            {
                if (spacing == value)
                    return;
                spacing = value;
                RefreshControl();
            }
        }

        #endregion

        /// <summary>
        /// Resets Theme Property to Default Value.
        /// </summary>
        protected void ResetThemeProperty()
        {
            ThemeProperty.Reset();
        }

        /// <summary>
        /// Resets <see cref="Design.Layout.Appearance.Item"/> to default value.
        /// </summary>
        protected void ResetItemStyle()
        {
            appearance.Item.Assign(DEFAULT.Item);
        }

        /// <summary>
        /// Resets <see cref="Design.Layout.Appearance.Bar"/> to default value.
        /// </summary>
        protected void ResetBarStyle()
        {
            appearance.Bar.Assign(DEFAULT.Bar);
        }

        /// <summary>
        /// Indicates wether <see cref="ThemeProperty"/> needs to be serialized by designer or not.
        /// </summary>
        /// <returns>true if designer needs to serialize</returns>
        protected bool ShouldSerializeThemeProperty()
        {
            return ThemeProperty.DefaultChanged();
        }

        /// <summary>
        /// Indicates wether <see cref="Design.Layout.Appearance.Item"/> needs to be serialized by designer or not.
        /// </summary>
        /// <returns>true if designer needs to serialize</returns>
        protected bool ShouldSerializeItemStyle()
        {
            return appearance.Item != DEFAULT.Item;
        }

        /// <summary>
        /// Indicates wether <see cref="Design.Layout.Appearance.Bar"/> needs to be serialized by designer or not.
        /// </summary>
        /// <returns>true if designer needs to serialize</returns>
        protected bool ShouldSerializeBarStyle()
        {
            return appearance.Bar != DEFAULT.Bar;
        }
    }
}