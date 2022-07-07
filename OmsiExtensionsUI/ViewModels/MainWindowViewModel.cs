using OmsiHook;
using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI.Fody.Helpers;
using System.Windows.Input;
using ReactiveUI;

namespace OmsiExtensionsUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";
        [Reactive] public string PlayerBusPosition { get; private set; }
        public ICommand UpdateDataCommand { get; private set; }

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
            UpdateDataCommand = ReactiveCommand.Create(() => UpdateData());
        }

        private void UpdateData()
        {
            omsiModel ??= new Models.OmsiModel();

            var pos = omsiModel.OmsiHook.Globals.PlayerVehicle.Position;
            PlayerBusPosition = $"Player Bus Position = [{pos.x}, {pos.y}, {pos.z}]";
        }
    }
}
