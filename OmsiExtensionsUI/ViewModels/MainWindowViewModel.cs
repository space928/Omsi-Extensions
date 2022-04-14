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

        public MainWindowViewModel()
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

            var pos = omsiModel.OmsiHook.PlayerVehicle.Position;
            PlayerBusPosition = $"Player Bus Position = [{pos.x}, {pos.y}, {pos.z}]";
        }
    }
}
