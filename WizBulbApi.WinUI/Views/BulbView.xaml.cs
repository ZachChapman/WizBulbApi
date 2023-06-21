using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MvvmFramework.WinUI;

namespace WizBulbApi.WinUI;

public abstract class _BulbView : View<BulbViewModel> { }
public sealed partial class BulbView : _BulbView
{
    public BulbView()
    {
        this.InitializeComponent();

        Loaded += BulbView_Loaded;
    }

    private void BulbView_Loaded(object sender, RoutedEventArgs e)
    {
        ContentFrame.Navigate(typeof(BulbControlsView), ViewModel);
    }

    private void ColourButtonButton_Click(object sender, RoutedEventArgs e)
    {
        //TODO: ToggleOpenPopupWindow<>
    }

    private void ColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
    {

    }

    private void ToggleOpenPopupWindow<TView>(object? parameter = default)
    {

    }
}
