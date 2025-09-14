using Common.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace LotteryWPF
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
            var userModule = _serviceProvider.GetRequiredService<IModule>();
            AvailableModules.Add(userModule);

            // 可以添加更多模块
            // var productModule = _serviceProvider.GetRequiredService<ProductModule>();
            // AvailableModules.Add(productModule);
        }

        public ObservableObject? GetModuleViewModel(string moduleName)
        {
            var module = AvailableModules.FirstOrDefault(m => m.ModuleName == moduleName);
            return module?.GetViewModel();
        }
    }
}
