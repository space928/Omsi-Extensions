using OmsiHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiExtensionsUI.ViewModels
{
    public class OmsiStringViewModel
    {
        public string Name { get; set; } = "";
        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
            }
        }

        private string value;

        public OmsiStringViewModel(string name, string value)
        {
            Name = name;
            this.value = value;
        }
    }
}
