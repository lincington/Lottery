
namespace Lottery
{
    public class Program
    {     
        static void Main(string[] args)
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
            //string json =  File.ReadAllText("A.txt").Replace("\r\n","\t");
            //string[] jkl = json.Trim().Split('\t');
            //List<int>  jsondata  = new List<int>();
            //foreach (string jklItem in jkl) {
            //    jsondata.Add(int.Parse(jklItem));
            //}
            //while (true)
            //{
            //    Random random = new Random();
            //    // 生成包含6个随机数字字符串的数组
            //    int[] randomStrings = new int[6];
            //    string datajson = "";
            //    for (int i = 0; i < 6; i++)
            //    {
            //        randomStrings[i] = jsondata[random.Next(0, jkl.Length - 1)]; // 0-9的数字
            //    }
            //    IEnumerable<int> jkkd =  randomStrings.ToList().OrderBy(x => x);
            //    jkkd = jkkd.Distinct();
            //    if (jkkd.Count() ==6 &&  jkkd.Sum() > 90 && jkkd.Last()==33 && jkkd.ToArray()[4] == 29 && jkkd.Sum() < 130 && jkkd.First() <12)
            //    {
            //        foreach (var item in jkkd)
            //        {
            //            datajson += item.ToString("00");
            //        }
            //        Console.WriteLine(datajson);
            //    }
            //}
            //var DAD  = SQLServerHelper.GetAverage(3359, "FR1", "17.0");
            //     DAD = SQLServerHelper.GetAverage(3359, "FR2", "17.0");
            //     DAD = SQLServerHelper.GetAverage(3359, "FR3", "17.0");
            //     DAD = SQLServerHelper.GetAverage(3359, "FR4", "17.0");
            //     DAD = SQLServerHelper.GetAverage(3359, "FR5", "17.0");
            //     DAD = SQLServerHelper.GetAverage(3359, "FR6", "17.0");
           // NpgsqlHelper.GetCount();

        }
    }
}
