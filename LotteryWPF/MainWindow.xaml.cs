using Common;
using ScottPlot;
using ScottPlot.WPF;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Color = ScottPlot.Color;
using Colors = ScottPlot.Colors;




namespace LotteryWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// 正态分布类
        /// </summary>
        StandardDistribution standardDistribution1 = null;

        public MainWindow()
        {
            InitializeComponent();
            // DataContext = new MainWindowViewModel();
        }

        /// <summary>
        /// 显示曲线
        /// </summary>
        /// <param name="standardDistribution">正泰分布对象</param>
        /// <param name="WpfPlot">曲线名字</param>
        private void PlotData(StandardDistribution standardDistribution, WpfPlot WpfPlot)
        {
            List<double> xList = new List<double>();
            double XBarValue = standardDistribution.XsMin;
            xList.Add(XBarValue - standardDistribution.GroupLenth);
            for (int i = 0; i < standardDistribution.GroupCount; i++)
            {
                xList.Add(XBarValue);
                XBarValue = XBarValue + standardDistribution.GroupLenth;
            }
            xList.Add(standardDistribution.XsMax + standardDistribution.GroupLenth);

            List<double> YList = new List<double>();
            for (int i = 0; i < xList.Count; i++)
            {
                if (i + 1 < xList.Count)
                {
                    var Count = standardDistribution.XDatas.Count(n => n >= xList[i] && n < xList[i + 1]);
                    YList.Add(Count);
                }
            }
            List<Bar> barList = new List<Bar>();

            for (int i = 0; i < xList.Count; i++)
            {
                if (i + 1 < xList.Count)
                {
                    var Count = (xList[i] + xList[i + 1]) / 2;
                    Bar bar = new Bar();
                    bar.Position = Count;
                    bar.Value = YList[i];
                    bar.Size = standardDistribution.GroupLenth;
                    bar.FillColor = Color.FromHex("#1F77B4");
                    barList.Add(bar);
                }
            }

            var barPlot = WpfPlot.Plot.Add.Bars(barList.ToArray());
            // define the content of labels
            foreach (var bar in barPlot.Bars)
            {
                bar.Label = bar.Value.ToString();
            }
            barPlot.ValueLabelStyle.Bold = true;
            barPlot.ValueLabelStyle.FontSize = 18;
            //纵坐标系_给频率用
            barPlot.Axes.YAxis = WpfPlot.Plot.Axes.Left;

            var result222 = standardDistribution.GetGaussianDistributionYs().OrderBy(x => x.Item1).ToList(); ;
            double[] doublesx = result222.Select(x => x.Item1).ToArray();
            double[] doublesY = result222.Select(x => x.Item2).ToArray();
            var sp = WpfPlot.Plot.Add.Scatter(doublesx, doublesY);
            sp.LineColor = Colors.Red;
            //把折线平滑
            sp.Smooth = true;
            sp.LegendText = $"N(u:{standardDistribution.Average.ToString("0.00")},σ:{standardDistribution.StandardVariance.ToString("0.00")})";
            sp.LineWidth = 3;
            sp.MarkerSize = 0;
            //纵坐标系_给正态分布值用
            sp.Axes.YAxis = WpfPlot.Plot.Axes.Right;
            WpfPlot.Plot.Axes.Margins(bottom: 0);
            WpfPlot.Plot.ShowLegend(Alignment.UpperLeft);
            WpfPlot.Plot.XLabel("样本数据");
            WpfPlot.Plot.YLabel("频率");
            WpfPlot.Plot.Title("正态分布");
            WpfPlot.Plot.Font.Automatic();
            WpfPlot.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button  = sender as Button;

            switch (button.Content)
            {
                case "R1":
                    {
                        SqliteHelper sqliteHelper = new SqliteHelper();
                        List<double> doubles = sqliteHelper.GetAllLotteries().Select(x => (double)x.R1).ToList();
                        //正泰分布类，传入样本数据和要分组的个数
                        standardDistribution1 = new StandardDistribution(doubles, 33 - 6);
                        //显示曲线方法
                        PlotData(standardDistribution1, WpfPlot1);
                    }
                    break;

                case "R2":
                    {
                        SqliteHelper sqliteHelper = new SqliteHelper();
                        List<double> doubles = sqliteHelper.GetAllLotteries().Select(x => (double)x.R2).ToList();
                        //正泰分布类，传入样本数据和要分组的个数
                        standardDistribution1 = new StandardDistribution(doubles, 33-6);
                        //显示曲线方法
                        PlotData(standardDistribution1, WpfPlot2);
                    }
                    break;
                case "R3":
                    {
                        SqliteHelper sqliteHelper = new SqliteHelper();
                        List<double> doubles = sqliteHelper.GetAllLotteries().Select(x => (double)x.R3).ToList();

                        //正泰分布类，传入样本数据和要分组的个数
                        standardDistribution1 = new StandardDistribution(doubles, 33 - 6);
                        //显示曲线方法
                        PlotData(standardDistribution1, WpfPlot3);
                    }
                    break;
                case "R4":
                    {
                        SqliteHelper sqliteHelper = new SqliteHelper();
                        List<double> doubles = sqliteHelper.GetAllLotteries().Select(x => (double)x.R4).ToList();

                        //正泰分布类，传入样本数据和要分组的个数
                        standardDistribution1 = new StandardDistribution(doubles, 33 - 6);
                        //显示曲线方法
                        PlotData(standardDistribution1, WpfPlot4);
                    }
                    break;
                case "R5":
                    {
                        SqliteHelper sqliteHelper = new SqliteHelper();
                        List<double> doubles = sqliteHelper.GetAllLotteries().Select(x => (double)x.R5).ToList();
                
                        //正泰分布类，传入样本数据和要分组的个数
                        standardDistribution1 = new StandardDistribution(doubles, 33 - 6);
                        //显示曲线方法
                        PlotData(standardDistribution1, WpfPlot5);
                    }
                    break;
                case "R6":
                    {
                        SqliteHelper sqliteHelper = new SqliteHelper();
                        List<double> doubles = sqliteHelper.GetAllLotteries().Select(x => (double)x.R6).ToList();
           
                        //正泰分布类，传入样本数据和要分组的个数
                        standardDistribution1 = new StandardDistribution(doubles, 33 - 6);
                        //显示曲线方法
                        PlotData(standardDistribution1, WpfPlot6);
                    }
                    break;
                case "B1":
                    {
                        SqliteHelper sqliteHelper = new SqliteHelper();
                        List<double> doubles = sqliteHelper.GetAllLotteries().Select(x => (double)x.B1).ToList();
                        //正泰分布类，传入样本数据和要分组的个数
                        standardDistribution1 = new StandardDistribution(doubles, 16);
                        //显示曲线方法
                        PlotData(standardDistribution1, WpfPlot7);
                    }
                    break;

            }
        }
    }
}