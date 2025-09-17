using Common;
using CommonModules.LotteryModule;
using ScottPlot;
using ScottPlot.WPF;
using System.Threading.Tasks;
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
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Application.Current.Dispatcher.BeginInvoke(new Action(delegate
            {
            LotteryReactiveCtrl lotteryReactiveCtrl = new LotteryReactiveCtrl();
            lotteryReactiveCtrl.Show();

             }));
           
        }
    }
}