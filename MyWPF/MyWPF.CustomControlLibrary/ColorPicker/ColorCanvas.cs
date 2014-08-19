// ***********************************************************************
// Assembly         : MyWPF.CustomControlLibrary
// Author           : ZhenKaining
// Created          : 08-15-2014
//
// Last Modified By : ZhenKaining
// Last Modified On : 08-15-2014
// ***********************************************************************
// <copyright file="ColorCanvas.cs" company="Geoway">
//     Copyright (c) Geoway. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System;

/// <summary>
/// The Constrols namespace.
/// </summary>
namespace MyWPF.CustomControlLibrary
{
    /// <summary>
    /// Class ColorCanvas.
    /// </summary>
    [TemplatePart(Name = PART_ColorShadingCanvas, Type = typeof(Canvas))]
    [TemplatePart(Name = PART_ColorShadeSelector, Type = typeof(Canvas))]
    [TemplatePart(Name = PART_SpectrumSlider, Type = typeof(ColorSpectrumSlider))]
    [TemplatePart(Name = PART_HexadecimalTextBox, Type = typeof(TextBox))]
    public class ColorCanvas : Control
    {
        /// <summary>
        /// The par t_ color shading canvas
        /// </summary>
        private const string PART_ColorShadingCanvas = "PART_ColorShadingCanvas";
        /// <summary>
        /// The par t_ color shade selector
        /// </summary>
        private const string PART_ColorShadeSelector = "PART_ColorShadeSelector";
        /// <summary>
        /// The par t_ spectrum slider
        /// </summary>
        private const string PART_SpectrumSlider = "PART_SpectrumSlider";
        /// <summary>
        /// The par t_ hexadecimal text box
        /// </summary>
        private const string PART_HexadecimalTextBox = "PART_HexadecimalTextBox";

        #region Private Members

        /// <summary>
        /// The _color shade selector transform
        /// </summary>
        private TranslateTransform _colorShadeSelectorTransform = new TranslateTransform();
        /// <summary>
        /// The _color shading canvas
        /// </summary>
        private Canvas _colorShadingCanvas;
        /// <summary>
        /// The _color shade selector
        /// </summary>
        private Canvas _colorShadeSelector;
        /// <summary>
        /// The _spectrum slider
        /// </summary>
        private ColorSpectrumSlider _spectrumSlider;
        /// <summary>
        /// The _hexadecimal text box
        /// </summary>
        private TextBox _hexadecimalTextBox;
        /// <summary>
        /// The _current color position
        /// </summary>
        private Point? _currentColorPosition;
        /// <summary>
        /// The _surpress property changed
        /// </summary>
        private bool _surpressPropertyChanged;

        #endregion //Private Members

        #region Properties

        #region SelectedColor

