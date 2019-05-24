using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelImage
{
    public class Configuration
    {
        [Bindable(true)]
        [Category("Behavior")]
        public int UIFontSize { get; set; }

        public Configuration()
        {
            this.UIFontSize = 16;
        }
    }
}
