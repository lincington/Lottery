using Common.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using ScottPlot;
using ScottPlot.WPF;


namespace CommonModules.LotteryModule
{
    public partial class LotteryHistoryCtrlViewModel : ObservableObject, IModule
    {
        public string ModuleName => "历史数据";

        public ObservableObject GetViewModel() => this;

        [ObservableProperty]
        private WpfPlot _red;

        public LotteryHistoryCtrlViewModel()
        {
            _red = new WpfPlot();

            List<double> dataX = new List<double>();
            List<double> dataY = new List<double>();
            List<double> dataYY = new List<double>();
            List<double> dataAVYY = new List<double>();
            List<double> dataAVY = new List<double>();
            for (int i = 1; i < 34; i++)
            {
                dataX.Add(i);
                dataY.Add(0);
                dataAVY.Add(0);
                if (i < 17)
                {
                    dataYY.Add(0);
                    dataAVYY.Add(0);
                }
            }
            
            Red.Plot.Clear();

            var sp1 = Red.Plot.Add.ScatterPoints(dataX.ToArray(), dataY.ToArray()); // markerSize定义marker大小
            sp1.MarkerShape = MarkerShape.Asterisk; // 空心圆
            sp1.MarkerSize = 10; // markerSize定义marker大小

            sp1 = Red.Plot.Add.ScatterPoints(dataX.ToArray(), dataYY.ToArray());
            sp1.MarkerShape = MarkerShape.Asterisk; // 空心圆
            sp1.MarkerSize = 10; // markerSize定义marker大小

            Red.Plot.Add.HorizontalLine(dataY.Sum() / 33);
            Red.Plot.Add.HorizontalLine(dataYY.Sum() / 16);

            Red.Plot.Axes.AutoScale();
            Red.Refresh();
        }

    }
}
