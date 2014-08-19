// ***********************************************************************
// Assembly         : MyWPF.CustomControlLibrary
// Author           : ZhenKaining
// Created          : 08-15-2014
//
// Last Modified By : ZhenKaining
// Last Modified On : 08-15-2014
// ***********************************************************************
// <copyright file="ColorItem.cs" company="Geoway">
//     Copyright (c) Geoway. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows.Media;

/// <summary>
/// The Constrols namespace.
/// </summary>
namespace MyWPF.CustomControlLibrary
{
    /// <summary>
    /// Class ColorItem.k
    /// </summary>
    public class ColorItem
    {
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        public Color Color
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorItem"/> class.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="name">The name.</param>
        public ColorItem(Color color, string name)
        {
            Color = color;
            Name = name;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">与当前的 <see cref="T:System.Object" /> 进行比较的 <see cref="T:System.Object" />。</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var ci = obj as ColorItem;
            if (ci == null)
                return false;
            return (ci.Color.Equals(Color) && ci.Name.Equals(Name));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return this.Color.GetHashCode() ^ this.Name.GetHashCode();
        }
    }
}
