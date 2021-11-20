
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using UnityEngine;
public class WindowAPI : MonoBehaviour
{
    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
    [DllImport("user32.dll")]
    static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint wFlags);

    public Rect screenPosition;
    const uint SWP_SHOWWINDOW = 0x0040;
    const int HWND_TOP = 0;
    const int HWND_TOPMOST = -1;
    const int GWL_STYLE = -16;
    const int WS_BORDER = 1;

    void Start()
    {
        IntPtr ptr = FindWindow(null, "MusicPlayer"); //TestAnimation为程序名，需要替换
        //SetWindowLong(ptr, GWL_STYLE, WS_BORDER);   //窗口全屏
        SetWindowPos(ptr, (IntPtr)HWND_TOPMOST, (int)screenPosition.x, (int)screenPosition.y, (int)screenPosition.width, (int)screenPosition.height, SWP_SHOWWINDOW);   //窗口置顶
    }
}