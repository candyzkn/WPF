// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Matthew Ward" email="mrward@users.sourceforge.net"/>
//     <version>$Revision: 1965 $</version>
// </file>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows;

namespace MyWPF.Utils
{
    /// <summary>
    /// Runs a process that sends output to standard output and to
    /// standard error.
    /// </summary>
    public class ProcessRunnerHelp
    {
        public static void SetScreen(string path, int screenNo, int waitingTime)
        {
            if (screenNo > Screen.AllScreens.Count())
                screenNo = Screen.AllScreens.Count();
            new Thread(() =>
            {
                int count = 0;
                int sign = 200;
            connect:
                var porcess = GetProcesses(path);
                if (porcess != null)
                {
                    if (porcess.MainWindowHandle == IntPtr.Zero)
                    {
                        Thread.Sleep(sign);
                        count = count + sign;
                        if (count >= waitingTime)
                        {
                            MessageBox.Show("改变程序位置超时", "提示");
                            return;
                        }
                        goto connect;
                    }
                    Screen extendedScreen = Screen.AllScreens[screenNo - 1];
                    var listView = GetMainWindowHandle((uint)porcess.Id);
                    foreach (var intPtr in listView)
                    {
                        var rect = new RECT();
                        GetWindowRect(intPtr, ref rect);
                        ShowWindow(porcess.MainWindowHandle, 1);
                        porcess.Refresh();
                        SetWindowPos(intPtr, HWND_BOTTOM,
                                     extendedScreen.WorkingArea.Width * (screenNo - 1) +
                                     rect.Left % extendedScreen.WorkingArea.Width, rect.Top, rect.Width, rect.Height,
                                     SWP_SHOWWINDOW);
                    }
                }
            }).Start();
        }
        public static List<IntPtr> GetMainWindowHandle(uint processId)
        {
            var result = new List<IntPtr>();
            EnumWindows((hwnd, lParam) =>
            {
                if (GetParent(hwnd) == IntPtr.Zero)
                {
                    uint uiPid;
                    GetWindowThreadProcessId(hwnd, out uiPid);
                    if (uiPid == processId)    // 找到进程对应的主窗口句柄
                    {
                        result.Add(hwnd);   // 把句柄缓存起来
                        return false;   // 返回 false 以终止枚举窗口
                    }
                }
                return true;
            }, 0);
            return result;
        }

        private static Process GetProcesses(string path)
        {
            var list = new List<Process>();
            var allprocess = Process.GetProcesses();
            foreach (var process in allprocess)
            {
                try
                {
                    var fileName = process.MainModule.FileName;
                    if (string.IsNullOrEmpty(fileName)) continue;
                    if (fileName.ToLower().IndexOf(path.ToLower()) == 0)
                    {
                        list.Add(process);
                    }
                }
                catch (Exception)
                {
                }
            }
            try
            {
                return list.FirstOrDefault(s => s.StartTime == list.Max(t => t.StartTime));
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region Win32
        static IntPtr HWND_BOTTOM = new IntPtr(1);
        private static uint SWP_SHOWWINDOW = 0x0040;
        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndlnsertAfter, int X, int Y, int cx, int cy, uint Flags);

        public delegate bool WNDENUMPROC(IntPtr hwnd, uint lParam);
        [DllImport("user32")]
        public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, uint lParam);

        [DllImport("user32.dll", EntryPoint = "GetParent", SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out uint ID);

        [DllImport("user32.dll", EntryPoint = "IsWindow")]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(uint dwErrCode);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        #endregion

    }

    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public RECT(int left_, int top_, int right_, int bottom_)
        {
            Left = left_;
            Top = top_;
            Right = right_;
            Bottom = bottom_;
        }

        public int Height { get { return Bottom - Top; } }
        public int Width { get { return Right - Left; } }
        public Size Size { get { return new Size(Width, Height); } }

        public Point Location { get { return new Point(Left, Top); } }

        public override int GetHashCode()
        {
            return Left ^ ((Top << 13) | (Top >> 0x13))
                   ^ ((Width << 0x1a) | (Width >> 6))
                   ^ ((Height << 7) | (Height >> 0x19));
        }
    }
}