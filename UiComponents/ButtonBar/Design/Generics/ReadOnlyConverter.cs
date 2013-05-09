using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ButtonBarsControl.Design.Generics
{
    internal class ReadOnlyConverter : TypeConverter
    {
        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
        /// </summary>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. </param><param name="sourceType">A <see cref="T:System.Type"/> that represents the type you want to convert from. </param>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return false;
        }

        /// <summary>
        /// Returns whether this converter can convert the object to the specified type, using the specified context.
        /// </summary>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. </param><param name="destinationType">A <see cref="T:System.Type"/> that represents the type you want to convert to. </param>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return false;
        }

        /// <summary>
        /// Returns whether changing a value on this object requires a call to <see cref="M:System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)"/> to create a new value, using the specified context.
        /// </summary>
        /// <returns>
        /// true if changing a property on this object requires a call to <see cref="M:System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)"/> to create a new value; otherwise, false.
        /// </returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. </param>
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return false;
        }

        /// <summary>
        /// Returns whether this object supports properties, using the specified context.
        /// </summary>
        /// <returns>
        /// true if <see cref="M:System.ComponentModel.TypeConverter.GetProperties(System.Object)"/> should be called to find the properties of this object; otherwise, false.
        /// </returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. </param>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Returns a collection of properties for the type of array specified by the value parameter, using the specified context and attributes.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> with the properties that are exposed for this data type, or null if there are no properties.
        /// </returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. </param><param name="value">An <see cref="T:System.Object"/> that specifies the type of array for which to get properties. </param><param name="attributes">An array of type <see cref="T:System.Attribute"/> that is used as a filter. </param>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value,
                                                                   Attribute[] attributes)
        {
            var init = TypeDescriptor.GetProperties(value.GetType(), attributes);
            var pds = new PropertyDescriptor[init.Count];
            for (var i = 0; i < init.Count; i++)
            {
                if (!init[i].IsBrowsable)
                    continue;
                var attrs = new List<Attribute>();
                for (var j = 0; j < attributes.Length; j++)
                {
                    attrs.Add(attributes[j]);
                }
                attrs.Add(new ReadOnlyAttribute(true));
                if (init[i].Converter == null || !init[i].Converter.GetType().Assembly.GlobalAssemblyCache)
                {
                    attrs.Add(new TypeConverterAttribute(typeof (ReadOnlyConverter)));
                }
                attrs.Add(new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden));
                pds[i] = new PD(init[i].ComponentType, init[i].Name, init[i].PropertyType, attrs.ToArray());
            }
            return new PropertyDescriptorCollection(pds);
        }

        #region Nested type: PD

        private class PD : SimplePropertyDescriptor
        {
            public PD(Type componentType, string name, Type propertyType, Attribute[] attributes)
                : base(componentType, name, propertyType, attributes)
            {
            }

            #region Overrides of PropertyDescriptor

            /// <summary>
            /// When overridden in a derived class, gets the current value of the property on a component.
            /// </summary>
            /// <returns>
            /// The value of a property for a given component.
            /// </returns>
            /// <param name="component">The component with the property for which to retrieve the value. </param>
            public override object GetValue(object component)
            {
                return component.GetType().GetProperty(Name).GetValue(component, null);
            }

            /// <summary>
            /// When overridden in a derived class, sets the value of the component to a different value.
            /// </summary>
            /// <param name="component">The component with the property value that is to be set. </param><param name="value">The new value. </param>
            public override void SetValue(object component, object value)
            {
            }

            #endregion
        }

        #endregion
    }
}