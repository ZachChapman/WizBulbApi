using MvvmFramework.WinUI;
using System.Drawing;

namespace WizBulbApi.WinUI;

public class ScreenSampler
{
    public Color GetAverageColour(Point[] sampleCoords)
    {
        return GetAverageColour(IntPtr.Zero, sampleCoords);
    }

    public Color GetDesktopAverageColour(Point[] sampleCoords)
    {
        return GetAverageColour(PInvoke.User32.GetDesktopWindow(), sampleCoords);
    }

    public Color GetAverageColour(IntPtr hWnd, Point[] sampleCoords)
    {
        var r = 0;
        var g = 0;
        var b = 0;
        var total = 0;

        foreach(var coord in sampleCoords)
        {
            var color = GetPixelColour(hWnd, coord.X, coord.Y);

            r += color.R;
            g += color.G;
            b += color.B;

            total++;
        }

        if(total == 0)
        {
            return Color.White;
        }

        return Color.FromArgb(
           r / total,
           g / total,
           b / total);
    }

    public Color GetPixelColour(int x, int y)
    {
        return GetPixelColour(IntPtr.Zero, x, y);
    }

    public Color GetPixelColour(IntPtr hWnd, int x, int y)
    {
        var desktopPtr = WindowInterop.GetDC(hWnd);
        var pixel = WindowInterop.GetPixel(desktopPtr, x, y);
        WindowInterop.ReleaseDC(IntPtr.Zero, desktopPtr);

        return
            Color.FromArgb(
            (int)(pixel & 0x000000FF),
            (int)(pixel & 0x0000FF00) >> 8,
            (int)(pixel & 0x00FF0000) >> 16);
    }
}
