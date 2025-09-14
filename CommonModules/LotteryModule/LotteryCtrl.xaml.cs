using Common;
using CommonLib;
using ScottPlot;
using ScottPlot.WPF;
using System.Windows;
using System.Windows.Controls;
using Color = ScottPlot.Color;
using Colors = ScottPlot.Colors;

namespace CommonModules.LotteryModule
{
    /// <summary>
    /// LotteryCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class LotteryCtrl : UserControl
    {
        public LotteryCtrl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 正态分布类
        /// </summary>
        StandardDistribution? standardDistribution1 = null;
    
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


        SQLServerHelper sqliteHelper = new SQLServerHelper();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;

            switch (button.Content)
            {
                case "R1":
                    {
                        List<double> doubles = sqliteHelper.GetAllLotteries().Select(x => (double)x.FR1).ToList();
                        //正泰分布类，传入样本数据和要分组的个数
                        standardDistribution1 = new StandardDistribution(doubles, 33);
                        //显示曲线方法
                        PlotData(standardDistribution1, WpfPlot1);
                    }
                    break;

                case "R2":
                    {
                        List<double> doubles = sqliteHelper.GetAllLotteries().Select(x => (double)x.FR2).ToList();
                        //正泰分布类，传入样本数据和要分组的个数
                        standardDistribution1 = new StandardDistribution(doubles, 33);
                        //显示曲线方法
                        PlotData(standardDistribution1, WpfPlot2);
                    }
                    break;
                case "R3":
                    {
                        List<double> doubles = sqliteHelper.GetAllLotteries().Select(x => (double)x.FR3).ToList();
                        //正泰分布类，传入样本数据和要分组的个数
                        standardDistribution1 = new StandardDistribution(doubles, 33);
                        //显示曲线方法
                        PlotData(standardDistribution1, WpfPlot3);
                    }
                    break;
                case "R4":
                    {
                        List<double> doubles = sqliteHelper.GetAllLotteries().Select(x => (double)x.FR4).ToList();
                        //正泰分布类，传入样本数据和要分组的个数
                        standardDistribution1 = new StandardDistribution(doubles, 33);
                        //显示曲线方法
                        PlotData(standardDistribution1, WpfPlot4);
                    }
                    break;
                case "R5":
                    {
                        List<double> doubles = sqliteHelper.GetAllLotteries().Select(x => (double)x.FR5).ToList();
                        //正泰分布类，传入样本数据和要分组的个数
                        standardDistribution1 = new StandardDistribution(doubles, 33);
                        //显示曲线方法
                        PlotData(standardDistribution1, WpfPlot5);
                    }
                    break;
                case "R6":
                    {

                        List<double> doubles = sqliteHelper.GetAllLotteries().Select(x => (double)x.FR6).ToList();

                        //正泰分布类，传入样本数据和要分组的个数
                        standardDistribution1 = new StandardDistribution(doubles, 33);
                        //显示曲线方法
                        PlotData(standardDistribution1, WpfPlot6);
                    }
                    break;
                case "B1":
                    {

                        List<double> doubles = sqliteHelper.GetAllLotteries().Select(x => (double)x.B1).ToList();

                        doubles.Sort();
                        //正泰分布类，传入样本数据和要分组的个数
                        standardDistribution1 = new StandardDistribution(doubles, 16);
                        //显示曲线方法
                        PlotData(standardDistribution1, WpfPlot7);
                    }
                    break;
                case "SUM":
                    {

                        List<double> doubles = sqliteHelper.GetAllLotteriesSum();

                        doubles.Sort();
                        //正泰分布类，传入样本数据和要分组的个数
                        standardDistribution1 = new StandardDistribution(doubles, 33);
                        //显示曲线方法
                        PlotData(standardDistribution1, WpfPlot8);
                    }
                    break;

                case "Min":
                    {

                        List<double> doubles = sqliteHelper.GetAllAsync().Select(x => (double)x.MinM).ToList();

                        doubles.Sort();
                        //正泰分布类，传入样本数据和要分组的个数
                        standardDistribution1 = new StandardDistribution(doubles, 1000);
                        //显示曲线方法
                        PlotData(standardDistribution1, WpfPlot9);
                    }
                    break;

                case "Max":
                    {

                        List<double> doubles = sqliteHelper.GetAllAsync().Select(x => (double)x.MaxM).ToList();

                        doubles.Sort();
                        //正泰分布类，传入样本数据和要分组的个数
                        standardDistribution1 = new StandardDistribution(doubles, 1000);
                        //显示曲线方法
                        PlotData(standardDistribution1, WpfPlot10);
                    }
                    break;
            }
        }




















    }
}
