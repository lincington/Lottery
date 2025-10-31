 using Common.Contracts;
using Common.DBHelper;
using Common.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
 

namespace CommonModules.LotteryModule
{
    public partial class LotteryBuleCtrlViewModel : ObservableObject, IModule
    {
        public string ModuleName => "蓝球数据";
        public ObservableObject GetViewModel() => this;

        [ObservableProperty]
        public ObservableCollection<AvgData> _avg;
        SQLServerHelper sQLServerHelper = new SQLServerHelper();
        public LotteryBuleCtrlViewModel()
        {
            LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);

            _avg = new ObservableCollection<AvgData>();

        }
        public IAsyncRelayCommand LoadDataCommand { get; }

        [RelayCommand]
        public async Task Edit()
        {
             LoadData();
        }
        private async Task LoadDataAsync()
        {
            sQLServerHelper.avgDatas(3371/64,4).ForEach(data =>
            {
                Avg.Add(data);
            });
        }

        public void LoadData()
        {
             sQLServerHelper.avgDatas(3371/64,4).ForEach(data =>
            {
                Avg.Add(data);
            });
        }
    }
}
