using Common;
using Common.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using Org.BouncyCastle.Ocsp;
using ScottPlot.WPF;
using System.Windows.Threading;

namespace CommonModules.LotteryModule
{
    public partial class LotteryRealCtrlViewModel : ObservableObject, IModule
    {
        public string ModuleName => "实时数据";

        public ObservableObject GetViewModel() => this;

        [ObservableProperty]
        private  WpfPlot _red;


        DispatcherTimer readDataTimer = new  DispatcherTimer();
  
        List<double> dataX = new List<double> () ;
        List<double> dataY = new List<double>();
        List<double> dataYY = new List<double>();
        public LotteryRealCtrlViewModel()
        {
            _red = new WpfPlot();
            for (int i = 1; i < 34; i++)
            {
                dataX.Add(i);
                dataY.Add(0);
                if (i < 17)
                {
                    dataYY.Add(0);
                }
            }     
            readDataTimer.Tick += ReadDataTimer_Tick;
            readDataTimer.Interval = new TimeSpan(0, 0, 0, 1);
            readDataTimer.Start();
        }

        private void ReadDataTimer_Tick(object? sender, EventArgs e)
        {
            IEnumerable<Lottery> collection =   DoubleColorBallGenerator.GenerateSQLTickets(33*16).ToList();

            foreach (var item in collection)
            {
                dataY[item.R1 - 1]++;  
                dataY[item.R2 - 1]++;
                dataY[item.R3 - 1]++;
                dataY[item.R4 - 1]++;
                dataY[item.R5 - 1]++;
                dataY[item.R6 - 1]++;
                dataYY[item.B1 - 1]++;
            }
            Red.Plot.Clear();
            Red.Plot.Add.Scatter(dataX.ToArray(), dataY.ToArray());
            Red.Plot.Add.Scatter(dataX.ToArray(), dataYY.ToArray());
            Red.Plot.Axes.AutoScale();
            Red.Refresh();
        }
    }
}
