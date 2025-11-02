using Common;
using Common.Contracts;
using Common.DBHelper;
using Common.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using ScottPlot;
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
        public LotteryRealCtrlViewModel()
        {
            _red = new WpfPlot();   
            readDataTimer.Tick += ReadDataTimer_Tick;
            readDataTimer.Interval = new TimeSpan(0, 0, 0, 0,300);
            readDataTimer.Start();
        }

        private void ReadDataTimer_Tick(object? sender, EventArgs e)
        {
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

            IEnumerable<Lottery> collection = DoubleColorBallGenerator.GenerateSQLTickets(3352).ToList();

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

            for (int i = 0; i < 33; i++)
            {
                //dataAVY[i]= (dataY.Sum()/33);
            }

            for (int i = 0; i < 16; i++)
            {
              //  dataAVYY[i] = (dataYY.Sum() / 16);
            }
            Red.Plot.Clear();

            var sp1 = Red.Plot.Add.ScatterPoints(dataX.ToArray(), dataY.ToArray()); // markerSize定义marker大小
            sp1.MarkerShape = MarkerShape.Asterisk; // 空心圆
            sp1.MarkerSize = 10; // markerSize定义marker大小
                              
            sp1= Red.Plot.Add.ScatterPoints(dataX.ToArray(), dataYY.ToArray());
            sp1.MarkerShape = MarkerShape.Asterisk; // 空心圆
            sp1.MarkerSize = 10; // markerSize定义marker大小

            Red.Plot.Add.HorizontalLine(dataY.Sum() / 33);
            Red.Plot.Add.HorizontalLine(dataYY.Sum() / 16);
            //Red.Plot.Add.Scatter(dataX.ToArray(), dataAVY.ToArray());
            //Red.Plot.Add.Scatter(dataX.ToArray(), dataAVYY.ToArray());
            Red.Plot.Axes.AutoScale();
            Red.Refresh();
        }
    }
}
