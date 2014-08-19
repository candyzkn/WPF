// ***********************************************************************
// Assembly         : MyWPF.CustomControlLibrary
// Author           : ZhenKaining
// Created          : 08-15-2014
//
// Last Modified By : ZhenKaining
// Last Modified On : 08-15-2014
// ***********************************************************************
// <copyright file="ColorPicker.cs" company="Geoway">
//     Copyright (c) Geoway. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Data;

/// <summary>
/// The Constrols namespace.
/// </summary>
namespace MyWPF.CustomControlLibrary
{
    /// <summary>
    /// Enum ColorMode
    /// </summary>
    public enum ColorMode
    {
        /// <summary>
        /// The color palette
        /// </summary>
        ColorPalette,
        /// <summary>
        /// The color canvas
        /// </summary>
        ColorCanvas
    }

    /// <summary>
    /// Enum ColorSortingMode
    /// </summary>
    public enum ColorSortingMode
    {
        /// <summary>
        /// The alphabetical
        /// </summary>
        Alphabetical,
        /// <summary>
        /// The hue saturation brightness
        /// </summary>
        HueSaturationBrightness
    }

    /// <summary>
    /// Class ColorPicker.
    /// </summary>
    [TemplatePart(Name = PART_AvailableColors, Type = typeof(ListBox))]
    [TemplatePart(Name = PART_StandardColors, Type = typeof(ListBox))]
    [TemplatePart(Name = PART_RecentColors, Type = typeof(ListBox))]
    [TemplatePart(Name = PART_ColorPickerToggleButton, Type = typeof(ToggleButton))]
    [TemplatePart(Name = PART_ColorPickerPalettePopup, Type = typeof(Popup))]
    [TemplatePart(Name = PART_ColorModeButton, Type = typeof(Button))]
    public class ColorPicker : Control
    {
        /// <summary>
        /// The par t_ available colors
        /// </summary>
        private const string PART_AvailableColors = "PART_AvailableColors";
        /// <summary>
        /// The par t_ standard colors
        /// </summary>
        private const string PART_StandardColors = "PART_StandardColors";
        /// <summary>
        /// The par t_ recent colors
        /// </summary>
        private const string PART_RecentColors = "PART_RecentColors";
        /// <summary>
        /// The par t_ color picker toggle button
        /// </summary>
        private const string PART_ColorPickerToggleButton = "PART_ColorPickerToggleButton";
        /// <summary>
        /// The par t_ color picker palette popup
        /// </summary>
        private const string PART_ColorPickerPalettePopup = "PART_ColorPickerPalettePopup";
        /// <summary>
        /// The par t_ color mode button
        /// </summary>
        private const string PART_ColorModeButton = "PART_ColorModeButton";

        #region Members

        /// <summary>
        /// The _available colors
        /// </summary>
        private ListBox _availableColors;
        /// <summary>
        /// The _standard colors
        /// </summary>
        private ListBox _standardColors;
        /// <summary>
        /// The _recent colors
        /// </summary>
        private ListBox _recentColors;
        /// <summary>
        /// The _toggle button
        /// </summary>
        private ToggleButton _toggleButton;
        /// <summary>
        /// The _popup
        /// </summary>
        private Popup _popup;
        /// <summary>
        /// The _color mode button
        /// </summary>
        private Button _colorModeButton;
        /// <summary>
        /// The _selection changed
        /// </summary>
        private bool _selectionChanged;

        #endregion //Members

        #region Properties

        #region AvailableColors

        /// <summary>
        /// The available colors property
        /// </summary>
        public static readonly DependencyProperty AvailableColorsProperty = DependencyProperty.Register("AvailableColors", typeof(ObservableCollection<ColorItem>), typeof(ColorPicker), new UIPropertyMetadata(CreateAvailableColors()));
        /// <summary>
        /// Gets or sets the available colors.
        /// </summary>
        /// <value>The available colors.</value>
        public ObservableCollection<ColorItem> AvailableColors
        {
            get
            {
                return (ObservableCollection<ColorItem>)GetValue(AvailableColorsProperty);
            }
            set
            {
                SetValue(AvailableColorsProperty, value);
            }
        }

        #endregion //AvailableColors

        #region AvailableColorsSortingMode

