using Common;
using Common.Contracts;
using Common.DBHelper;
using Common.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MathNet.Numerics.Distributions;
using ScottPlot;
using ScottPlot.WPF;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace CommonModules.LotteryModule
{
    public partial class LotteryBuleCtrlViewModel : ObservableObject, IModule
    {
        public string ModuleName => "蓝球数据";
        public ObservableObject GetViewModel() => this;


        [ObservableProperty]
        private ObservableCollection<AVGData> _avg;

        public LotteryBuleCtrlViewModel()
        {
            _avg = new ObservableCollection<AVGData>();            
        }

        [RelayCommand]
        public void Edit()
        {
            MessageBox.Show("编辑蓝球数据");
            LoadData();
        }


        public void LoadData()
        {
            Avg.Add(new AVGData() {  
             A=4132.41f,
              H=27,
               ID="蓝球",
                L=1,
                 S= 16
            });
        }
    }
}
