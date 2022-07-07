using OmsiHook;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiExtensionsUI.Models
{
    public class OmsiFloatModel : ReactiveObject
    {
        public string Name { get; set; } = "";
        public float value;
        [Reactive]public float Value
        {
            get { return value; }
            set
            {
                this.value = value;
                //PlayerVehicle.SetVariable(Name, value);
            }
        }
        private OmsiRoadVehicleInst PlayerVehicle;
        [Reactive] public bool ShowInGraph { get; set; }
        public OmsiFloatModel(string name, float value, OmsiRoadVehicleInst playerVehicle)
        {
            Name = name;
            this.value = value;
            PlayerVehicle = playerVehicle;
        }
    }
}
