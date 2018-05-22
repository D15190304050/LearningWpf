using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDesktop
{
    /// <summary>
    /// The UserMouseCommand class represents the user mouse event.
    /// </summary>
    [Serializable]
    public class UserMouseCommand : UserCommandBase
    {
        /// <summary>
        /// X-coordinate of the mouse (absolute).
        /// </summary>
        public int MouseX { get; }

        /// <summary>
        /// Y-coordinate of the mouse (absolute).
        /// </summary>
        public int MouseY { get; }

        /// <summary>
        /// The amount of wheel movement.
        /// </summary>
        public int WheelMovement { get; }

        /// <summary>
        /// The flag that indicates which mouse event happens.
        /// </summary>
        public MouseEventFlag MouseEventFlag { get; }

        /// <summary>
        /// Initializes a new instance of the UserMouseCommand with specified informatin.
        /// </summary>
        /// <param name="mouseX">X-coordinate of the mouse (absolute).</param>
        /// <param name="mouseY">Y-coordinate of the mouse (absolute).</param>
        /// <param name="wheelMovement">The amount of wheel movement.</param>
        /// <param name="flag">The flag that indicates which mouse event happens.</param>
        public UserMouseCommand(int mouseX, int mouseY, int wheelMovement, MouseEventFlag flag)
        {
            this.MouseX = mouseX;
            this.MouseY = mouseY;
            this.WheelMovement = wheelMovement;
            this.MouseEventFlag = flag;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        public override void Execute()
        {
            // Set the position of the mouse before execution.
            UserMouseEvent.SetCursorPos(this.MouseX, this.MouseY);

            // Execute the command using absolute coordinate.
            if (this.MouseEventFlag != MouseEventFlag.Move)
                UserMouseEvent.RaiseMouseEvent(MouseEventFlag.Absolute | this.MouseEventFlag, this.MouseX, this.MouseY, this.WheelMovement);
        }

        /// <summary>
        /// Returns the string representation of this mouse event, including the flag and coordinate.
        /// </summary>
        /// <returns>the string representation of this mouse event, including the flag and coordinate.</returns>
        public override string ToString()
        {
            string mouseCommand = this.MouseEventFlag.ToString() + " at (" + this.MouseX + "," + this.MouseY + ")";
            return mouseCommand;
        }
    }
}
