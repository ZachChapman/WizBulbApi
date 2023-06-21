using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using MvvmFramework.WinUI;
using Windows.UI.Text;

namespace WizBulbApi.WinUI;

public sealed partial class LabelledTextBlock : UserControl
{
    public LabelledTextBlock()
    {
        this.InitializeComponent();

        DataContext = this;
    }

    public Orientation Orientation
    {
        get { return (Orientation)GetValue(OrientationProperty); }
        set { SetValue(OrientationProperty, value); }
    }
    public readonly static DependencyProperty OrientationProperty = DependencyPropertyHelper.AutoRegister(nameof(Orientation), new PropertyMetadata(Orientation.Horizontal));

    public TextAlignment HorizontalTextAlignment
    {
        get { return (TextAlignment)GetValue(HorizontalTextAlignmentProperty); }
        set { SetValue(HorizontalTextAlignmentProperty, value); }
    }
    public readonly static DependencyProperty HorizontalTextAlignmentProperty = DependencyPropertyHelper.AutoRegister(nameof(HorizontalTextAlignment));

    public string SelectedText
    {
        get { return (string)GetValue(SelectedTextProperty); }
    }
    public readonly static DependencyProperty SelectedTextProperty = DependencyPropertyHelper.AutoRegister(nameof(SelectedText));

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }
    public readonly static DependencyProperty TextProperty = DependencyPropertyHelper.AutoRegister(nameof(Text));

    public TextAlignment TextAlignment
    {
        get { return (TextAlignment)GetValue(TextAlignmentProperty); }
        set { SetValue(TextAlignmentProperty, value); }
    }
    public readonly static DependencyProperty TextAlignmentProperty = DependencyPropertyHelper.AutoRegister(nameof(TextAlignment));

    public TextDecorations TextDecorations
    {
        get { return (TextDecorations)GetValue(TextDecorationsProperty); }
        set { SetValue(TextDecorationsProperty, value); }
    }
    public readonly static DependencyProperty TextDecorationsProperty = DependencyPropertyHelper.AutoRegister(nameof(TextDecorations));

    public TextTrimming TextTrimming
    {
        get { return (TextTrimming)GetValue(TextTrimmingProperty); }
        set { SetValue(TextTrimmingProperty, value); }
    }
    public readonly static DependencyProperty TextTrimmingProperty = DependencyPropertyHelper.AutoRegister(nameof(TextTrimming));

    public TextWrapping TextWrapping
    {
        get { return (TextWrapping)GetValue(TextWrappingProperty); }
        set { SetValue(TextWrappingProperty, value); }
    }
    public readonly static DependencyProperty TextWrappingProperty = DependencyPropertyHelper.AutoRegister(nameof(TextWrapping));


    public string Label
    {
        get { return (string)GetValue(LabelProperty); }
        set { SetValue(LabelProperty, value); }
    }
    public readonly static DependencyProperty LabelProperty = DependencyPropertyHelper.AutoRegister(nameof(Label));

    public Brush LabelForeground
    {
        get { return (Brush)GetValue(LabelForegroundProperty); }
        set { SetValue(LabelForegroundProperty, value); }
    }
    public static readonly DependencyProperty LabelForegroundProperty = DependencyPropertyHelper.AutoRegister(nameof(LabelForeground));

    public Thickness LabelMargin
    {
        get { return (Thickness)GetValue(LabelMarginMarginProperty); }
        set { SetValue(LabelMarginMarginProperty, value); }
    }
    public readonly static DependencyProperty LabelMarginMarginProperty = DependencyPropertyHelper.AutoRegister(nameof(LabelMargin));

    public FontWeight LabelFontWeight
    {
        get { return (FontWeight)GetValue(LabelFontWeightProperty); }
        set { SetValue(LabelFontWeightProperty, value); }
    }
    public static readonly DependencyProperty LabelFontWeightProperty = DependencyPropertyHelper.AutoRegister(nameof(LabelFontWeight), new PropertyMetadata(FontWeights.Normal));
}
