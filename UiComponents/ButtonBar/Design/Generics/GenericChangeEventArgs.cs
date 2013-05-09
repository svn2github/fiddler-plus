using System.ComponentModel;

namespace ButtonBarsControl.Design.Generics
{
    /// <summary>
    /// Represents a method which can be used for Change events which will have <see cref="GenericChangeEventArgs{T}.OldValue"/>, <see cref="GenericChangeEventArgs{T}.NewValue"/> and <see cref="CancelEventArgs.Cancel"/>.
    /// </summary>
    /// <typeparam name="T">Type of object to be used for.</typeparam>
    /// <param name="sender">sender of Event.</param>
    /// <param name="e">Object containing event data</param>
    public delegate void GenericValueChangingHandler<T>(object sender, GenericChangeEventArgs<T> e);

    /// <summary>
    /// Place holder for change event containing Old and New Value.
    /// </summary>
    /// <typeparam name="T">Generic type</typeparam>
    public class GenericChangeEventArgs<T> : CancelEventArgs
    {
        private readonly T oldValue;

        /// <summary>
        /// Initializes new instance with specified parameter.
        /// </summary>/// 
        /// <param name="oldValue">Old Value.</param>
        /// <param name="newValue">New Value</param>
        public GenericChangeEventArgs(T oldValue, T newValue) : base(false)
        {
            this.oldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// Initializes new instance with specified parameter.
        /// </summary>/// 
        /// <param name="oldValue">Old Value.</param>
        /// <param name="newValue">New Value</param>
        /// <param name="cancel">Cancel flag which can be used to stop event execution.</param>
        public GenericChangeEventArgs(T oldValue, T newValue, bool cancel) : base(cancel)
        {
            this.oldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// Gets old value which is being modified.
        /// </summary>
        public T OldValue
        {
            get { return oldValue; }
        }

        /// <summary>
        /// Gets or Sets new Value which is being set for old value.
        /// </summary>
        public T NewValue { get; set; }
    }
}