namespace ButtonBarsControl.Control
{
    /// <summary>
    /// Represents a method which will handle <see cref="ButtonBar.ItemsChanged"/>.
    /// </summary>
    /// <param name="index">Index of item</param>
    /// <param name="oldValue">Old value of <see cref="BarItem"/></param>
    /// <param name="newValue">New value of <see cref="BarItem"/></param>
    public delegate void ItemChangeHandler(int index, BarItem oldValue, BarItem newValue);
}