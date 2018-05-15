using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDesktop
{
    [Serializable]
    public abstract class UserCommandBase
    {
        public CommandType CommandType { get; }

        public abstract void Execute();
    }
}
