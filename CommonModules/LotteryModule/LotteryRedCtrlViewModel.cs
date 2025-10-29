using Common;
using Common.Contracts;
using Common.DBHelper;
using CommunityToolkit.Mvvm.ComponentModel;
using ScottPlot;
using ScottPlot.WPF;
using System.Windows.Threading;

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
