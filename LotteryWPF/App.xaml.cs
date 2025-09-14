using Common.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace LotteryWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private IHost? _host;


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

            //// 业务服务
            //services.AddTransient<IUserService, UserService>();

            //// 主程序
            //services.AddTransient<MainViewModel>();
            //services.AddTransient<MainWindow>();

            //// 模块注册
            //services.AddTransient<IModule, UserListViewModel>();
            //services.AddTransient<UserListViewModel>();

            //// 其他模块
            //services.AddTransient<DashboardViewModel>();
        }
    }

}
