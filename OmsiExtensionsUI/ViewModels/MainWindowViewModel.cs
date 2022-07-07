using OmsiHook;
using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI.Fody.Helpers;
using System.Windows.Input;
using ReactiveUI;
using OmsiExtensionsUI.Models;
using System.Collections.ObjectModel;
using Avalonia.Threading;
using System.Threading.Tasks;

namespace OmsiExtensionsUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        [Reactive] public int PlayerBus { get; private set; }
        [Reactive] public string Search { get; set; }
        [Reactive] public ObservableCollection<OmsiFloatModel> Floats { get; private set; } = new();
        [Reactive] public ObservableCollection<OmsiStringModel> SVars { get; private set; } = new();
        public ICommand ConnectToOmsiCommand { get; private set; }

        private Models.OmsiModel omsiModel;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        // Disabled warning given they are set in CreateCommands() and UpdateData().
        public MainWindowViewModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            CreateCommands();
        }

        private void CreateCommands()
        {
            ConnectToOmsiCommand = ReactiveCommand.Create(() => ConnectToOmsi());
        }

        async private void ConnectToOmsi()
        {
            omsiModel ??= new Models.OmsiModel(this);
            await Dispatcher.UIThread.InvokeAsync(omsiModel.Connect, DispatcherPriority.Background);

            while (true)
            {
                await Dispatcher.UIThread.InvokeAsync(() => {
                    var vehicle = omsiModel.OmsiHook.Globals.PlayerVehicle;
                    var VarStrings = vehicle.ComplMapObj.VarStrings;
                    var keys = new List<string>(VarStrings.IndexDictionary.Keys);
                    var pv = vehicle.PublicVars.WrappedArray;
                    for (int i = 0; i < keys.Count; i++)
                    {
                        if (Floats.Count > i)
                        {
                            Floats[i].Value = pv[VarStrings[keys[i]]].Float;
                            
                        }
                        else
                        {
                            Floats.Add(new(keys[i], pv[VarStrings[keys[i]]].Float, vehicle));
                        }
                    }

                    /*VarStrings = vehicle.ComplMapObj.SVarStrings;
                    keys = new List<string>(VarStrings.IndexDictionary.Keys);
                    var pv2 = vehicle.ComplObjInst.StringVars.WrappedArray;
                    for (int i = 0; i < keys.Count; i++)
                    {
                        this.VM.SVars.Add(new(keys[i], pv2[VarStrings[keys[i]]].String, vehicle));
                    }*/
                }, DispatcherPriority.Background);
                await Task.Delay(50);
            }

        }
    }
}
