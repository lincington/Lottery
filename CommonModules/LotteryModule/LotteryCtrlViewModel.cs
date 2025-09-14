using Common.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;


namespace CommonModules.LotteryModule
{
    public class LotteryCtrlViewModel : ObservableObject, IModule
    {
        public string ModuleName => "LotteryCtrl";

        public ObservableObject GetViewModel() => this;
    }
}
