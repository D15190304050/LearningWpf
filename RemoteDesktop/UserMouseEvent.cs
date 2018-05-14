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
        private static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref System.Drawing.Point point);

        [Flags]
        private enum MouseEventFlag : uint
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            XDown = 0x0080,
            XUp = 0x0100,
            Wheel = 0x0800,
            VirtualDesk = 0x4000,
            Absolute = 0x8000
        }

        public static void MouseLeftClick(int dx, int dy, uint data)
        {
            mouse_event(MouseEventFlag.LeftDown | MouseEventFlag.Absolute, dx, dy, data, UIntPtr.Zero);
            mouse_event(MouseEventFlag.LeftUp | MouseEventFlag.Absolute, dx, dy, data, UIntPtr.Zero);
        }

        public static void MouseRightClick(int dx, int dy, uint data)
        {
            mouse_event(MouseEventFlag.RightDown | MouseEventFlag.Absolute, dx, dy, data, UIntPtr.Zero);
            mouse_event(MouseEventFlag.RightUp | MouseEventFlag.Absolute, dx, dy, data, UIntPtr.Zero);
        }
    }
}
