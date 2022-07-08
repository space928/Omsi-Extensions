using OmsiHook;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiExtensionsUI.ViewModels
{
    public class OmsiFloatViewModel : ReactiveObject
    {
        public string Name { get; set; } = "";
        [Reactive]
        public float Value
        {
            get { return value; }
            set { this.value = value; }
        }
        [Reactive] public bool ShowInGraph { get; set; }

        private float value;

        public OmsiFloatViewModel(string name, float value)
        {
            Name = name;
            this.value = value;
        }
    }
}
