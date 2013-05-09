using System.ComponentModel;

namespace ButtonBarsControl.Design.Generics
{
    /// <summary>
    /// Represents a method which can handle generic cancel event
    /// </summary>
    /// <typeparam name="T">Object type to be associated with the event</typeparam>
    /// <param name="sender">Event source</param>
    /// <param name="tArgs">Object containing event data</param>
    public delegate void GenericCancelEventHandler<T>(object sender, GenericCancelEventArgs<T> tArgs);

    /// <summary>
    /// Class holding information related to generic cancel event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericCancelEventArgs<T> : CancelEventArgs
    {
        /// <summary>
        /// Initailizes new instance with parameters.
        /// </summary>
        /// <param name="value"></param>
        public GenericCancelEventArgs(T value) : base(false)
        {
            Value = value;
        }

        /// <summary>
        /// Initializes new instance with parameter
        /// </summary>
        /// <param name="value">Object associated with event</param>
        /// <param name="cancel">Indicate wether event needs to be cancelled.</param>
        public GenericCancelEventArgs(T value, bool cancel) : base(cancel)
        {
            Value = value;
        }

        /// <summary>
        /// Gets associated object with event.
        /// </summary>
        public T Value { get; set; }
    }
}