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
        public string Key { get; set; }

        public UserKeyboardCommand(string key)
        {
            this.Key = key;
        }

        public override void Execute()
        {
            SendKeys.SendWait(Key);
        }

        public override string ToString()
        {
            return "keyboard: " + Key;
        }
    }
}
