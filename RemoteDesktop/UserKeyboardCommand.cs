using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;

namespace RemoteDesktop
{
    [Serializable]
    public class UserKeyboardCommand : UserCommandBase
    {
        private string key;

        public UserKeyboardCommand(string key)
        {
            this.key = key;
        }

        public override void Execute()
        {
            SendKeys.SendWait(key);
        }

        public override string ToString()
        {
            return "keyboard: " + key;
        }
    }
}
