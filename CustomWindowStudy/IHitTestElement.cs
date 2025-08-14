using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomWindowStudy
{
    public interface IHitTestElement
    {
        public HitTestValues ReservedHitTest { get; set; }
    }
}