        /// <summary>
        /// The selected color property
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorCanvas), new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedColorChanged));
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

        /// <summary>
        /// Handles the <see cref="E:SelectedColorChanged" /> event.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnSelectedColorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ColorCanvas colorCanvas = o as ColorCanvas;
            if (colorCanvas != null)
                colorCanvas.OnSelectedColorChanged((Color)e.OldValue, (Color)e.NewValue);
        }

        /// <summary>
        /// Called when [selected color changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnSelectedColorChanged(Color oldValue, Color newValue)
        {
            SetHexadecimalStringProperty(GetFormatedColorString(newValue), false);
            UpdateRGBValues(newValue);
            UpdateColorShadeSelectorPosition(newValue);

            RoutedPropertyChangedEventArgs<Color> args = new RoutedPropertyChangedEventArgs<Color>(oldValue, newValue);
            args.RoutedEvent = SelectedColorChangedEvent;
            RaiseEvent(args);
        }

        #endregion //SelectedColor

        #region RGB

        #region A

        /// <summary>
        /// A property
        /// </summary>
        public static readonly DependencyProperty AProperty = DependencyProperty.Register("A", typeof(byte), typeof(ColorCanvas), new UIPropertyMetadata((byte)255, OnAChanged));
        /// <summary>
        /// Gets or sets a.
        /// </summary>
        /// <value>A.</value>
        public byte A
        {
            get
            {
                return (byte)GetValue(AProperty);
            }
            set
            {
                SetValue(AProperty, value);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:AChanged" /> event.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnAChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ColorCanvas colorCanvas = o as ColorCanvas;
            if (colorCanvas != null)
                colorCanvas.OnAChanged((byte)e.OldValue, (byte)e.NewValue);
        }

        /// <summary>
        /// Called when [a changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnAChanged(byte oldValue, byte newValue)
        {
            if (!_surpressPropertyChanged)
                UpdateSelectedColor();
        }

        #endregion //A

        #region R

        /// <summary>
        /// The r property
        /// </summary>
        public static readonly DependencyProperty RProperty = DependencyProperty.Register("R", typeof(byte), typeof(ColorCanvas), new UIPropertyMetadata((byte)0, OnRChanged));
        /// <summary>
        /// Gets or sets the r.
        /// </summary>
        /// <value>The r.</value>
        public byte R
        {
            get
            {
                return (byte)GetValue(RProperty);
            }
            set
            {
                SetValue(RProperty, value);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:RChanged" /> event.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnRChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ColorCanvas colorCanvas = o as ColorCanvas;
            if (colorCanvas != null)
                colorCanvas.OnRChanged((byte)e.OldValue, (byte)e.NewValue);
        }

        /// <summary>
        /// Called when [r changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnRChanged(byte oldValue, byte newValue)
        {
            if (!_surpressPropertyChanged)
                UpdateSelectedColor();
        }

        #endregion //R

        #region G

        /// <summary>
        /// The g property
        /// </summary>
        public static readonly DependencyProperty GProperty = DependencyProperty.Register("G", typeof(byte), typeof(ColorCanvas), new UIPropertyMetadata((byte)0, OnGChanged));
        /// <summary>
        /// Gets or sets the g.
        /// </summary>
        /// <value>The g.</value>
        public byte G
        {
            get
            {
                return (byte)GetValue(GProperty);
            }
            set
            {
                SetValue(GProperty, value);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:GChanged" /> event.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnGChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ColorCanvas colorCanvas = o as ColorCanvas;
            if (colorCanvas != null)
                colorCanvas.OnGChanged((byte)e.OldValue, (byte)e.NewValue);
        }

        /// <summary>
        /// Called when [g changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnGChanged(byte oldValue, byte newValue)
        {
            if (!_surpressPropertyChanged)
                UpdateSelectedColor();
        }

        #endregion //G

        #region B

        /// <summary>
        /// The b property
        /// </summary>
        public static readonly DependencyProperty BProperty = DependencyProperty.Register("B", typeof(byte), typeof(ColorCanvas), new UIPropertyMetadata((byte)0, OnBChanged));
        /// <summary>
        /// Gets or sets the b.
        /// </summary>
        /// <value>The b.</value>
        public byte B
        {
            get
            {
                return (byte)GetValue(BProperty);
            }
            set
            {
                SetValue(BProperty, value);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:BChanged" /> event.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnBChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ColorCanvas colorCanvas = o as ColorCanvas;
            if (colorCanvas != null)
                colorCanvas.OnBChanged((byte)e.OldValue, (byte)e.NewValue);
        }

        /// <summary>
        /// Called when [b changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnBChanged(byte oldValue, byte newValue)
        {
            if (!_surpressPropertyChanged)
                UpdateSelectedColor();
        }

        #endregion //B

        #endregion //RGB

        #region HexadecimalString

        /// <summary>
        /// The hexadecimal string property
        /// </summary>
        public static readonly DependencyProperty HexadecimalStringProperty = DependencyProperty.Register("HexadecimalString", typeof(string), typeof(ColorCanvas), new UIPropertyMetadata("#FFFFFFFF", OnHexadecimalStringChanged, OnCoerceHexadecimalString));
        /// <summary>
        /// Gets or sets the hexadecimal string.
        /// </summary>
        /// <value>The hexadecimal string.</value>
        public string HexadecimalString
        {
            get
            {
                return (string)GetValue(HexadecimalStringProperty);
            }
            set
            {
                SetValue(HexadecimalStringProperty, value);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:HexadecimalStringChanged" /> event.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnHexadecimalStringChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ColorCanvas colorCanvas = o as ColorCanvas;
            if (colorCanvas != null)
                colorCanvas.OnHexadecimalStringChanged((string)e.OldValue, (string)e.NewValue);
        }

        /// <summary>
        /// Called when [hexadecimal string changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnHexadecimalStringChanged(string oldValue, string newValue)
        {
            string newColorString = GetFormatedColorString(newValue);
            string currentColorString = GetFormatedColorString(SelectedColor);
            if (!currentColorString.Equals(newColorString))
                UpdateSelectedColor((Color)ColorConverter.ConvertFromString(newColorString));

            SetHexadecimalTextBoxTextProperty(newValue);
        }

        /// <summary>
        /// Called when [coerce hexadecimal string].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="basevalue">The basevalue.</param>
        /// <returns>System.Object.</returns>
        private static object OnCoerceHexadecimalString(DependencyObject d, object basevalue)
        {
            var colorCanvas = (ColorCanvas)d;
            if (colorCanvas == null)
                return basevalue;

            return colorCanvas.OnCoerceHexadecimalString(basevalue);
        }

        /// <summary>
        /// Called when [coerce hexadecimal string].
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.IO.InvalidDataException">Color provided is not in the correct format.</exception>
        private object OnCoerceHexadecimalString(object newValue)
        {
            var value = newValue as string;
            string retValue = value;

            try
            {
                ColorConverter.ConvertFromString(value);
            }
            catch
            {
                //When HexadecimalString is changed via Code-Behind and hexadecimal format is bad, throw.
                throw new InvalidDataException("Color provided is not in the correct format.");
            }

            return retValue;
        }

        #endregion //HexadecimalString

        #region UsingAlphaChannel

        /// <summary>
        /// The using alpha channel property
        /// </summary>
        public static readonly DependencyProperty UsingAlphaChannelProperty = DependencyProperty.Register("UsingAlphaChannel", typeof(bool), typeof(ColorCanvas), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnUsingAlphaChannelPropertyChanged)));
        /// <summary>
        /// Gets or sets a value indicating whether [using alpha channel].
        /// </summary>
        /// <value><c>true</c> if [using alpha channel]; otherwise, <c>false</c>.</value>
        public bool UsingAlphaChannel
        {
            get
            {
                return (bool)GetValue(UsingAlphaChannelProperty);
            }
            set
            {
                SetValue(UsingAlphaChannelProperty, value);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:UsingAlphaChannelPropertyChanged" /> event.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnUsingAlphaChannelPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ColorCanvas colorCanvas = o as ColorCanvas;
            if (colorCanvas != null)
                colorCanvas.OnUsingAlphaChannelChanged();
        }

        /// <summary>
        /// Called when [using alpha channel changed].
        /// </summary>
        protected virtual void OnUsingAlphaChannelChanged()
        {
            SetHexadecimalStringProperty(GetFormatedColorString(SelectedColor), false);
        }

        #endregion //UsingAlphaChannel

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// Initializes static members of the <see cref="ColorCanvas" /> class.
        /// </summary>
        static ColorCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorCanvas), new FrameworkPropertyMetadata(typeof(ColorCanvas)));
        }

        #endregion //Constructors

        #region Base Class Overrides

        /// <summary>
        /// 在派生类中重写后，每当应用程序代码或内部进程调用 <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />，都将调用此方法。
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_colorShadingCanvas != null)
            {
                _colorShadingCanvas.MouseLeftButtonDown -= ColorShadingCanvas_MouseLeftButtonDown;
                _colorShadingCanvas.MouseLeftButtonUp -= ColorShadingCanvas_MouseLeftButtonUp;
                _colorShadingCanvas.MouseMove -= ColorShadingCanvas_MouseMove;
                _colorShadingCanvas.SizeChanged -= ColorShadingCanvas_SizeChanged;
            }

            _colorShadingCanvas = GetTemplateChild(PART_ColorShadingCanvas) as Canvas;

            if (_colorShadingCanvas != null)
            {
                _colorShadingCanvas.MouseLeftButtonDown += ColorShadingCanvas_MouseLeftButtonDown;
                _colorShadingCanvas.MouseLeftButtonUp += ColorShadingCanvas_MouseLeftButtonUp;
                _colorShadingCanvas.MouseMove += ColorShadingCanvas_MouseMove;
                _colorShadingCanvas.SizeChanged += ColorShadingCanvas_SizeChanged;
            }

            _colorShadeSelector = GetTemplateChild(PART_ColorShadeSelector) as Canvas;

            if (_colorShadeSelector != null)
                _colorShadeSelector.RenderTransform = _colorShadeSelectorTransform;

            if (_spectrumSlider != null)
                _spectrumSlider.ValueChanged -= SpectrumSlider_ValueChanged;

            _spectrumSlider = GetTemplateChild(PART_SpectrumSlider) as ColorSpectrumSlider;

            if (_spectrumSlider != null)
                _spectrumSlider.ValueChanged += SpectrumSlider_ValueChanged;

            if (_hexadecimalTextBox != null)
                _hexadecimalTextBox.LostFocus -= new RoutedEventHandler(HexadecimalTextBox_LostFocus);

            _hexadecimalTextBox = GetTemplateChild(PART_HexadecimalTextBox) as TextBox;

            if (_hexadecimalTextBox != null)
                _hexadecimalTextBox.LostFocus += new RoutedEventHandler(HexadecimalTextBox_LostFocus);

            UpdateRGBValues(SelectedColor);
            UpdateColorShadeSelectorPosition(SelectedColor);

            // When changing theme, HexadecimalString needs to be set since it is not binded.
            SetHexadecimalTextBoxTextProperty(GetFormatedColorString(SelectedColor));
        }

        /// <summary>
        /// 当未处理的 <see cref="E:System.Windows.Input.Keyboard.KeyDown" /> 附加事件在其路由中到达派生自此类的元素时，调用此方法。实现此方法可为此事件添加类处理。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="T:System.Windows.Input.KeyEventArgs" />。</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            //hitting enter on textbox will update Hexadecimal string
            if (e.Key == Key.Enter && e.OriginalSource is TextBox)
            {
                TextBox textBox = (TextBox)e.OriginalSource;
                if (textBox.Name == PART_HexadecimalTextBox)
                    SetHexadecimalStringProperty(textBox.Text, true);
            }
        }

        #endregion //Base Class Overrides

        #region Event Handlers

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the ColorShadingCanvas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
        void ColorShadingCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(_colorShadingCanvas);
            UpdateColorShadeSelectorPositionAndCalculateColor(p, true);
            _colorShadingCanvas.CaptureMouse();
        }

        /// <summary>
        /// Handles the MouseLeftButtonUp event of the ColorShadingCanvas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
        void ColorShadingCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _colorShadingCanvas.ReleaseMouseCapture();
        }

        /// <summary>
        /// Handles the MouseMove event of the ColorShadingCanvas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        void ColorShadingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point p = e.GetPosition(_colorShadingCanvas);
                UpdateColorShadeSelectorPositionAndCalculateColor(p, true);
                Mouse.Synchronize();
            }
        }

        /// <summary>
        /// Handles the SizeChanged event of the ColorShadingCanvas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SizeChangedEventArgs" /> instance containing the event data.</param>
        void ColorShadingCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_currentColorPosition != null)
            {
                Point _newPoint = new Point
                {
                    X = ((Point)_currentColorPosition).X * e.NewSize.Width,
                    Y = ((Point)_currentColorPosition).Y * e.NewSize.Height
                };

                UpdateColorShadeSelectorPositionAndCalculateColor(_newPoint, false);
            }
        }

        /// <summary>
        /// Spectrums the slider_ value changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void SpectrumSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_currentColorPosition != null)
            {
                CalculateColor((Point)_currentColorPosition);
            }
        }

        /// <summary>
        /// Handles the LostFocus event of the HexadecimalTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        void HexadecimalTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            SetHexadecimalStringProperty(textbox.Text, true);
        }

        #endregion //Event Handlers

        #region Events

        /// <summary>
        /// The selected color changed event
        /// </summary>
        public static readonly RoutedEvent SelectedColorChangedEvent = EventManager.RegisterRoutedEvent("SelectedColorChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Color>), typeof(ColorCanvas));
        /// <summary>
        /// Occurs when [selected color changed].
        /// </summary>
        public event RoutedPropertyChangedEventHandler<Color> SelectedColorChanged
        {
            add
            {
                AddHandler(SelectedColorChangedEvent, value);
            }
            remove
            {
                RemoveHandler(SelectedColorChangedEvent, value);
            }
        }

        #endregion //Events

        #region Methods

        /// <summary>
        /// Updates the color of the selected.
        /// </summary>
        private void UpdateSelectedColor()
        {
            SelectedColor = Color.FromArgb(A, R, G, B);
        }

        /// <summary>
        /// Updates the color of the selected.
        /// </summary>
        /// <param name="color">The color.</param>
        private void UpdateSelectedColor(Color color)
        {
            SelectedColor = Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Updates the RGB values.
        /// </summary>
        /// <param name="color">The color.</param>
        private void UpdateRGBValues(Color color)
        {
            _surpressPropertyChanged = true;

            A = color.A;
            R = color.R;
            G = color.G;
            B = color.B;

            _surpressPropertyChanged = false;
        }

        /// <summary>
        /// Updates the color of the color shade selector position and calculate.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="calculateColor">if set to <c>true</c> [calculate color].</param>
        private void UpdateColorShadeSelectorPositionAndCalculateColor(Point p, bool calculateColor)
        {
            if (p.Y < 0)
                p.Y = 0;

            if (p.X < 0)
                p.X = 0;

            if (p.X > _colorShadingCanvas.ActualWidth)
                p.X = _colorShadingCanvas.ActualWidth;

            if (p.Y > _colorShadingCanvas.ActualHeight)
                p.Y = _colorShadingCanvas.ActualHeight;

            _colorShadeSelectorTransform.X = p.X - (_colorShadeSelector.Width / 2);
            _colorShadeSelectorTransform.Y = p.Y - (_colorShadeSelector.Height / 2);

            p.X = p.X / _colorShadingCanvas.ActualWidth;
            p.Y = p.Y / _colorShadingCanvas.ActualHeight;

            _currentColorPosition = p;

            if (calculateColor)
                CalculateColor(p);
        }

        /// <summary>
        /// Updates the color shade selector position.
        /// </summary>
        /// <param name="color">The color.</param>
        private void UpdateColorShadeSelectorPosition(Color color)
        {
            if (_spectrumSlider == null || _colorShadingCanvas == null)
                return;

            _currentColorPosition = null;

            HsvColor hsv = ColorUtilities.ConvertRgbToHsv(color.R, color.G, color.B);

            if (!(color.R == color.G && color.R == color.B))
                _spectrumSlider.Value = hsv.H;

            Point p = new Point(hsv.S, 1 - hsv.V);

            _currentColorPosition = p;

            _colorShadeSelectorTransform.X = (p.X * _colorShadingCanvas.Width) - 5;
            _colorShadeSelectorTransform.Y = (p.Y * _colorShadingCanvas.Height) - 5;
        }

        /// <summary>
        /// Calculates the color.
        /// </summary>
        /// <param name="p">The p.</param>
        private void CalculateColor(Point p)
        {
            HsvColor hsv = new HsvColor(360 - _spectrumSlider.Value, 1, 1)
            {
                S = p.X,
                V = 1 - p.Y
            };
            var currentColor = ColorUtilities.ConvertHsvToRgb(hsv.H, hsv.S, hsv.V);
            currentColor.A = A;
            SelectedColor = currentColor;
            SetHexadecimalStringProperty(GetFormatedColorString(SelectedColor), false);
        }

        /// <summary>
        /// Gets the formated color string.
        /// </summary>
        /// <param name="colorToFormat">The color to format.</param>
        /// <returns>System.String.</returns>
        private string GetFormatedColorString(Color colorToFormat)
        {
            return ColorUtilities.FormatColorString(colorToFormat.ToString(), UsingAlphaChannel);
        }

        /// <summary>
        /// Gets the formated color string.
        /// </summary>
        /// <param name="stringToFormat">The string to format.</param>
        /// <returns>System.String.</returns>
        private string GetFormatedColorString(string stringToFormat)
        {
            return ColorUtilities.FormatColorString(stringToFormat, UsingAlphaChannel);
        }

        /// <summary>
        /// Sets the hexadecimal string property.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <param name="modifyFromUI">if set to <c>true</c> [modify from UI].</param>
        private void SetHexadecimalStringProperty(string newValue, bool modifyFromUI)
        {
            if (modifyFromUI)
            {
                try
                {
                    ColorConverter.ConvertFromString(newValue);
                    HexadecimalString = newValue;
                }
                catch
                {
                    //When HexadecimalString is changed via UI and hexadecimal format is bad, keep the previous HexadecimalString.
                    SetHexadecimalTextBoxTextProperty(HexadecimalString);
                }
            }
            else
            {
                //When HexadecimalString is changed via Code-Behind, hexadecimal format will be evaluated in OnCoerceHexadecimalString()
                HexadecimalString = newValue;
            }
        }

        /// <summary>
        /// Sets the hexadecimal text box text property.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        private void SetHexadecimalTextBoxTextProperty(string newValue)
        {
            if (_hexadecimalTextBox != null)
                _hexadecimalTextBox.Text = newValue;
        }

        #endregion //Methods
    }
}
