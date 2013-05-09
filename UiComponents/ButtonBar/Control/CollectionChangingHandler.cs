using ButtonBarsControl.Design.Generics;

namespace ButtonBarsControl.Control
{
    /// <summary>
    /// Represents a method which will handle <see cref="ButtonBar.ItemsInserting"/> and <see cref="ButtonBar.ItemsRemoving"/>
    /// </summary>
    /// <param name="index">Index of the item in collection.</param>
    /// <param name="value">Collection containing item.</param>
    public delegate void CollectionChangingHandler(int index, GenericCancelEventArgs<BarItem> value);
}