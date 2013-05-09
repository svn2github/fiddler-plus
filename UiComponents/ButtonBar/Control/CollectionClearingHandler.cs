using ButtonBarsControl.Design.Generics;

namespace ButtonBarsControl.Control
{
    /// <summary>
    /// Represents a method which will handle <see cref="ButtonBar.ItemsClearing"/>
    /// </summary>
    /// <param name="value">Object that contains event data.</param>
    public delegate void CollectionClearingHandler(GenericCancelEventArgs<GenericCollection<BarItem>> value);
}