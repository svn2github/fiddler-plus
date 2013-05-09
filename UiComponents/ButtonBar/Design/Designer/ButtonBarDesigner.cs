using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml;
using System.Xml.Serialization;
using ButtonBarsControl.Control;
using ButtonBarsControl.Design.Editors;
using ButtonBarsControl.Design.Enums;
using ButtonBarsControl.Design.Generics;
using ButtonBarsControl.Design.Layout;
using ButtonBarsControl.Properties;
using Appearance=ButtonBarsControl.Design.Layout.Appearance;

namespace ButtonBarsControl.Design.Designer
{
    internal class ButtonBarDesigner : ControlDesigner
    {
        private readonly DesignerActionListCollection actionListCollection = new DesignerActionListCollection();
        private ButtonBar buttonBar;
        private ButtonBarDesignerActionList designerActionList;

        /// <summary>
        /// Gets the design-time action lists supported by the component associated with the designer.
        /// </summary>
        /// <returns>
        /// The design-time action lists supported by the component associated with the designer.
        /// </returns>
        public override DesignerActionListCollection ActionLists
        {
            get { return actionListCollection; }
        }

        private DesignerActionUIService Service
        {
            get { return GetService(typeof (DesignerActionUIService)) as DesignerActionUIService; }
        }

        /// <summary>
        /// Indicates whether a mouse click at the specified point should be handled by the control.
        /// </summary>
        /// <returns>
        /// true if a click at the specified point is to be handled by the control; otherwise, false.
        /// </returns>
        /// <param name="point">A <see cref="T:System.Drawing.Point"/> indicating the position at which the mouse was clicked, in screen coordinates. </param>
        protected override bool GetHitTest(Point point)
        {
            var test = buttonBar.HitTest(buttonBar.PointToClient(point));
            return test.ButtonIndex >= 0;
        }

        /// <summary>
        /// Initializes the designer with the specified component.
        /// </summary>
        /// <param name="component">The <see cref="T:System.ComponentModel.IComponent"/> to associate with the designer. </param>
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            buttonBar = (ButtonBar) component;
            buttonBar.ThemeProperty.ThemeChanged += delegate { RefreshComponent(); };
            buttonBar.SelectionChanged += delegate { RefreshComponent(); };
            buttonBar.ItemsChanging += delegate { RefreshComponent(); };
            buttonBar.ItemsClearing += delegate { RefreshComponent(); };
            buttonBar.ItemsInserting += delegate { RefreshComponent(); };
            buttonBar.ItemsRemoving += delegate { RefreshComponent(); };
            buttonBar.ItemClick += delegate { RefreshComponent(); };
            buttonBar.Appearance.Bar.AppearanceChanged += delegate { RefreshComponent(); };
            buttonBar.Appearance.Item.AppearanceChanged += delegate { RefreshComponent(); };
            designerActionList = new ButtonBarDesignerActionList(buttonBar);
            actionListCollection.Add(designerActionList);
        }

