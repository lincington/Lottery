using Dapper;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Text.Json;


namespace Common
{
    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public string instruction { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string input { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string output { get; set; } = "";
    }

    public class MysqlHelper
    {
        List<int> RedCount = new List<int>(32);

       static  string connectionString = "Server=localhost;Port=3306;Database=lottery;User ID=root;Password=201015;";
        public MysqlHelper()
        {

        }
        public  bool GeTtest()
        {
            // Get();
            //   Avg();
            // EvUpdate(new int[] { 2,6,9,12,14,30,8},"25076");
            // StepOne();
            //Parallel.For(1, 50,(i) => { Test(); });
            Check();
            // 其他代码逻辑
            // 例如，调用其他方法或执行其他操作
            // Console.WriteLine("Hello, World!");
            return true;
        }
        public static void Cal()
        {
            string connectionString = "Server=localhost;Port=3306;Database=lottery;User ID=root;Password=201015;";
            string sql = @"INSERT INTO lotterydatanow 
                (R1, R2, R3, R4, R5, R6, B1, Date, CalTime, Ticks) 
                VALUES (@R1, @R2, @R3, @R4, @R5, @R6, @B1, @Date, @CalTime, @Ticks)";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    for (int i = 1; i < 33; i++)
                    {
                        string sqlsel = $"SELECT COUNT(*) as C from lotterydatanow WHERE R1={i}";
                        var results = connection.Query<Lotterydata>(sqlsel).First();
                        if (results != null)
                        {
                            AvgRData[0] = ((long)results.R1 + AvgRData[0]);
                            AvgRData[1] = ((long)results.R2 + AvgRData[1]);
                            AvgRData[2] = ((long)results.R3 + AvgRData[2]);
                            AvgRData[3] = ((long)results.R4 + AvgRData[3]);
                            AvgRData[4] = ((long)results.R5 + AvgRData[4]);
                            AvgRData[5] = ((long)results.R6 + AvgRData[5]);
                            AvgBData = ((long)results.B1 + AvgBData);

                            var param = new
                            {

                                R1 = (double)((double)AvgRData[0] / Idex),
                                R2 = (double)((double)AvgRData[1] / Idex),
                                R3 = (double)((double)AvgRData[2] / Idex),
                                R4 = (double)((double)AvgRData[3] / Idex),
                                R5 = (double)((double)AvgRData[4] / Idex),
                                R6 = (double)((double)AvgRData[5] / Idex),
                                B1 = (double)((double)AvgBData / Idex),
                                Date = DateTime.Now.ToString("yyyy-MM-dd"),
                                CalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                Ticks = DateTime.Now.Ticks
                            };
                            int rows = connection.Execute(sql, param);
                            Num = Num + 1;
                            Idex = Idex + 1;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public static void Check()
        {
            List<Lotterydata> list = new List<Lotterydata>();
            string connectionString = "Server=localhost;Port=3306;Database=lottery;User ID=root;Password=201015;";
            string sql = @"INSERT INTO lotterydatanow 
                (R1, R2, R3, R4, R5, R6, B1, Date, CalTime, Ticks) 
                VALUES (@R1, @R2, @R3, @R4, @R5, @R6, @B1, @Date, @CalTime, @Ticks)";
            string sqlreal = "SELECT R1, R2, R3, R4, R5, R6, B1, Date, CalTime, Ticks FROM `lotterydatareal`";
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    list = connection.Query<Lotterydata>(sqlreal).ToList();
                    int ALL = 0;
                    while (true)
                    {
                        if (ALL > 2000000) break;
                        ALL++;
                        var rand = new Random();
                        // 红球号码池 1-33
                        var redBalls = Enumerable.Range(1, 33).ToList();
                        // 随机选6个红球
                        var reds = redBalls.OrderBy(x => rand.Next()).Take(6).OrderBy(x => x).ToList();
                        // 蓝球 1-16
                        int blue = rand.Next(1, 17);
                        var param = new Lotterydata
                        {
                            ID = 10000,
                            R1 = reds[0],
                            R2 = reds[1],
                            R3 = reds[2],
                            R4 = reds[3],
                            R5 = reds[4],
                            R6 = reds[5],
                            B1 = blue,
                            Date = DateTime.Now.ToString("yyyy-MM-dd"),
                            CalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            Ticks = DateTime.Now.Ticks
                        };
                        list.Add(param);
                        var avgR1 = list.Where(x => x.R1.HasValue).Average(x => x.R1 ?? 0);
                        var avgR2 = list.Where(x => x.R2.HasValue).Average(x => x.R2 ?? 0);
                        var avgR3 = list.Where(x => x.R3.HasValue).Average(x => x.R3 ?? 0);
                        var avgR4 = list.Where(x => x.R4.HasValue).Average(x => x.R4 ?? 0);
                        var avgR5 = list.Where(x => x.R5.HasValue).Average(x => x.R5 ?? 0);
                        var avgR6 = list.Where(x => x.R6.HasValue).Average(x => x.R6 ?? 0);
                        var avgB1 = list.Where(x => x.B1.HasValue).Average(x => x.B1 ?? 0);
                        LotterydataAvg yyu = JsonSerializer.Deserialize<LotterydataAvg>(File.ReadAllText("avg.json"));

                        double davgR1 = (Math.Abs(avgR1 - yyu.R1));
                        double davgR2 = (Math.Abs(avgR2 - yyu.R2));
                        double davgR3 = (Math.Abs(avgR3 - yyu.R3));
                        double davgR4 = (Math.Abs(avgR4 - yyu.R4));
                        double davgR5 = (Math.Abs(avgR5 - yyu.R5));
                        double davgR6 = (Math.Abs(avgR6 - yyu.R6));
                        double davgB1 = (Math.Abs(yyu.B1 - avgB1));

                        for (int i = 1; i < 26; i++)
                        {
                            if (davgR1 < 0.0025 - (i * 0.0001) &&
                                                   davgR2 < 0.0025 - (i * 0.0001) &&
                                                   davgR3 < 0.0025 - (i * 0.0001) &&
                                                   davgR4 < 0.0025 - (i * 0.0001) &&
                                                   davgR5 < 0.0025 - (i * 0.0001) &&
                                                   davgR6 < 0.0025 - (i * 0.0001) &&
                                                   davgB1 < 0.0025 - (i * 0.0001))
                            {

                                string avgData = $@"SELECT AVG(R1) as R1 ,AVG(R2) as R2,AVG(R3) as R3,AVG(R4) as R4,AVG(R5) as R5,AVG(R6) as R6,AVG(B1) as B1     FROM 
                                                    ( SELECT   R1 , R2, R3, R4, R5, R6, B1 FROM  `lotterydatareal`
                                                    UNION ALL
                                                    (SELECT   {param.R1}  AS R1,  {param.R2} AS  R2,  {param.R3} AS R3, 
                                                    {param.R4} AS R4 ,  {param.R5} AS  R5 ,  {param.R6} AS  R6  ,  {param.B1} AS  B1  FROM   `lotterydatareal`  LIMIT 1 )  ) as  t ";
                                LotterydataAvg listD = connection.Query<LotterydataAvg>(avgData).ToList().FirstOrDefault();
                                if (listD.R1 + listD.R2 + listD.R3 + listD.R4 + listD.R5 + listD.R6 + listD.B1 > 109.5532)
                                {
                                    int rows = connection.Execute(sql, param);
                                }
                            }
                        }

                        list.Remove(param);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static void Get()
        {
            List<Root> ad = JsonSerializer.Deserialize<List<Root>>(File.ReadAllText("lottery.json"));
            // 替换为你的 MySQL 连接字符串
            string connectionString = "Server=localhost;Port=3306;Database=lottery;User ID=root;Password=201015;";
            string sql = @"INSERT INTO lotterydatareal (R1, R2, R3, R4, R5, R6, B1, Date, CalTime, Ticks) 
                VALUES (@R1, @R2, @R3, @R4, @R5, @R6, @B1, @Date, @CalTime, @Ticks)";
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    foreach (Root root in ad)
                    {
                        string[] reds = root.output.Split(',');
                        var param = new
                        {
                            R1 = reds[0],
                            R2 = reds[1],
                            R3 = reds[2],
                            R4 = reds[3],
                            R5 = reds[4],
                            R6 = reds[5],
                            B1 = reds[6],
                            Date = DateTime.Now.ToString("yyyy-MM-dd"),
                            CalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            Ticks = DateTime.Now.Ticks
                        };
                        int rows = connection.Execute(sql, param);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public static void Test()
        {
            // 替换为你的 MySQL 连接字符串
            string connectionString = "Server=localhost;Port=3306;Database=lottery;User ID=root;Password=201015;";
            string sql = @"INSERT INTO lotterydata (R1, R2, R3, R4, R5, R6, B1, Date, CalTime, Ticks) 
                VALUES (@R1, @R2, @R3, @R4, @R5, @R6, @B1, @Date, @CalTime, @Ticks)";
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    while (true)
                    {
                        var rand = new Random();
                        // 红球号码池 1-33
                        var redBalls = Enumerable.Range(1, 33).ToList();
                        // 随机选6个红球
                        var reds = redBalls.OrderBy(x => rand.Next()).Take(6).OrderBy(x => x).ToList();
                        // 蓝球 1-16
                        int blue = rand.Next(1, 17);
                        var param = new
                        {
                            R1 = reds[0],
                            R2 = reds[1],
                            R3 = reds[2],
                            R4 = reds[3],
                            R5 = reds[4],
                            R6 = reds[5],
                            B1 = blue,
                            Date = DateTime.Now.ToString("yyyy-MM-dd"),
                            CalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            Ticks = DateTime.Now.Ticks
                        };
                        int rows = connection.Execute(sql, param);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }

        static long Num = 1;
        static int Idex = 1;
        static long[] AvgRData = new long[6] { 3, 5, 15, 21, 25, 31 };
        static long AvgBData = 9;
        public static void Avg()
        {
            // 替换为你的 MySQL 连接字符串
     
            string sql = @"INSERT INTO lotterydatarealavg 
                (R1, R2, R3, R4, R5, R6, B1, Date, CalTime, Ticks) 
                VALUES (@R1, @R2, @R3, @R4, @R5, @R6, @B1, @Date, @CalTime, @Ticks)";

            string sqlall = "SELECT COUNT(*) as NUM  from lotterydatareal";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    int count = connection.QuerySingle<int>(sqlall);

                    Idex = count;

                    //  while (true)
                    {
                        string sqlsel = $@"
                            SELECT 
                            AVG(R1) AS R1,
                            AVG(R2) AS R2,
                            AVG(R3) AS R3,
                            AVG(R4) AS R4,
                            AVG(R5) AS R5,
                            AVG(R6) AS R6,
                            AVG(B1) AS B1
                            FROM (
                            SELECT R1, R2, R3, R4, R5, R6, B1
                            FROM lotterydatareal
                            ORDER BY ID
                            LIMIT {count}
                            ) AS t;";
                        var results = connection.Query<LotterydataAvg>(sqlsel).First();

                        if (results != null)
                        {
                            var param = new
                            {
                                R1 = results.R1,
                                R2 = results.R2,
                                R3 = results.R3,
                                R4 = results.R4,
                                R5 = results.R5,
                                R6 = results.R6,
                                B1 = results.B1,
                                Date = DateTime.Now.ToString("yyyy-MM-dd"),
                                CalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                Ticks = DateTime.Now.Ticks
                            };
                            int rows = connection.Execute(sql, param);
                            Idex = Idex + 1;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public static void EvUpdate(int[] ints, string No)
        {
            // 替换为你的 MySQL 连接字符串
            string connectionString = "Server=localhost;Port=3306;Database=lottery;User ID=root;Password=201015;";


            string sqlreal = @"INSERT INTO lotterydatareal
                (R1, R2, R3, R4, R5, R6, B1, Date, CalTime, Ticks,No) 
                VALUES (@R1, @R2, @R3, @R4, @R5, @R6, @B1, @Date, @CalTime, @Ticks,@No)";


            string sql = @"INSERT INTO lotterydatarealavg 
                (R1, R2, R3, R4, R5, R6, B1, Date, CalTime, Ticks) 
                VALUES (@R1, @R2, @R3, @R4, @R5, @R6, @B1, @Date, @CalTime, @Ticks)";

            string sqlall = "SELECT COUNT(*) as NUM  from lotterydatareal";



            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    var param = new
                    {
                        R1 = ints[0],
                        R2 = ints[1],
                        R3 = ints[2],
                        R4 = ints[3],
                        R5 = ints[4],
                        R6 = ints[5],
                        B1 = ints[6],
                        Date = DateTime.Now.ToString("yyyy-MM-dd"),
                        CalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        Ticks = DateTime.Now.Ticks,
                        No = No
                    };

                    int rows = connection.Execute(sqlreal, param);

                    Thread.Sleep(500);
                    int count = connection.QuerySingle<int>(sqlall);

                    Idex = count;

                    //  while (true)
                    {
                        string sqlsel = $@"
                            SELECT 
                            AVG(R1) AS R1,
                            AVG(R2) AS R2,
                            AVG(R3) AS R3,
                            AVG(R4) AS R4,
                            AVG(R5) AS R5,
                            AVG(R6) AS R6,
                            AVG(B1) AS B1
                            FROM (
                            SELECT R1, R2, R3, R4, R5, R6, B1
                            FROM lotterydatareal
                            ORDER BY ID
                            LIMIT {count}
                            ) AS t;";
                        var results = connection.Query<LotterydataAvg>(sqlsel).First();

                        if (results != null)
                        {
                            var param2 = new
                            {
                                R1 = results.R1,
                                R2 = results.R2,
                                R3 = results.R3,
                                R4 = results.R4,
                                R5 = results.R5,
                                R6 = results.R6,
                                B1 = results.B1,
                                Date = DateTime.Now.ToString("yyyy-MM-dd"),
                                CalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                Ticks = DateTime.Now.Ticks
                            };
                            int rows2 = connection.Execute(sql, param2);
                            Idex = Idex + 1;
                        }
                        StepOne(count - 2);
                        //if (Idex > count)
                        //{
                        //    break;
                        //}
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }



        }
        public static void StepOne()
        {

            string sqlall = "SELECT ID as NUM  from lotterydatareal";

            string sql = @"INSERT INTO lotterydatarealavgstep
                (R1, R2, R3, R4, R5, R6, B1, Date, CalTime, Ticks) 
                VALUES (@R1, @R2, @R3, @R4, @R5, @R6, @B1, @Date, @CalTime, @Ticks)";

            string connectionString = "Server=localhost;Port=3306;Database=lottery;User ID=root;Password=201015;";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    List<int> count = connection.Query<int>(sqlall).ToList();

                    foreach (var ID in count)
                    {
                        if (ID < 862)
                        {
                            continue;
                        }
                        string sqlreal = $"SELECT * FROM `lotterydatarealavg` WHERE ID>{ID - 1}  LIMIT 2";
                        List<LotterydataAvg> DataAvg = connection.Query<LotterydataAvg>(sqlreal).ToList();

                        if (DataAvg.Count == 2)
                        {
                            var param = new
                            {

                                R1 = (DataAvg[1].R1 - DataAvg[0].R1),
                                R2 = (DataAvg[1].R2 - DataAvg[0].R2),
                                R3 = (DataAvg[1].R3 - DataAvg[0].R3),
                                R4 = (DataAvg[1].R4 - DataAvg[0].R4),
                                R5 = (DataAvg[1].R5 - DataAvg[0].R5),
                                R6 = (DataAvg[1].R6 - DataAvg[0].R6),
                                B1 = (DataAvg[1].B1 - DataAvg[0].B1),
                                Date = DateTime.Now.ToString("yyyy-MM-dd"),
                                CalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                Ticks = ID,

                            };
                            int rows = connection.Execute(sql, param);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public static void StepOne(int ID)
        {

            string sqlall = "SELECT ID as NUM  from lotterydatareal";

            string sql = @"INSERT INTO lotterydatarealavgstepabs 
                (R1, R2, R3, R4, R5, R6, B1, Date, CalTime, Ticks) 
                VALUES (@R1, @R2, @R3, @R4, @R5, @R6, @B1, @Date, @CalTime, @Ticks)";

            string connectionString = "Server=localhost;Port=3306;Database=lottery;User ID=root;Password=201015;";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    List<int> count = connection.Query<int>(sqlall).ToList();

                    // foreach (var ID in count)
                    {
                        //if(ID< 862)
                        //{
                        //    continue;
                        //}
                        string sqlreal = $"SELECT * FROM `lotterydatarealavg` WHERE ID>{ID - 1}  LIMIT 2";
                        List<LotterydataAvg> DataAvg = connection.Query<LotterydataAvg>(sqlreal).ToList();

                        if (DataAvg.Count == 2)
                        {
                            var param = new
                            {
                                R1 = Math.Abs(DataAvg[1].R1 - DataAvg[0].R1),
                                R2 = Math.Abs(DataAvg[1].R2 - DataAvg[0].R2),
                                R3 = Math.Abs(DataAvg[1].R3 - DataAvg[0].R3),
                                R4 = Math.Abs(DataAvg[1].R4 - DataAvg[0].R4),
                                R5 = Math.Abs(DataAvg[1].R5 - DataAvg[0].R5),
                                R6 = Math.Abs(DataAvg[1].R6 - DataAvg[0].R6),
                                B1 = Math.Abs(DataAvg[1].B1 - DataAvg[0].B1),
                                Date = DateTime.Now.ToString("yyyy-MM-dd"),
                                CalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                Ticks = DateTime.Now.Ticks,

                            };
                            int rows = connection.Execute(sql, param);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        public List<Lottery> GetAllLotteries()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT * FROM lottery";
                connection.Open();
                return connection.Query<Lottery>(sql).ToList();
            }
        }

    }

    public class LotterydataAvg
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double R1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double R2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double R3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double R4 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double R5 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double R6 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double B1 { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string Date { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public string CalTime { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public long? Ticks { get; set; }
    }
    public class Lotterydata
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? R1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? R2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? R3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? R4 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? R5 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? R6 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? B1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CalTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? Ticks { get; set; }
    }


}
