using Common.Contracts;
using CommonLib;
using CommonModules.LotteryModule;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace LotteryWPF
{
   
    public partial class MainWindowViewModel : ObservableObject
    {

        [ObservableProperty]
        private string windowTitle = "Tutorial App";

        [ObservableProperty]
        private string currentNameInfo="";

        [ObservableProperty]
        private string currentClassInfo = "";

        [ObservableProperty]
        private string currentPhoneInfo = "";



        [RelayCommand]
        public void ShowMessage()
        {
            MessageBox.Show("Hello world!");
        }

        private readonly INavigationService _navigationService;
        private readonly ModuleManager _moduleManager;

        [ObservableProperty]
        private ObservableObject? _currentModule;

        [ObservableProperty]
        private string _title = "模块化应用程序";

        public ObservableCollection<IModule> Modules => _moduleManager.AvailableModules;

        public MainWindowViewModel(INavigationService navigationService, ModuleManager moduleManager)
        {
            _navigationService = navigationService;
            _moduleManager = moduleManager;

            _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged(object? sender, EventArgs e)
        {
            CurrentModule = _navigationService.CurrentViewModel;
        }

        [RelayCommand]
        private void NavigateToModule(IModule module)
        {
            var viewModel = module.GetViewModel();
            _navigationService.NavigateTo(viewModel);
        }

        [RelayCommand]
        private void ShowDashboard()
        {
            _navigationService.NavigateTo<LotteryCtrlViewModel>();
        }


    }
}