        internal void RefreshComponent()
        {
            if (Service != null)
                Service.Refresh(Control);
        }
    }

    internal class ButtonBarDesignerActionList : DesignerActionList
    {
        public ButtonBarDesignerActionList(IComponent component) : base(component)
        {
        }

        protected virtual ButtonBar ButtonBar
        {
            get { return (ButtonBar) Component; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the smart tag panel should automatically be displayed when it is created.
        /// </summary>
        /// <returns>
        /// true if the panel should be shown when the owning component is created; otherwise, false. The default is false.
        /// </returns>
        public override bool AutoShow
        {
            get { return true; }
            set { base.AutoShow = value; }
        }

        private DesignerActionUIService DesignerActionUIService
        {
            get { return GetService(typeof (DesignerActionUIService)) as DesignerActionUIService; }
        }

        public GenericCollection<BarItem> EditItems
        {
            get { return ButtonBar.Items; }
        }

        public ColorScheme Theme
        {
            get { return ButtonBar.ThemeProperty.ColorScheme; }
            set
            {
                if (ButtonBar.ThemeProperty.ColorScheme == value) return;
                ButtonBar.ThemeProperty.ColorScheme = value;
                ButtonBar.ThemeProperty.ColorScheme = value;
                ButtonBar.Invalidate();
            }
        }

        public int ButtonWidth
        {
            get { return ButtonBar.ButtonWidth; }
            set
            {
                if (ButtonBar.ButtonWidth == value) return;
                ButtonBar.ButtonWidth = value;
                ButtonBar.Invalidate();
            }
        }

        /// <summary>
        /// Returns the collection of <see cref="T:System.ComponentModel.Design.DesignerActionItem"/> objects contained in the list.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.Design.DesignerActionItem"/> array that contains the items in this list.
        /// </returns>
        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var items = new DesignerActionItemCollection();
            items.Add(new DesignerActionHeaderItem("Design", "Appearance"));
            items.Add(new DesignerActionPropertyItem("Theme", "Theme", "Appearance"));
            if (!ButtonBar.ShowBorders)
                items.Add(new DesignerActionMethodItem(this, "ShowBorders", "Show Borders", "Appearance", true));
            else
                items.Add(new DesignerActionMethodItem(this, "ShowBorders", "Hide Borders", "Appearance", true));

            items.Add(new DesignerActionPropertyItem("ButtonWidth", "ButtonWidth", "Appearance"));
            items.Add(new DesignerActionMethodItem(this, "ApplyTemplate", "Apply Template", "Appearance", true));

            items.Add(new DesignerActionHeaderItem("Collection", "Collection"));
            items.Add(new DesignerActionPropertyItem("EditItems", "Edit Buttons", "Collection"));
            if (ButtonBar.Items.Count > 0)
                items.Add(new DesignerActionMethodItem(this, "ClearButtons", "Clear Buttons", "Collection", true));
            items.Add(new DesignerActionMethodItem(this, "AddButton", "Add Button", "Collection", true));
            if (ButtonBar.Items.Count > 0)
                items.Add(new DesignerActionMethodItem(this, "DeleteButton", "Delete Button", "Collection", true));

            items.Add(new DesignerActionHeaderItem("Export", "Export"));
            items.Add(new DesignerActionMethodItem(this, "Export", "Save Appearance", "Export", true));
            items.Add(new DesignerActionMethodItem(this, "Import", "Load Appearance", "Export", true));

            return items;
        }

        internal void RefreshComponent()
        {
            if (DesignerActionUIService != null)
                DesignerActionUIService.Refresh(ButtonBar);
        }

        protected virtual void ShowBorders()
        {
            ButtonBar.ShowBorders = !ButtonBar.ShowBorders;
            ButtonBar.Invalidate();
            RefreshComponent();
        }

        protected virtual void ClearButtons()
        {
            ButtonBar.Items.Clear();
            ButtonBar.RefreshControl();
            ButtonBar.Invalidate();
            RefreshComponent();
        }

        protected virtual void ApplyTemplate()
        {
            var editor = new AppearanceEditor.AppearanceEditorUI(ButtonBar);
            editor.ShowDialog();
        }

        protected virtual void AddButton()
        {
            var item = new BarItem(ButtonBar);
            ButtonBar.Items.Add(item);
            ButtonBar.RefreshControl();
            ButtonBar.Invalidate();
            RefreshComponent();
        }

        protected virtual void DeleteButton()
        {
            if (ButtonBar.SelectedItem == null) return;
            ButtonBar.Items.Remove(ButtonBar.SelectedItem);
            if (ButtonBar.Items.Count > 0)
            {
                ButtonBar.Items[0].Selected = true;
            }
            ButtonBar.RefreshControl();
            ButtonBar.Invalidate();
            RefreshComponent();
        }

        protected virtual void Export()
        {
            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = Resources.XML_FILE;
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                using (XmlWriter writer = new XmlTextWriter(dlg.FileName, Encoding.UTF8))
                {
                    var serializer = new XmlSerializer(typeof(Appearance));
                    serializer.Serialize(writer, ButtonBar.Appearance);
                    writer.Flush();
                    writer.Close();
                }
            }
        }

        protected virtual void Import()
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = Resources.XML_FILE;
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                using (var fs = new FileStream(dlg.FileName, FileMode.Open))
                {
                    var serializer = new XmlSerializer(typeof(Appearance));
                    var app = (Layout.Appearance)serializer.Deserialize(fs);
                    ButtonBar.Appearance.Assign(app);
                    ButtonBar.SetThemeDefaults();
                    ButtonBar.Refresh();
                }
            }
        }
    }
}