using Common.Contracts;
using CommonModules.LotteryModule;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace CommonLib
{
    public class ModuleManager
    {
        private readonly IServiceProvider _serviceProvider;

        public ObservableCollection<IModule> AvailableModules { get; } = new();

        public ModuleManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            DiscoverModules();
        }

        private void DiscoverModules()
        {
            var LotteryCtrl = _serviceProvider.GetRequiredService<LotteryCtrlViewModel>();
            AvailableModules.Add(LotteryCtrl);
            var lotteryCtrl = _serviceProvider.GetRequiredService<lotteryCtrlViewModel>();
            AvailableModules.Add(lotteryCtrl);
            var LotteryReactiveCtrl = _serviceProvider.GetRequiredService<LotteryReactiveCtrlViewModel>();
            AvailableModules.Add(LotteryReactiveCtrl);
            var LotteryHistoryCtrl = _serviceProvider.GetRequiredService<LotteryHistoryCtrlViewModel>();
            AvailableModules.Add(LotteryHistoryCtrl);

            var LotteryRedCtrl = _serviceProvider.GetRequiredService<LotteryRedCtrlViewModel>();
            AvailableModules.Add(LotteryRedCtrl);
            var LotteryBuleCtrl = _serviceProvider.GetRequiredService<LotteryBuleCtrlViewModel>();
            AvailableModules.Add(LotteryBuleCtrl);

        }

        public ObservableObject? GetModuleViewModel(string moduleName)
        {
            var module = AvailableModules.FirstOrDefault(m => m.ModuleName == moduleName);
            return module?.GetViewModel();
        }
    }
}
