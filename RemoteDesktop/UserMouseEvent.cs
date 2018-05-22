using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDesktop
{
    /// <summary>
    /// The UserMouseEvent class provides static methods for raise system mouse events using C#.
    /// </summary>
    public static class UserMouseEvent
    {
        /// <summary>
        /// Raises a system mouse event.
        /// </summary>
        /// <param name="flags">The specific mouse event.</param>
        /// <param name="dx">X-coordinate of the mouse.</param>
        /// <param name="dy">Y-coordinate of the mouse.</param>
        /// <param name="data">If flags contains MouseEventFlags.Wheel, then data specifies the amount of wheel movement.</param>
        /// <param name="extraInfo">A set of bit flags that specify various aspects of mouse motion and button clicks. See MSDN for further information.</param>
        /// <remarks>
        ///  A positive value indicates that the wheel was rotated forward, away from the user;
        /// a negative value indicates that the wheel was rotated backward, toward the user. One wheel click is defined as WHEEL_DELTA, which is 120.
        /// </remarks>
        [DllImport("user32.dll")]
        private static extern void mouse_event(MouseEventFlag flags, int dx, int dy, int data, UIntPtr extraInfo);

        /// <summary>
        /// Sets the absolute coordinate of the mouse.
        /// </summary>
        /// <param name="x">X-coordinate of the mouse (absolute).</param>
        /// <param name="y">Y-coordinate of the mouse (absolute).</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        /// <summary>
        /// Gets the coordinate of the mouse.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Nonzero if successful or zero otherwise.</returns>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref System.Drawing.Point point);

        /// <summary>
        /// Raises a mouse event using absolute coordinate.
        /// </summary>
        /// <param name="flags">The specific mouse event.</param>
        /// <param name="mouseX">X-coordinate of the mouse (absolute).</param>
        /// <param name="mouseY">Y-coordinate of the mouse (absolute).</param>
        /// <param name="wheelMovement">The amount of wheel movement.</param>
        public static void RaiseMouseEvent(MouseEventFlag flags, int mouseX, int mouseY, int wheelMovement)
        {
            mouse_event(flags | MouseEventFlag.Absolute, mouseX, mouseY, wheelMovement, UIntPtr.Zero);
        }

        /// <summary>
        /// Simultes the mouse left button click event.
        /// </summary>
        /// <param name="mouseX">X-coordinate of the mouse (absolute).</param>
        /// <param name="mouseY">Y-coordinate of the mouse (absolute).</param>
        public static void MouseLeftClick(int mouseX, int mouseY)
        {
            mouse_event(MouseEventFlag.LeftDown | MouseEventFlag.Absolute, mouseX, mouseY, 0, UIntPtr.Zero);
            mouse_event(MouseEventFlag.LeftUp | MouseEventFlag.Absolute, mouseX, mouseY, 0, UIntPtr.Zero);
        }

        /// <summary>
        /// Simultes the mouse right button click event.
        /// </summary>
        /// <param name="mouseX">X-coordinate of the mouse (absolute).</param>
        /// <param name="mouseY">Y-coordinate of the mouse (absolute).</param>
        public static void MouseRightClick(int mouseX, int mouseY)
        {
            mouse_event(MouseEventFlag.RightDown | MouseEventFlag.Absolute, mouseX, mouseY, 0, UIntPtr.Zero);
            mouse_event(MouseEventFlag.RightUp | MouseEventFlag.Absolute, mouseX, mouseY, 0, UIntPtr.Zero);
        }

        /// <summary>
        /// Simultes the mouse left button double click event.
        /// </summary>
        /// <param name="mouseX">X-coordinate of the mouse (absolute).</param>
        /// <param name="mouseY">Y-coordinate of the mouse (absolute).</param>
        public static void MouseLeftDoubleClick(int mouseX, int mouseY)
        {
            MouseLeftClick(mouseX, mouseY);
            MouseLeftClick(mouseX, mouseY);
        }
    }
}
