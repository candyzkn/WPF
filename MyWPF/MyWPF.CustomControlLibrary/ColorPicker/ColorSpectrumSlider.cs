// ***********************************************************************
// Assembly         : MyWPF.CustomControlLibrary
// Author           : ZhenKaining
// Created          : 08-15-2014
//
// Last Modified By : ZhenKaining
// Last Modified On : 08-15-2014
// ***********************************************************************
// <copyright file="ColorSpectrumSlider.cs" company="Geoway">
//     Copyright (c) Geoway. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

/// <summary>
/// The Constrols namespace.
/// </summary>
namespace MyWPF.CustomControlLibrary
{
    /// <summary>
    /// Class ColorSpectrumSlider.
    /// </summary>
    [TemplatePart(Name = PART_SpectrumDisplay, Type = typeof(Rectangle))]
    public class ColorSpectrumSlider : Slider
    {
        /// <summary>
        /// The par t_ spectrum display
        /// </summary>
        private const string PART_SpectrumDisplay = "PART_SpectrumDisplay";

        #region Private Members

        /// <summary>
        /// The _spectrum display
        /// </summary>
        private Rectangle _spectrumDisplay;
        /// <summary>
        /// The _picker brush
        /// </summary>
        private LinearGradientBrush _pickerBrush;

        #endregion //Private Members

        #region Constructors

        /// <summary>
        /// Initializes static members of the <see cref="ColorSpectrumSlider" /> class.
        /// </summary>
        static ColorSpectrumSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorSpectrumSlider), new FrameworkPropertyMetadata(typeof(ColorSpectrumSlider)));
        }

        #endregion //Constructors

        #region Dependency Properties

        /// <summary>
        /// The selected color property
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorSpectrumSlider), new PropertyMetadata(System.Windows.Media.Colors.Transparent));
        /// <summary>
        /// Gets or sets the color of the selected.
        /// </summary>
        /// <value>The color of the selected.</value>
        public Color SelectedColor
        {
            get
            {
                return (Color)GetValue(SelectedColorProperty);
            }
            set
            {
                SetValue(SelectedColorProperty, value);
            }
        }

        #endregion //Dependency Properties

        #region Base Class Overrides

        /// <summary>
        /// 生成 <see cref="T:System.Windows.Controls.Slider" /> 控件的可视化树。
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _spectrumDisplay = (Rectangle)GetTemplateChild(PART_SpectrumDisplay);
            CreateSpectrum();
            OnValueChanged(Double.NaN, Value);
        }

        /// <summary>
        /// 当 <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> 属性更改时，更新 <see cref="T:System.Windows.Controls.Slider" /> 的当前位置。
        /// </summary>
        /// <param name="oldValue"><see cref="T:System.Windows.Controls.Slider" /> 的旧 <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" />。</param>
        /// <param name="newValue"><see cref="T:System.Windows.Controls.Slider" /> 的新 <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" />。</param>
        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);

            Color color = ColorUtilities.ConvertHsvToRgb(360 - newValue, 1, 1);
            SelectedColor = color;
        }

        #endregion //Base Class Overrides

        #region Methods

        /// <summary>
        /// Creates the spectrum.
        /// </summary>
        private void CreateSpectrum()
        {
            _pickerBrush = new LinearGradientBrush();
            _pickerBrush.StartPoint = new Point(0.5, 0);
            _pickerBrush.EndPoint = new Point(0.5, 1);
            _pickerBrush.ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation;

            List<Color> colorsList = ColorUtilities.GenerateHsvSpectrum();

            double stopIncrement = (double)1 / colorsList.Count;

            int i;
            for (i = 0; i < colorsList.Count; i++)
            {
                _pickerBrush.GradientStops.Add(new GradientStop(colorsList[i], i * stopIncrement));
            }

            _pickerBrush.GradientStops[i - 1].Offset = 1.0;
            _spectrumDisplay.Fill = _pickerBrush;
        }

        #endregion //Methods
    }
}
