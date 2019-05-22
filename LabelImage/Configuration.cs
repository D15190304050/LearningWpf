using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelImage
{
    public class Configuration
    {
        private int uiFontSize;

        public int UIFontSize
        {
            get { return uiFontSize; }
            set { uiFontSize = value; }
        }

        public Configuration()
        {
            uiFontSize = 16;
        }
    }
}
