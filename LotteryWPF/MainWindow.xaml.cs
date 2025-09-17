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
        
        //
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
          DataContext = viewModel;

        }

    }
}