        /// <summary>
        /// The available colors sorting mode property
        /// </summary>
        public static readonly DependencyProperty AvailableColorsSortingModeProperty = DependencyProperty.Register("AvailableColorsSortingMode", typeof(ColorSortingMode), typeof(ColorPicker), new UIPropertyMetadata(ColorSortingMode.Alphabetical, OnAvailableColorsSortingModeChanged));
        /// <summary>
        /// Gets or sets the available colors sorting mode.
        /// </summary>
        /// <value>The available colors sorting mode.</value>
        public ColorSortingMode AvailableColorsSortingMode
        {
            get
            {
                return (ColorSortingMode)GetValue(AvailableColorsSortingModeProperty);
            }
            set
            {
                SetValue(AvailableColorsSortingModeProperty, value);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:AvailableColorsSortingModeChanged" /> event.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnAvailableColorsSortingModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker colorPicker = (ColorPicker)d;
            if (colorPicker != null)
                colorPicker.OnAvailableColorsSortingModeChanged((ColorSortingMode)e.OldValue, (ColorSortingMode)e.NewValue);
        }

        /// <summary>
        /// Called when [available colors sorting mode changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private void OnAvailableColorsSortingModeChanged(ColorSortingMode oldValue, ColorSortingMode newValue)
        {
            ListCollectionView lcv = (ListCollectionView)(CollectionViewSource.GetDefaultView(this.AvailableColors));
            if (lcv != null)
            {
                lcv.CustomSort = (AvailableColorsSortingMode == ColorSortingMode.HueSaturationBrightness)
                                  ? new ColorSorter()
                                  : null;
            }
        }

        #endregion //AvailableColorsSortingMode

        #region AvailableColorsHeader

        /// <summary>
        /// The available colors header property
        /// </summary>
        public static readonly DependencyProperty AvailableColorsHeaderProperty = DependencyProperty.Register("AvailableColorsHeader", typeof(string), typeof(ColorPicker), new UIPropertyMetadata("系统颜色"));
        /// <summary>
        /// Gets or sets the available colors header.
        /// </summary>
        /// <value>The available colors header.</value>
        public string AvailableColorsHeader
        {
            get
            {
                return (string)GetValue(AvailableColorsHeaderProperty);
            }
            set
            {
                SetValue(AvailableColorsHeaderProperty, value);
            }
        }

        #endregion //AvailableColorsHeader

        #region ButtonStyle

        /// <summary>
        /// The button style property
        /// </summary>
        public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(ColorPicker));
        /// <summary>
        /// Gets or sets the button style.
        /// </summary>
        /// <value>The button style.</value>
        public Style ButtonStyle
        {
            get
            {
                return (Style)GetValue(ButtonStyleProperty);
            }
            set
            {
                SetValue(ButtonStyleProperty, value);
            }
        }

        #endregion //ButtonStyle

        #region DisplayColorAndName

        /// <summary>
        /// The display color and name property
        /// </summary>
        public static readonly DependencyProperty DisplayColorAndNameProperty = DependencyProperty.Register("DisplayColorAndName", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(false));
        /// <summary>
        /// Gets or sets a value indicating whether [display color and name].
        /// </summary>
        /// <value><c>true</c> if [display color and name]; otherwise, <c>false</c>.</value>
        public bool DisplayColorAndName
        {
            get
            {
                return (bool)GetValue(DisplayColorAndNameProperty);
            }
            set
            {
                SetValue(DisplayColorAndNameProperty, value);
            }
        }

        #endregion //DisplayColorAndName

        #region ColorMode

        /// <summary>
        /// The color mode property
        /// </summary>
        public static readonly DependencyProperty ColorModeProperty = DependencyProperty.Register("ColorMode", typeof(ColorMode), typeof(ColorPicker), new UIPropertyMetadata(ColorMode.ColorPalette));
        /// <summary>
        /// Gets or sets the color mode.
        /// </summary>
        /// <value>The color mode.</value>
        public ColorMode ColorMode
        {
            get
            {
                return (ColorMode)GetValue(ColorModeProperty);
            }
            set
            {
                SetValue(ColorModeProperty, value);
            }
        }

        #endregion //ColorMode

        #region IsOpen

        /// <summary>
        /// The is open property
        /// </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(false));
        /// <summary>
        /// Gets or sets a value indicating whether this instance is open.
        /// </summary>
        /// <value><c>true</c> if this instance is open; otherwise, <c>false</c>.</value>
        public bool IsOpen
        {
            get
            {
                return (bool)GetValue(IsOpenProperty);
            }
            set
            {
                SetValue(IsOpenProperty, value);
            }
        }

