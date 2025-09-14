using Common.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;


namespace CommonModules.LotteryModule
{
    public class LotteryCtrlViewModel : ObservableObject, IModule
    {
        public string ModuleName => "历史数据";

        public ObservableObject GetViewModel() => this;
    }
}
