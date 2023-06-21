using PInvoke;
using System.Drawing;
using System.Runtime.InteropServices;
using Toolbox;

namespace WizBulbApi.WinUI;

public class NotifyIconManager
{
    private readonly Dictionary<int, (NotifyIconData, Action<WindowMessageEventArgs>)> _onClickCallbacks = new();
    private readonly IntPtr _windowHandle;
    private readonly WindowProcessHook _windowProcessHook;
    private int _nextCallbackMessage = (int)User32.WindowMessage.WM_APP + 100;

    public NotifyIconManager(IntPtr windowHandle)
    {
        _windowHandle = windowHandle;
        _windowProcessHook = WindowProcessHook.GetOrCreate(_windowHandle);
        _windowProcessHook.AddWindowMessageCallback(WndProc);
    }

    public int AddNotifyIcon(string iconPath, Action<WindowMessageEventArgs> onClick)
    {
        var id = _nextCallbackMessage++;

        var notifyIconData = new NotifyIconData
        {
            hWnd = _windowHandle,
            cbSize = Marshal.SizeOf<NotifyIconData>(),
            uFlags = NotifyFlags.NIF_ICON | NotifyFlags.NIF_MESSAGE,
            hIcon = new Icon(iconPath).Handle,
            uCallbackMessage = id,
        };

        _onClickCallbacks.Add(id, (notifyIconData, onClick));

        Shell_NotifyIcon((int)NotifyIconMessage.NIM_ADD, ref notifyIconData);

        return id;
    }

    public void RemoveNotifyIcon(int id)
    {
        if(!_onClickCallbacks.ContainsKey(id))
        {
            return;
        }

        var data = _onClickCallbacks[id];

        _onClickCallbacks.Remove(id);

        Shell_NotifyIcon((int)NotifyIconMessage.NIM_DELETE, ref data.Item1);
    }

    private void WndProc(WindowMessageEventArgs args)
    {
        if(_onClickCallbacks.ContainsKey((int)args.WindowMessage))
        {
            var data = _onClickCallbacks[(int)args.WindowMessage];

            data.Item2.Invoke(args);
        }
    }


    [DllImport("shell32.dll")]
    static extern bool Shell_NotifyIcon(uint dwMessage, [In] ref NotifyIconData pnid);

    public enum NotifyIconMessage : int
    {
        NIM_ADD = 0x00000000,
        NIM_MODIFY = 0x00000001,
        NIM_DELETE = 0x00000002,
        NIM_SETFOCUS = 0x00000003,
        NIM_SETVERSION = 0x00000004,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NotifyIconData
    {
        public int cbSize; // DWORD
        public IntPtr hWnd; // HWND
        public int uID; // UINT
        public NotifyFlags uFlags; // UINT
        public int uCallbackMessage; // UINT
        public IntPtr hIcon; // HICON
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szTip; // char[128]
        public int dwState; // DWORD
        public int dwStateMask; // DWORD
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szInfo; // char[256]
        public int uTimeoutOrVersion; // UINT
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string szInfoTitle; // char[64]
        public int dwInfoFlags; // DWORD
        //GUID guidItem; > IE 6
    }

    [Flags]
    public enum NotifyFlags
    {
        NIF_MESSAGE = 0x01,
        NIF_ICON = 0x02,
        NIF_TIP = 0x04,
        NIF_STATE = 0x08,
        NIF_INFO = 0x10,
        NIF_GUID = 0x20,
        NIF_REALTIME = 0x40,
        NIF_SHOWTIP = 0x80,
    }
}