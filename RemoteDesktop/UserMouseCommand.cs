using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDesktop
{
    [Serializable]
    public class UserMouseCommand : UserCommandBase
    {
        public int MouseX { get; }
        public int MouseY { get; }

        public int WheelMovement { get; }

        public MouseEventFlag MouseEventFlag { get; }

        public UserMouseCommand(int mouseX, int mouseY, int wheelMovement, MouseEventFlag flag)
        {
            this.MouseX = mouseX;
            this.MouseY = mouseY;
            this.WheelMovement = wheelMovement;
            this.MouseEventFlag = flag;
        }

        public override void Execute()
        {
            UserMouseEvent.SetCursorPos(this.MouseX, this.MouseY);
            UserMouseEvent.RaiseMouseEvent(MouseEventFlag.Absolute | this.MouseEventFlag, this.MouseX, this.MouseY, this.WheelMovement);
        }

        public override string ToString()
        {
            string mouseCommand = this.MouseEventFlag.ToString() + " at (" + this.MouseX + "," + this.MouseY + ")";
            return mouseCommand;
        }
    }
}
