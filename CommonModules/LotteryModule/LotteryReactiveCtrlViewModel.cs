using Common.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using ScottPlot.WPF;
using System.Reactive;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace CommonModules.LotteryModule
{
    public partial class LotteryReactiveCtrlViewModel : ObservableObject, IModule
    {
        [ObservableProperty]
        private string _name="ffhjj";
  

        [RelayCommand]
        public void SayHello()
        {
            MessageBox.Show("Hello world!");
         

        }

        public string ModuleName => "相应数据";

        public ObservableObject GetViewModel() => this;
    }
}
