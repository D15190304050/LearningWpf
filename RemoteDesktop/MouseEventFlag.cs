using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDesktop
{
    /// <summary>
    /// The MouseEventFlag enum indicates which mouse event happens.
    /// </summary>
    [Flags]
    public enum MouseEventFlag : uint
    {
        /// <summary>
        /// Movement occurred.
        /// </summary>
        Move = 0x0001,

        /// <summary>
        /// The left button was pressed.
        /// </summary>
        LeftDown = 0x0002,

        /// <summary>
        /// The left button was released.
        /// </summary>
        LeftUp = 0x0004,

        /// <summary>
        /// The right button was pressed.
        /// </summary>
        RightDown = 0x0008,

        /// <summary>
        /// The right button was released.
        /// </summary>
        RightUp = 0x0010,

        /// <summary>
        ///The middle button was pressed.
        /// </summary>
        MiddleDown = 0x0020,

        /// <summary>
        /// The middle button was released.
        /// </summary>
        MiddleUp = 0x0040,

        /// <summary>
        /// An X button was pressed.
        /// </summary>
        XDown = 0x0080,

        /// <summary>
        /// An X button was released.
        /// </summary>
        XUp = 0x0100,

        /// <summary>
        /// The wheel was moved, if the mouse has a wheel. The amount of movement is specified in mouseData.
        /// </summary>
        Wheel = 0x0800,

        /// <summary>
        /// Maps coordinates to the entire desktop. Must be used with MouseEventFlag.Absolute.
        /// </summary>
        VirtualDesk = 0x4000,

        /// <summary>
        /// The dx and dy members contain normalized absolute coordinates. If the flag is not set, dxand dy contain relative data (the change in position since the last reported position).
        /// </summary>
        Absolute = 0x8000
    }
}
