using Common.Contracts;
 
using CommonLib;
using CommonModules.LotteryModule;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace LotteryWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // 核心服务
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<ModuleManager>();

         
            //// 主程序
            services.AddTransient<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();


            // 可以注册其他服务
            // services.AddSingleton<IMyService, MyService>();
            // 模块注册
         

            services.AddTransient<IModule, LotteryCtrlViewModel>();
            services.AddTransient<LotteryCtrlViewModel>();

            services.AddTransient<IModule, LotteryRealCtrlViewModel>();
            services.AddTransient<LotteryRealCtrlViewModel>();


            //// 其他模块
            //services.AddTransient<DashboardViewModel>();
        }
    }

}
