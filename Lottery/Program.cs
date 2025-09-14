namespace Lottery
{
    public class Program
    {     
        static async Task Main(string[] args)
        {
            //var services = new ServiceCollection();
            //// 注册服务
            //services.AddTransient<IVehicle, Car>();
            //services.AddSingleton<IWeapon, Gun>();
            //// 构建服务提供者
            //var provider = services.BuildServiceProvider();
            //// 解析服务
            //var car = provider.GetService<IVehicle>();
            //var gun = provider.GetService<IWeapon>();
            //// 使用服务
            //car.Run();
            //gun.Fire();


            string json =  File.ReadAllText("A.txt").Replace("\r\n","\t");
            string[] jkl = json.Trim().Split('\t');
            while (true)
            {
                Random random = new Random();
                // 生成包含6个随机数字字符串的数组
                string[] randomStrings = new string[6];

                for (int i = 0; i < 6; i++)
                {
                    randomStrings[i] = jkl[random.Next(0, jkl.Length - 1)]; // 0-9的数字
                }
                File.AppendAllText("B.csv", string.Join(",", randomStrings) + "\r\n");
            }
        }
    }
}
