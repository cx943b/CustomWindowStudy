using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomWindowStudy
{
    public enum WindowTitleBarButtonFlag : short
    {
        None = 0,
        Close = 0x0001,
        Minimize = 0x0010,
        Maximize = 0x0100,
        Restore = 0x1000
    }
}
