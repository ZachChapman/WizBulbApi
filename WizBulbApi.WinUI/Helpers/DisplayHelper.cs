using System.Runtime.InteropServices;

namespace WizBulbApi.WinUI;

public static class TaskbarHelper
{
    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

    [DllImport("user32.dll")]
    static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    public static int GetTaskbarHeight()
    {
        var taskbarHandle = FindWindow("Shell_TrayWnd", "");
        var taskbarChildHandle = FindWindowEx(taskbarHandle, IntPtr.Zero, "TrayNotifyWnd", "");

        if(GetWindowRect(taskbarChildHandle, out var taskbarRect))
        {
            return taskbarRect.Bottom - taskbarRect.Top;
        }

        return 0;
    }
}