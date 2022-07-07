using OmsiHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiExtensionsUI.Models
{
    public class OmsiStringModel
    {
        public string Name { get; set; } = "";
        internal string ValueInternal;
        public string Value {
            get { return ValueInternal; }
            set {
                ValueInternal = value;
                PlayerVehicle.SetStringVariable(Name, value);
            } 
        }
        private OmsiRoadVehicleInst PlayerVehicle;
        public OmsiStringModel(string name, string value, OmsiRoadVehicleInst playerVehicle)
        {
            Name = name;
            ValueInternal = value;
            PlayerVehicle = playerVehicle;
        }
    }
}
