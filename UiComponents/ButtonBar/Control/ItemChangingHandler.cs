using ButtonBarsControl.Design.Generics;

namespace ButtonBarsControl.Control
{
    /// <summary>
    /// Represents a method which will handle <see cref="ButtonBar.ItemsChanging"/>
    /// </summary>
    /// <param name="index">Index of item being changed.</param>
    /// <param name="e">Object that contains event data.</param>
    public delegate void ItemChangingHandler(int index, GenericChangeEventArgs<BarItem> e);
}