using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace LotteryWPF
{
   
    public partial class MainWindowViewModel : ObservableObject
    {

        [ObservableProperty]
        private string windowTitle = "Tutorial App";

        [ObservableProperty]
        private string currentNameInfo="";

        [ObservableProperty]
        private string currentClassInfo = "";

        [ObservableProperty]
        private string currentPhoneInfo = "";



        [RelayCommand]
        public void ShowMessage()
        {
            MessageBox.Show("Hello world!");
        }
    }
}
