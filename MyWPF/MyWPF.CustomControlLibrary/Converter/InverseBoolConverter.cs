// ***********************************************************************
// Assembly         : MyWPF.CustomControlLibrary
// Author           : ZhenKaining
// Created          : 08-15-2014
//
// Last Modified By : ZhenKaining
// Last Modified On : 08-15-2014
// ***********************************************************************
// <copyright file="InverseBoolConverter.cs" company="Geoway">
//     Copyright (c) Geoway. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows.Data;

/// <summary>
/// The CustomControlLibrary namespace.
/// </summary>
namespace MyWPF.CustomControlLibrary
{
    /// <summary>
    /// Class InverseBoolConverter.
    /// </summary>
    public class InverseBoolConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// 转换值。
        /// </summary>
        /// <param name="value">绑定源生成的值。</param>
        /// <param name="targetType">绑定目标属性的类型。</param>
        /// <param name="parameter">要使用的转换器参数。</param>
        /// <param name="culture">要用在转换器中的区域性。</param>
        /// <returns>转换后的值。如果该方法返回 null，则使用有效的 null 值。</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }

        /// <summary>
        /// 转换值。
        /// </summary>
        /// <param name="value">绑定目标生成的值。</param>
        /// <param name="targetType">要转换到的类型。</param>
        /// <param name="parameter">要使用的转换器参数。</param>
        /// <param name="culture">要用在转换器中的区域性。</param>
        /// <returns>转换后的值。如果该方法返回 null，则使用有效的 null 值。</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
