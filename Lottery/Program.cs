using Common.Services;

namespace Lottery
{
    public class Program
    {     
        static void Main(string[]  args)
        {
            #region =======================================================================
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
            //SQLServerHelper sQLServerHelper = new SQLServerHelper();
            //sQLServerHelper.GetAllLotteries();
            //string path = AppDomain.CurrentDomain.BaseDirectory + "data.txt";
            //for (int i = 1; i <= 3361; i++) {
            //    double  R1= sQLServerHelper.GetAverageData(i,"R1");
            //        double R2= sQLServerHelper.GetAverageData(i, "R2");
            //        double R3= sQLServerHelper.GetAverageData(i, "R3");
            //        double R4= sQLServerHelper.GetAverageData(i, "R4");
            //        double R5= sQLServerHelper.GetAverageData(i, "R5");
            //    double R6= sQLServerHelper.GetAverageData(i, "R6");
            //    double FR1 = sQLServerHelper.GetAverageData(i, "FR1");
            //    double FR2 = sQLServerHelper.GetAverageData(i, "FR2");
            //    double FR3 = sQLServerHelper.GetAverageData(i, "FR3");
            //    double FR4 = sQLServerHelper.GetAverageData(i, "FR4");
            //    double FR5 = sQLServerHelper.GetAverageData(i, "FR5");
            //    double FR6 = sQLServerHelper.GetAverageData(i, "FR6");
            //    double B1 = sQLServerHelper.GetAverageData(i, "B1");
            //    double FR = FR1 + FR2 + FR3 + FR4 + FR5 + FR6;
            //    NpgsqlHelper.BulkInsertLotteryD(
            //        new LotteryD  {
            //                ID = i,
            //                SUM = FR,
            //                FR1 = FR1,
            //                FR2 = FR2,
            //                FR3 = FR3,
            //                FR4 = FR4,
            //                FR5 = FR5,
            //                FR6 = FR6,
            //                R1 = R1,
            //                R2 = R2,
            //                R3 = R3,
            //                R4 = R4,
            //                R5 = R5,
            //                R6 = R6,
            //                B1 = B1
            //        }   );    
            //}
            //int count = 0;
            //int Sum  = 0;
            //double dataList =0;
            //int N  = 0;
            //Dictionary<int,int>  ints = new Dictionary<int,int>();
            //while (true)
            //{
            //    int L = DoubleColorBallGenerator.GenerateBuleBalls();
            //    Sum += L; 
            //    count++;
            //    N++;
            //    dataList = (double)Sum / count;
            //    if(dataList.ToString("0.#") == "8.5")
            //    {
            //       // Console.WriteLine("Count:" + count + "  Sum:" + Sum + "  Avg:" + dataList.ToString("0.##"));
            //        if (ints.ContainsKey(count))
            //        {
            //            ints[count] = ints[count] + 1;
            //        }
            //        else
            //        {
            //            ints.TryAdd(count, 1);
            //        }
            //        count = 0;
            //        Sum = 0;
            //    }
            //    if (N == 17721088)
            //    {
            //        count = 0;
            //        Sum = 0;
            //        foreach (var item in ints.OrderBy(oaad=>oaad.Key))
            //        {
            //            File.AppendAllText("AAA.txt", item.Key + " : " + item.Value + Environment.NewLine);
            //            Console.WriteLine(item.Key + " * " + item.Value+ "="+ item.Key * item.Value );
            //        }
            //        N = 0;
            //        ints.Clear();
            //    }
            //}

            #endregion
       
            CommonServices commonServices = new CommonServices();
            foreach (var item in commonServices.GetD33().OrderByDescending(x => x.Value))
            {
                Console.WriteLine(item.Key.ToString("00") + "----" + item.Value.ToString("0.0000"));
            }
            Console.WriteLine("-------------------------------------------------------------------");
            foreach (var item in commonServices.GetD16().OrderByDescending(x => x.Value))
            {
                Console.WriteLine(item.Key.ToString("00") + "----" + item.Value.ToString("0.0000"));
            }
        }
    }
}
