using Common.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CommonModules.LotteryModule
{
    public class LotteryRealCtrlViewModel : ObservableObject, IModule
    {
        public string ModuleName => "实时数据";

        public ObservableObject GetViewModel() => this;
    }
}
