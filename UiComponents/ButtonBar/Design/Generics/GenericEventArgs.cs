using System;

namespace ButtonBarsControl.Design.Generics
{
    /// <summary>
    /// Represents a method which can handle generic event with associated object.
    /// </summary>
    /// <typeparam name="T">Generic type which can be associated with the event.</typeparam>
    /// <param name="sender">Event source</param>
    /// <param name="tArgs">Object containing event data</param>
    public delegate void GenericEventHandler<T>(object sender, GenericEventArgs<T> tArgs);

    /// <summary>
    /// Class holding information related to generic event.
    /// </summary>
    /// <typeparam name="T">Generic type which can be associated with the event.</typeparam>
    public class GenericEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Initializes new default instance.
        /// </summary>
        public GenericEventArgs()
        {
            Value = default(T);
        }

        /// <summary>
        /// Initializes new instance with specified parameter.
        /// </summary>
        /// <param name="value">Object to be associated with the event.</param>
        public GenericEventArgs(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Object associated with the event.
        /// </summary>
        public T Value { get; set; }
    }
}