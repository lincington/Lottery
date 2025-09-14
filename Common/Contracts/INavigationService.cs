namespace Common.Contracts
{
    public interface INavigationService
    {
        void NavigateTo<T>() where T : ObservableObject;
        void NavigateTo(ObservableObject viewModel);
        ObservableObject? CurrentViewModel { get; }
        event EventHandler? CurrentViewModelChanged;
    }
}
