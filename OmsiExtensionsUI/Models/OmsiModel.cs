using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmsiHook;

namespace OmsiExtensionsUI.Models
{
    public class OmsiModel
    {
        public OmsiHook.OmsiHook OmsiHook { get; private set; }

        public OmsiModel()
        {
            this.OmsiHook = new OmsiHook.OmsiHook();
            this.OmsiHook.AttachToOMSI();
        }
    }
}
