using Microsoft.UI.Xaml;
using MvvmFramework.WinUI;

namespace WizBulbApi.WinUI;

public static class Grid
{
    #region AutoColumn

    public static bool GetAutoColumn(DependencyObject obj) => (bool)obj.GetValue(AutoColumnProperty);
    public static void SetAutoColumn(DependencyObject obj, bool value) => obj.SetValue(AutoColumnProperty, value);

    public static readonly DependencyProperty AutoColumnProperty = DependencyPropertyHelper.AutoRegisterAttached("AutoColumn", new(false, OnAutoColumnChanged));

    private static void OnAutoColumnChanged(object sender, DependencyPropertyChangedEventArgs args)
    {
        if(args.NewValue is not true) return;

        var grid = (Microsoft.UI.Xaml.Controls.Grid)sender;
        grid.WhenLoaded(() =>
        {
            var children = grid.Children;
            for(var i = 0; i < children.Count; i++)
            {
                var child = children[i];
                child.SetValue(Microsoft.UI.Xaml.Controls.Grid.ColumnProperty, i);
            }
        });
    }

    #endregion
}