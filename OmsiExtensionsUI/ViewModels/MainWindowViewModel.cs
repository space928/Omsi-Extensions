using OmsiHook;
using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI.Fody.Helpers;
using System.Windows.Input;
using ReactiveUI;
using System.Collections.ObjectModel;
using Avalonia.Threading;
using System.Threading.Tasks;

namespace OmsiExtensionsUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        [Reactive] public int PlayerBus { get; private set; }
        [Reactive] public string Search { get; set; }
        [Reactive] public ObservableCollection<OmsiFloatViewModel> Floats { get; private set; } = new();
        [Reactive] public ObservableCollection<OmsiStringViewModel> SVars { get; private set; } = new();
        public ICommand ConnectToOmsiCommand { get; private set; }
        public ICommand UpdateDataCommand { get; private set; }

        private readonly OmsiHook.OmsiHook omsiHook = new();
        private readonly DispatcherTimer updateTimer;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        // Disabled warning given they are set in CreateCommands() and UpdateData().
        public MainWindowViewModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            CreateCommands();

            updateTimer = new DispatcherTimer(DispatcherPriority.Background);
            updateTimer.Interval = TimeSpan.FromMilliseconds(50);
            updateTimer.Tick += MonitorOmsiTick;
        }

        private void CreateCommands()
        {
            ConnectToOmsiCommand = ReactiveCommand.Create(() => ConnectToOmsi());
            UpdateDataCommand = ReactiveCommand.Create<string>((x) => updateTimer.Interval = TimeSpan.FromMilliseconds(double.Parse(x)));
        }

        async private void ConnectToOmsi()
        {
            await omsiHook.AttachToOMSI();

            ReadVarList();
            updateTimer.Start();
        }

        private void MonitorOmsiTick(object? sender, EventArgs e)
        {
            var vehicle = omsiHook.Globals.PlayerVehicle;
            var publicVars = vehicle.PublicVars.WrappedArray;
            var stringVars = vehicle.ComplObjInst.StringVars.WrappedArray;
            for (int i = 0; i < publicVars.Length; i++)
                Floats[i].Value = publicVars[i].Float;
            for (int i = 0; i < stringVars.Length; i++)
                SVars[i].Value = stringVars[i].String;
        }

        private void ReadVarList()
        {
            var vehicle = omsiHook.Globals.PlayerVehicle;
            var varStrings = vehicle.ComplMapObj.VarStrings;
            var svarStrings = vehicle.ComplMapObj.SVarStrings;

            var pv = vehicle.PublicVars.WrappedArray;
            for (int i = 0; i < varStrings.Count; i++)
                Floats.Add(new(varStrings[i], pv[i].Float));

            var sv = vehicle.ComplObjInst.StringVars;
            for (int i = 0; i < svarStrings.Count; i++)
                SVars.Add(new(svarStrings[i], sv[i].String));
        }
    }
}
