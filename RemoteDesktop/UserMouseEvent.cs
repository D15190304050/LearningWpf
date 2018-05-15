using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDesktop
{
    public class UserMouseEvent
    {
        [DllImport("user32.dll")]
        private static extern void mouse_event(MouseEventFlag flags, int dx, int dy, int data, UIntPtr extraInfo);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref System.Drawing.Point point);

        public static void RaiseMouseEvent(MouseEventFlag flags, int mouseX, int mouseY, int wheelMovement)
        {
            mouse_event(flags | MouseEventFlag.Absolute, mouseX, mouseY, wheelMovement, UIntPtr.Zero);
        }

        public static void MouseLeftClick(int dx, int dy, int data)
        {
            mouse_event(MouseEventFlag.LeftDown | MouseEventFlag.Absolute, dx, dy, data, UIntPtr.Zero);
            mouse_event(MouseEventFlag.LeftUp | MouseEventFlag.Absolute, dx, dy, data, UIntPtr.Zero);
        }

        public static void MouseRightClick(int dx, int dy, int data)
        {
            mouse_event(MouseEventFlag.RightDown | MouseEventFlag.Absolute, dx, dy, data, UIntPtr.Zero);
            mouse_event(MouseEventFlag.RightUp | MouseEventFlag.Absolute, dx, dy, data, UIntPtr.Zero);
        }
    }
}
