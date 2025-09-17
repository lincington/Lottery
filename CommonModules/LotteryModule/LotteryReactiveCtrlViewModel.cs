using ReactiveUI;
using System.Reactive;
using System.Windows;
using System.Windows.Threading;

namespace CommonModules.LotteryModule
{
    public class LotteryReactiveCtrlViewModel: ReactiveObject 
    {

        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private string _TextName;

        public string TextName
        {
            get { return _TextName; }
            set { this.RaiseAndSetIfChanged(ref _TextName, value); }
        }

        static object _lock = new object(); 
        public ReactiveCommand<Unit, IObservable<bool>> SayHelloCommand { get; protected set; }
        public LotteryReactiveCtrlViewModel() 
        {
            lock (_lock)
            {
                SayHelloCommand = ReactiveCommand.Create(() => this.WhenAny(x => x.TextName, x => !string.IsNullOrEmpty(x.Value)));
                SayHelloCommand.Subscribe(_ => MessageBox.Show("You clicked on DisplayCommand: Name is " + TextName));               
            }
        }
    }
}
