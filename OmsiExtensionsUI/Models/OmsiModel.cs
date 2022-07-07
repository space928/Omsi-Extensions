using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using OmsiExtensionsUI.ViewModels;
using OmsiHook;

namespace OmsiExtensionsUI.Models
{
    public class OmsiModel
    {
        public OmsiHook.OmsiHook OmsiHook { get; private set; }
        private MainWindowViewModel VM;
        async public void Connect()
        {
            await OmsiHook.AttachToOMSI();
        }
        public OmsiModel(MainWindowViewModel vm)
        {
            this.OmsiHook = new OmsiHook.OmsiHook();
            this.VM = vm;
        }
    }
}
