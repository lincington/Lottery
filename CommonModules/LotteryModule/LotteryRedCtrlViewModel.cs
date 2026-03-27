using Common.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CommonModules.LotteryModule
{
    public partial class LotteryRedCtrlViewModel : ObservableObject, IModule
    {
        public string ModuleName => "红球数据";
        public ObservableObject GetViewModel() => this;
     
        public LotteryRedCtrlViewModel()
        {
             
        }
    }
}