        #endregion //IsOpen

        #region RecentColors

        /// <summary>
        /// The recent colors property
        /// </summary>
        public static readonly DependencyProperty RecentColorsProperty = DependencyProperty.Register("RecentColors", typeof(ObservableCollection<ColorItem>), typeof(ColorPicker), new UIPropertyMetadata(null));
        /// <summary>
        /// Gets or sets the recent colors.
        /// </summary>
        /// <value>The recent colors.</value>
        public ObservableCollection<ColorItem> RecentColors
        {
            get
            {
                return (ObservableCollection<ColorItem>)GetValue(RecentColorsProperty);
            }
            set
            {
                SetValue(RecentColorsProperty, value);
            }
        }

        #endregion //RecentColors

        #region RecentColorsHeader

        /// <summary>
        /// The recent colors header property
        /// </summary>
        public static readonly DependencyProperty RecentColorsHeaderProperty = DependencyProperty.Register("RecentColorsHeader", typeof(string), typeof(ColorPicker), new UIPropertyMetadata("最近使用"));
        /// <summary>
        /// Gets or sets the recent colors header.
        /// </summary>
        /// <value>The recent colors header.</value>
        public string RecentColorsHeader
        {
            get
            {
                return (string)GetValue(RecentColorsHeaderProperty);
            }
            set
            {
                SetValue(RecentColorsHeaderProperty, value);
            }
        }

        #endregion //RecentColorsHeader

        #region SelectedColor

