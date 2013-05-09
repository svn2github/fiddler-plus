using ButtonBarsControl.Control;
using ButtonBarsControl.Design.Enums;

namespace ButtonBarsControl.Design.Entity
{
    /// <summary>
    /// Represents class holding information relatedd to hittest.
    /// </summary>
    public class HitTestInfo
    {
        private readonly HitArea area;
        private readonly int buttonIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="HitTestInfo"/> class. 
        /// </summary>
        /// <param name="buttonIndex">Index of button</param>
        /// <param name="area"><see cref="HitArea"/> representing what was hit area of a given point.</param>
        public HitTestInfo(int buttonIndex, HitArea area)
        {
            this.buttonIndex = buttonIndex;
            this.area = area;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HitTestInfo"/> class. 
        /// </summary>
        /// <param name="area"><see cref="HitArea"/> representing what was hit area of a given point.</param>
        public HitTestInfo(HitArea area)
        {
            buttonIndex = -1;
            this.area = area;
        }

        /// <summary>
        /// Gets Index of <see cref="BarItem"/> in <see cref="ButtonBar"/> as per hitest result. Returs -1 if there is no button at given point.
        /// </summary>
        public int ButtonIndex
        {
            get { return buttonIndex; }
        }

        /// <summary>
        /// <see cref="HitArea"/> representing what was hit area of a given point.
        /// </summary>
        public HitArea Area
        {
            get { return area; }
        }
    }
}