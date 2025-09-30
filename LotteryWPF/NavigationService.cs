using Common.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using LotteryWPF;

namespace CommonLib
{
    public class NavigationService : INavigationService
    {
        private ObservableObject? _currentViewModel;
 
        public ObservableObject? CurrentViewModel
        {
            get => _currentViewModel;
            private set
            {
                _currentViewModel = value;
                CurrentViewModelChanged?.Invoke(this, EventArgs.Empty);
            }
        }
      
        public event EventHandler? CurrentViewModelChanged;

        public void NavigateTo<T>() where T : ObservableObject
        {
            var viewModel = App.ServiceProvider.GetService(typeof(T)) as ObservableObject;
            CurrentViewModel = viewModel;
        }

        public void NavigateTo(ObservableObject viewModel)
        {
            CurrentViewModel = viewModel;
        }
    }
}
