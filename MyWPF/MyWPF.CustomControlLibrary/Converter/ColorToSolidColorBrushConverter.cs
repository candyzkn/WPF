// ***********************************************************************
// Assembly         : MyWPF.CustomControlLibrary
// Author           : ZhenKaining
// Created          : 08-15-2014
//
// Last Modified By : ZhenKaining
// Last Modified On : 08-15-2014
// ***********************************************************************
// <copyright file="ColorToSolidColorBrushConverter.cs" company="Geoway">
//     Copyright (c) Geoway. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows.Data;
using System.Windows.Media;

/// <summary>
/// The CustomControlLibrary namespace.
/// </summary>
namespace MyWPF.CustomControlLibrary
{

    /// <summary>
    /// Class ColorToSolidColorBrushConverter.
    /// </summary>
    public class ColorToSolidColorBrushConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts a Color to a SolidColorBrush.
        /// </summary>
        /// <param name="value">The Color produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted SolidColorBrush. If the method returns null, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
                return new SolidColorBrush((Color)value);

            return value;
        }


        /// <summary>
        /// Converts a SolidColorBrush to a Color.
        /// </summary>
        /// <param name="value">The SolidColorBrush that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <remarks>Currently not used in toolkit, but provided for developer use in their own projects</remarks>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
                return ((SolidColorBrush)value).Color;

            return value;
        }

        #endregion
    }
}
