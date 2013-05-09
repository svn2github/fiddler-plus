namespace ButtonBarsControl.Design.Enums
{
    /// <summary>
    /// Enums representing type of change when appearance is changed.
    /// </summary>
    public enum AppearanceAction
    {
        /// <summary>
        /// Appearance was recreated. 
        /// </summary>
        Recreate,
        /// <summary>
        /// Request to paint control.
        /// </summary>
        Repaint,
        /// <summary>
        /// Appearance was updated.
        /// </summary>
        Update
    }
}