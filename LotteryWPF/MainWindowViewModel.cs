using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace LotteryWPF
{
   
    public partial class MainWindowViewModel : ObservableObject
    {

        [ObservableProperty]
        private string windowTitle = "Tutorial App";

        [RelayCommand]
        public void ShowMessage()
        {
            MessageBox.Show("Hello world!");
        }
    }
}
