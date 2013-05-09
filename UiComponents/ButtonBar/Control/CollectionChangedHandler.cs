namespace ButtonBarsControl.Control
{
    /// <summary>
    /// Represents a method which will handle <see cref="ButtonBar.ItemsInserted"/> and <see cref="ButtonBar.ItemsRemoved"/>
    /// </summary>
    /// <param name="index">Index of item.</param>
    /// <param name="value">Associated <see cref="BarItem"/></param>
    public delegate void CollectionChangedHandler(int index, BarItem value);
}