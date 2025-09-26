using Common.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
 

namespace CommonModules.LotteryModule
{
    public partial class LotteryReactiveCtrlViewModel : ObservableObject, IModule
    {
        [ObservableProperty]
        private string _name="ffhjj";


        [ObservableProperty]
        private ObservableCollection<TagInfo> _selectedRed;

        [RelayCommand]
        public void SayHello(object ob)
        {
            MessageBox.Show(ob.ToString());
        }

        public LotteryReactiveCtrlViewModel()
        {
            _selectedRed = new ObservableCollection<TagInfo>();
            for (int i = 0; i < 27; i++)
            {
                SelectedRed.Add(new TagInfo() { Name = i.ToString(), Num = i });
            }
        }

        public string ModuleName => "相应数据";

        public ObservableObject GetViewModel() => this;
    }

    public class TagInfo 
    {
        public string Name { get; set; } = "";
        public int Num { get; set; } = 1;

        public RelayCommand<object> SayHelloCommand { get; set; }  = new RelayCommand<object>((ob) => 
        {
            MessageBox.Show(ob.ToString());
        });
    }
}
