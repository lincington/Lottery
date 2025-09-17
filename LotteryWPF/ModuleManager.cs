using Common.Contracts;
using Common.Models;
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
            // 通过反射或手动注册发现模块
            var LotteryCtrl = _serviceProvider.GetRequiredService<LotteryCtrlViewModel>();
            AvailableModules.Add(LotteryCtrl);
            var LotteryRealCtrl = _serviceProvider.GetRequiredService<LotteryRealCtrlViewModel>();
            AvailableModules.Add(LotteryRealCtrl);

            var LotteryReactiveCtrl = _serviceProvider.GetRequiredService<LotteryReactiveCtrlViewModel>();
            AvailableModules.Add(LotteryReactiveCtrl);
        }

        public ObservableObject? GetModuleViewModel(string moduleName)
        {
            var module = AvailableModules.FirstOrDefault(m => m.ModuleName == moduleName);
            return module?.GetViewModel();
        }
    }
}
