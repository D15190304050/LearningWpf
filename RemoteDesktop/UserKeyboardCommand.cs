using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;

namespace RemoteDesktop
{
    /// <summary>
    /// The UserKeyboardCommand class represents the user key down event.
    /// </summary>
    [Serializable]
    public class UserKeyboardCommand : UserCommandBase
    {
        /// <summary>
        /// The string representation of the key event.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the UserKeyboardCommand with the specified key string.
        /// </summary>
        /// <param name="key">The specified key string.</param>
        public UserKeyboardCommand(string key)
        {
            this.Key = key;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        public override void Execute()
        {
            // Send the key string to current active window and wait for response.
            SendKeys.SendWait(Key);
        }

        /// <summary>
        /// Returns the string representation of this keyboard event.
        /// </summary>
        /// <returns>The string representation of this keyboard event.</returns>
        public override string ToString()
        {
            return "keyboard: " + Key;
        }
    }
}
