using System;

namespace ButtonBarsControl.Design.Attributes
{
    /// <summary>
    /// Indicates Image property for indexing. e.g. If parent contains image list use "Parent.ImageList".
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal sealed class ImagePropertyAttribute : Attribute
    {
        private readonly string propertyName;

        /// <summary>
        /// Crate instance of the <see cref="ImagePropertyAttribute"/>
        /// </summary>
        /// <param name="relatedImageList"></param>
        public ImagePropertyAttribute(string relatedImageList)
        {
            propertyName = relatedImageList;
        }

        /// <summary>
        /// Gets the name of the property which is to be used for imagelist.
        /// </summary>
        public string PropertyName
        {
            get { return propertyName; }
        }
    }
}