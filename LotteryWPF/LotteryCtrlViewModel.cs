using Common.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;


namespace LotteryWPF
{
    internal class LotteryCtrlViewModel : ObservableObject, IModule
    {
        public string ModuleName => "LotteryCtrl";

        public ObservableObject GetViewModel()
        {
            throw new NotImplementedException();
        }
    }
}