        /// <summary>
        /// The selected color property
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorPicker), new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedColorPropertyChanged)));
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
        /// Handles the <see cref="E:SelectedColorPropertyChanged" /> event.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnSelectedColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker colorPicker = (ColorPicker)d;
            if (colorPicker != null)
                colorPicker.OnSelectedColorChanged((Color)e.OldValue, (Color)e.NewValue);
        }

        /// <summary>
        /// Called when [selected color changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private void OnSelectedColorChanged(Color oldValue, Color newValue)
        {
            SelectedColorText = GetFormatedColorString(newValue);

            RoutedPropertyChangedEventArgs<Color> args = new RoutedPropertyChangedEventArgs<Color>(oldValue, newValue);
            args.RoutedEvent = ColorPicker.SelectedColorChangedEvent;
            RaiseEvent(args);
        }

        #endregion //SelectedColor

        #region SelectedColorText

        /// <summary>
        /// The selected color text property
        /// </summary>
        public static readonly DependencyProperty SelectedColorTextProperty = DependencyProperty.Register("SelectedColorText", typeof(string), typeof(ColorPicker), new UIPropertyMetadata("Black"));
        /// <summary>
        /// Gets or sets the selected color text.
        /// </summary>
        /// <value>The selected color text.</value>
        public string SelectedColorText
        {
            get
            {
                return (string)GetValue(SelectedColorTextProperty);
            }
            protected set
            {
                SetValue(SelectedColorTextProperty, value);
            }
        }

        #endregion //SelectedColorText

        #region ShowAdvancedButton

        /// <summary>
        /// The show advanced button property
        /// </summary>
        public static readonly DependencyProperty ShowAdvancedButtonProperty = DependencyProperty.Register("ShowAdvancedButton", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(true));
        /// <summary>
        /// Gets or sets a value indicating whether [show advanced button].
        /// </summary>
        /// <value><c>true</c> if [show advanced button]; otherwise, <c>false</c>.</value>
        public bool ShowAdvancedButton
        {
            get
            {
                return (bool)GetValue(ShowAdvancedButtonProperty);
            }
            set
            {
                SetValue(ShowAdvancedButtonProperty, value);
            }
        }

        #endregion //ShowAdvancedButton

        #region ShowAvailableColors

        /// <summary>
        /// The show available colors property
        /// </summary>
        public static readonly DependencyProperty ShowAvailableColorsProperty = DependencyProperty.Register("ShowAvailableColors", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(true));
        /// <summary>
        /// Gets or sets a value indicating whether [show available colors].
        /// </summary>
        /// <value><c>true</c> if [show available colors]; otherwise, <c>false</c>.</value>
        public bool ShowAvailableColors
        {
            get
            {
                return (bool)GetValue(ShowAvailableColorsProperty);
            }
            set
            {
                SetValue(ShowAvailableColorsProperty, value);
            }
        }

        #endregion //ShowAvailableColors

        #region ShowRecentColors

        /// <summary>
        /// The show recent colors property
        /// </summary>
        public static readonly DependencyProperty ShowRecentColorsProperty = DependencyProperty.Register("ShowRecentColors", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(false));
        /// <summary>
        /// Gets or sets a value indicating whether [show recent colors].
        /// </summary>
        /// <value><c>true</c> if [show recent colors]; otherwise, <c>false</c>.</value>
        public bool ShowRecentColors
        {
            get
            {
                return (bool)GetValue(ShowRecentColorsProperty);
            }
            set
            {
                SetValue(ShowRecentColorsProperty, value);
            }
        }

        #endregion //DisplayRecentColors

        #region ShowStandardColors

        /// <summary>
        /// The show standard colors property
        /// </summary>
        public static readonly DependencyProperty ShowStandardColorsProperty = DependencyProperty.Register("ShowStandardColors", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(true));
        /// <summary>
        /// Gets or sets a value indicating whether [show standard colors].
        /// </summary>
        /// <value><c>true</c> if [show standard colors]; otherwise, <c>false</c>.</value>
        public bool ShowStandardColors
        {
            get
            {
                return (bool)GetValue(ShowStandardColorsProperty);
            }
            set
            {
                SetValue(ShowStandardColorsProperty, value);
            }
        }

        #endregion //DisplayStandardColors

        #region ShowDropDownButton

        /// <summary>
        /// The show drop down button property
        /// </summary>
        public static readonly DependencyProperty ShowDropDownButtonProperty = DependencyProperty.Register("ShowDropDownButton", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(true));
        /// <summary>
        /// Gets or sets a value indicating whether [show drop down button].
        /// </summary>
        /// <value><c>true</c> if [show drop down button]; otherwise, <c>false</c>.</value>
        public bool ShowDropDownButton
        {
            get
            {
                return (bool)GetValue(ShowDropDownButtonProperty);
            }
            set
            {
                SetValue(ShowDropDownButtonProperty, value);
            }
        }

        #endregion //ShowDropDownButton

        #region StandardColors

        /// <summary>
        /// The standard colors property
        /// </summary>
        public static readonly DependencyProperty StandardColorsProperty = DependencyProperty.Register("StandardColors", typeof(ObservableCollection<ColorItem>), typeof(ColorPicker), new UIPropertyMetadata(CreateStandardColors()));
        /// <summary>
        /// Gets or sets the standard colors.
        /// </summary>
        /// <value>The standard colors.</value>
        public ObservableCollection<ColorItem> StandardColors
        {
            get
            {
                return (ObservableCollection<ColorItem>)GetValue(StandardColorsProperty);
            }
            set
            {
                SetValue(StandardColorsProperty, value);
            }
        }

        #endregion //StandardColors

        #region StandardColorsHeader

        /// <summary>
        /// The standard colors header property
        /// </summary>
        public static readonly DependencyProperty StandardColorsHeaderProperty = DependencyProperty.Register("StandardColorsHeader", typeof(string), typeof(ColorPicker), new UIPropertyMetadata("标准颜色"));
        /// <summary>
        /// Gets or sets the standard colors header.
        /// </summary>
        /// <value>The standard colors header.</value>
        public string StandardColorsHeader
        {
            get
            {
                return (string)GetValue(StandardColorsHeaderProperty);
            }
            set
            {
                SetValue(StandardColorsHeaderProperty, value);
            }
        }

        #endregion //StandardColorsHeader

        #region UsingAlphaChannel

        /// <summary>
        /// The using alpha channel property
        /// </summary>
        public static readonly DependencyProperty UsingAlphaChannelProperty = DependencyProperty.Register("UsingAlphaChannel", typeof(bool), typeof(ColorPicker), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnUsingAlphaChannelPropertyChanged)));
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
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnUsingAlphaChannelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker colorPicker = (ColorPicker)d;
            if (colorPicker != null)
                colorPicker.OnUsingAlphaChannelChanged();
        }

        /// <summary>
        /// Called when [using alpha channel changed].
        /// </summary>
        private void OnUsingAlphaChannelChanged()
        {
            SelectedColorText = GetFormatedColorString(SelectedColor);
        }

        #endregion //UsingAlphaChannel

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// Initializes static members of the <see cref="ColorPicker" /> class.
        /// </summary>
        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPicker" /> class.
        /// </summary>
        public ColorPicker()
        {
            RecentColors = new ObservableCollection<ColorItem>();
            Keyboard.AddKeyDownHandler(this, OnKeyDown);
            Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, OnMouseDownOutsideCapturedElement);
        }

        #endregion //Constructors

        #region Base Class Overrides

        /// <summary>
        /// 在派生类中重写后，每当应用程序代码或内部进程调用 <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />，都将调用此方法。
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_availableColors != null)
                _availableColors.SelectionChanged -= Color_SelectionChanged;

            _availableColors = GetTemplateChild(PART_AvailableColors) as ListBox;
            if (_availableColors != null)
                _availableColors.SelectionChanged += Color_SelectionChanged;

            if (_standardColors != null)
                _standardColors.SelectionChanged -= Color_SelectionChanged;

            _standardColors = GetTemplateChild(PART_StandardColors) as ListBox;
            if (_standardColors != null)
                _standardColors.SelectionChanged += Color_SelectionChanged;

            if (_recentColors != null)
                _recentColors.SelectionChanged -= Color_SelectionChanged;

            _recentColors = GetTemplateChild(PART_RecentColors) as ListBox;
            if (_recentColors != null)
                _recentColors.SelectionChanged += Color_SelectionChanged;

            if (_popup != null)
                _popup.Opened -= Popup_Opened;

            _popup = GetTemplateChild(PART_ColorPickerPalettePopup) as Popup;
            if (_popup != null)
                _popup.Opened += Popup_Opened;

            _toggleButton = this.Template.FindName(PART_ColorPickerToggleButton, this) as ToggleButton;

            if (_colorModeButton != null)
                _colorModeButton.Click -= new RoutedEventHandler(this.ColorModeButton_Clicked);

            _colorModeButton = this.Template.FindName(PART_ColorModeButton, this) as Button;

            if (_colorModeButton != null)
                _colorModeButton.Click += new RoutedEventHandler(this.ColorModeButton_Clicked);
        }

        /// <summary>
        /// 当未处理的 <see cref="E:System.Windows.Input.Mouse.MouseUp" /> 路由事件在其路由中到达派生自此类的元素时，调用此方法。实现此方法可为此事件添加类处理。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="T:System.Windows.Input.MouseButtonEventArgs" />。事件数据将报告已释放了鼠标按钮。</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            // Close ColorPicker on MouseUp to prevent action of mouseUp on controls behind the ColorPicker.
            if (_selectionChanged)
            {
                CloseColorPicker(true);
                _selectionChanged = false;
            }
        }

        #endregion //Base Class Overrides

        #region Event Handlers

        /// <summary>
        /// Handles the <see cref="E:KeyDown" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyEventArgs" /> instance containing the event data.</param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!IsOpen)
            {
                if (KeyboardUtilities.IsKeyModifyingPopupState(e))
                {
                    IsOpen = true;
                    // Focus will be on ListBoxItem in Popup_Opened().
                    e.Handled = true;
                }
            }
            else
            {
                if (KeyboardUtilities.IsKeyModifyingPopupState(e))
                {
                    CloseColorPicker(true);
                    e.Handled = true;
                }
                else if (e.Key == Key.Escape)
                {
                    CloseColorPicker(true);
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Handles the <see cref="E:MouseDownOutsideCapturedElement" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
        private void OnMouseDownOutsideCapturedElement(object sender, MouseButtonEventArgs e)
        {
            CloseColorPicker(false);
        }

        /// <summary>
        /// Handles the SelectionChanged event of the Color control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs" /> instance containing the event data.</param>
        private void Color_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender;

            if (e.AddedItems.Count > 0)
            {
                var colorItem = (ColorItem)e.AddedItems[0];
                SelectedColor = colorItem.Color;
                UpdateRecentColors(colorItem);
                _selectionChanged = true;
                lb.SelectedIndex = -1; //for now I don't care about keeping track of the selected color
            }
        }

        /// <summary>
        /// Handles the Opened event of the Popup control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Popup_Opened(object sender, EventArgs e)
        {
            if ((_availableColors != null) && ShowAvailableColors)
                FocusOnListBoxItem(_availableColors);
            else if ((_standardColors != null) && ShowStandardColors)
                FocusOnListBoxItem(_standardColors);
            else if ((_recentColors != null) && ShowRecentColors)
                FocusOnListBoxItem(_recentColors);
        }

        /// <summary>
        /// Focuses the on ListBox item.
        /// </summary>
        /// <param name="listBox">The list box.</param>
        private void FocusOnListBoxItem(ListBox listBox)
        {
            ListBoxItem listBoxItem = (ListBoxItem)listBox.ItemContainerGenerator.ContainerFromItem(listBox.SelectedItem);
            if ((listBoxItem == null) && (listBox.Items.Count > 0))
                listBoxItem = (ListBoxItem)listBox.ItemContainerGenerator.ContainerFromItem(listBox.Items[0]);
            if (listBoxItem != null)
                listBoxItem.Focus();
        }

        /// <summary>
        /// Handles the Clicked event of the ColorModeButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ColorModeButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.ColorMode = (this.ColorMode == ColorMode.ColorPalette) ? ColorMode.ColorCanvas : ColorMode.ColorPalette;
        }

        #endregion //Event Handlers

        #region Events

        /// <summary>
        /// The selected color changed event
        /// </summary>
        public static readonly RoutedEvent SelectedColorChangedEvent = EventManager.RegisterRoutedEvent("SelectedColorChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Color>), typeof(ColorPicker));
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
        /// Closes the color picker.
        /// </summary>
        /// <param name="isFocusOnColorPicker">if set to <c>true</c> [is focus on color picker].</param>
        private void CloseColorPicker(bool isFocusOnColorPicker)
        {
            if (IsOpen)
                IsOpen = false;
            ReleaseMouseCapture();

            if (isFocusOnColorPicker && (_toggleButton != null))
                _toggleButton.Focus();
            this.UpdateRecentColors(new ColorItem(SelectedColor, SelectedColorText));
        }

        /// <summary>
        /// Updates the recent colors.
        /// </summary>
        /// <param name="colorItem">The color item.</param>
        private void UpdateRecentColors(ColorItem colorItem)
        {
            if (!RecentColors.Contains(colorItem))
                RecentColors.Add(colorItem);

            if (RecentColors.Count > 10) //don't allow more than ten, maybe make a property that can be set by the user.
                RecentColors.RemoveAt(0);
        }

        /// <summary>
        /// Gets the formated color string.
        /// </summary>
        /// <param name="colorToFormat">The color to format.</param>
        /// <returns>System.String.</returns>
        private string GetFormatedColorString(Color colorToFormat)
        {
            return ColorUtilities.FormatColorString(colorToFormat.GetColorName(), UsingAlphaChannel);
        }

        /// <summary>
        /// Creates the standard colors.
        /// </summary>
        /// <returns>ObservableCollection&lt;ColorItem&gt;.</returns>
        private static ObservableCollection<ColorItem> CreateStandardColors()
        {
            ObservableCollection<ColorItem> _standardColors = new ObservableCollection<ColorItem>();
            _standardColors.Add(new ColorItem(Colors.Transparent, "Transparent"));
            _standardColors.Add(new ColorItem(Colors.White, "White"));
            _standardColors.Add(new ColorItem(Colors.Gray, "Gray"));
            _standardColors.Add(new ColorItem(Colors.Black, "Black"));
            _standardColors.Add(new ColorItem(Colors.Red, "Red"));
            _standardColors.Add(new ColorItem(Colors.Green, "Green"));
            _standardColors.Add(new ColorItem(Colors.Blue, "Blue"));
            _standardColors.Add(new ColorItem(Colors.Yellow, "Yellow"));
            _standardColors.Add(new ColorItem(Colors.Orange, "Orange"));
            _standardColors.Add(new ColorItem(Colors.Purple, "Purple"));
            return _standardColors;
        }

        /// <summary>
        /// Creates the available colors.
        /// </summary>
        /// <returns>ObservableCollection&lt;ColorItem&gt;.</returns>
        private static ObservableCollection<ColorItem> CreateAvailableColors()
        {
            ObservableCollection<ColorItem> _standardColors = new ObservableCollection<ColorItem>();

            foreach (var item in ColorUtilities.KnownColors)
            {
                if (!String.Equals(item.Key, "Transparent"))
                {
                    var colorItem = new ColorItem(item.Value, item.Key);
                    if (!_standardColors.Contains(colorItem))
                        _standardColors.Add(colorItem);
                }
            }

            return _standardColors;
        }

        #endregion //Methods
    }
}
