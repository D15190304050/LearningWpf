using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDesktop
{
    /// <summary>
    /// The UserCommandBase class is an abstract that represents the user mouse and keyboard command.
    /// </summary>
    /// <remarks>
    /// This is a class because this class has the Serializable attribute, which can't be used for interface.
    /// </remarks>
    [Serializable]
    public abstract class UserCommandBase
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        public abstract void Execute();
    }
}
