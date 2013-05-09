using System;
using System.Drawing;
using ButtonBarsControl.Control;
using ButtonBarsControl.Design.Layout;
using ButtonBarsControl.Design.Utility;

namespace ButtonBarsControl.Design.Entity
{
    /// <summary>
    /// Class holding information related to <see cref="ButtonBar.CustomDrawBackGround"/> event.
    /// </summary>
    public class DrawBackGroundEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DrawBackGroundEventArgs"/> with supplied arguments.
        /// </summary>
        /// <param name="graphics">Graphics surface where drawing has to be done.</param>
        /// <param name="bounds"><see cref="ButtonBar"/> control's boundry.</param>
        /// <param name="appearance">Appearance of current <see cref="ButtonBar"/></param>
        /// <param name="bar">Related <see cref="ButtonBar"/></param>
        public DrawBackGroundEventArgs(Graphics graphics, Rectangle bounds, AppearanceBar appearance, ButtonBar bar)
        {
            Graphics = graphics;
            Bounds = bounds;
            Appearance = appearance;
            Bar = bar;
        }

        /// <summary>
        /// Gets or sets drwaing has been done by user or not.
        /// </summary>
        public bool Handeled { get; set; }

        /// <summary>
        /// Gets Graphics surface where drawing has to be done.
        /// </summary>
        public Graphics Graphics { get; private set; }

        /// <summary>
        /// Gets <see cref="ButtonBar"/> control's boundry.
        /// </summary>
        public Rectangle Bounds { get; private set; }

        /// <summary>
        /// Gets appearance of current <see cref="ButtonBar"/>
        /// </summary>
        public AppearanceBar Appearance { get; private set; }

        /// <summary>
        /// Gets related <see cref="ButtonBar"/>
        /// </summary>
        public ButtonBar Bar { get; private set; }

        /// <summary>
        /// Draw control background.
        /// </summary>
        public void DrawBackground()
        {
            PaintUtility.DrawBackground(Graphics, Bounds, Appearance.BackStyle.GetBrush(Bounds),
                                        Appearance.AppearanceBorder.CornerShape, Appearance.CornerRadius, null);
        }

        /// <summary>
        /// Draw control border.
        /// </summary>
        public void DrawBorder()
        {
            if (!Bar.ShowBorders) return;
            var rect = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width - 1, Bounds.Height - 1);
            PaintUtility.DrawBorder(Graphics, rect, Appearance.AppearanceBorder.CornerShape,
                                    Appearance.AppearanceBorder.BorderVisibility,
                                    Appearance.AppearanceBorder.BorderLineStyle, Appearance.CornerRadius,
                                    Bar.Focused ? Appearance.FocusedBorder : Appearance.NormalBorder, null, null);
        }
    }
}