namespace Common.Contracts
{
    public interface IModule
    {
        string ModuleName { get; }
        ObservableObject GetViewModel();
    }
}
