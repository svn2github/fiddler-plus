using System.ComponentModel;
using ButtonBarsControl.Control;
using ButtonBarsControl.Design.Enums;
using ButtonBarsControl.Design.Generics;

namespace ButtonBarsControl.Design.Entity
{
    /// <summary>
    /// Class holding theme information for <see cref="ButtonBar"/>
    /// </summary>
    [TypeConverter(typeof (GenericConverter<ThemeProperty>))]
    public class ThemeProperty
    {
        private ColorScheme colorScheme;
        private bool useTheme;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeProperty"/> class. 
        /// </summary>
        public ThemeProperty()
        {
            Reset();
        }

        /// <summary>
        /// Gets or Sets which color theme should be used. 
        /// This is only applicable when <see cref="UseTheme"/> is set to true.
        /// </summary>
        public ColorScheme ColorScheme
        {
            get { return colorScheme; }
            set
            {
                if (colorScheme != value)
                {
                    colorScheme = value;
                    OnThemeChanged(new GenericEventArgs<AppearanceAction>(AppearanceAction.Repaint));
                }
            }
        }

        /// <summary>
        /// Gets or Sets wether predefined themes should be used or not.
        /// </summary>
        public bool UseTheme
        {
            get { return useTheme; }
            set
            {
                if (useTheme != value)
                {
                    useTheme = value;
                    OnThemeChanged(new GenericEventArgs<AppearanceAction>(AppearanceAction.Repaint));
                }
            }
        }

        /// <summary>
        /// Occurs when properties of <see cref="ThemeProperty"/> has been changed.
        /// </summary>
        public event GenericEventHandler<AppearanceAction> ThemeChanged;

        /// <summary>
        /// Triggers <see cref="ThemeChanged"/> event.
        /// </summary>
        /// <param name="e">Object containing event data.</param>
        protected virtual void OnThemeChanged(GenericEventArgs<AppearanceAction> e)
        {
            if (ThemeChanged != null)
            {
                ThemeChanged(this, e);
            }
        }

        /// <summary>
        /// Get wether default values of the object has been modified.
        /// </summary>
        /// <returns>Returns true if default values has been modified for current object.</returns>
        public virtual bool DefaultChanged()
        {
            return ShouldSerializeColorScheme() || ShouldSerializeUseTheme();
        }

        /// <summary>
        /// Resets current object to use default value for each property.
        /// </summary>
        public void Reset()
        {
            ResetColorScheme();
            ResetUseTheme();
        }

        private bool ShouldSerializeUseTheme()
        {
            return useTheme != true;
        }

        private bool ShouldSerializeColorScheme()
        {
            return colorScheme != ColorScheme.Default;
        }

        private void ResetUseTheme()
        {
            useTheme = true;
        }

        private void ResetColorScheme()
        {
            colorScheme = ColorScheme.Default;
        }
    }
}