using Common.Models;
using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.Json;

namespace Common.DBHelper
{
    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public string instruction  { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string input { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string output  { get; set; } = "";
    }

    public class MysqlHelper
    {
        List<int> RedCount = new List<int>(32);
        private  static  string connectionString = "Server=localhost;Port=3306;Database=lottery;User ID=root;Password=201015;";
        public MysqlHelper()
        {
        }
        /// <summary>
        /// 解析文本文件，返回 SuperLotto 对象列表
        /// </summary>
        public static void LotteryParseFile()
        {
            try
            {
                var records = new List<Lottery>();
                string[] lines = File.ReadAllLines("lottery.txt");
                foreach (var line in lines)
                {
                    string[] parts = line.Split('\t');
                    if (parts.Length != 15)
                    {
                        Console.WriteLine($"字段数不正确，实际 {parts.Length}，期望 15");
                        return;
                    }
                    records.Add(new  Lottery
                    {
                        No = int.Parse(parts[0]),Date = parts[1],
                        FR1 = int.Parse(parts[2]),FR2 = int.Parse(parts[3]),FR3 = int.Parse(parts[4]),FR4 = int.Parse(parts[5]),
                        FR5 = int.Parse(parts[6]),FR6 = int.Parse(parts[7]),
                        R1 = int.Parse(parts[8]),R2 = int.Parse(parts[9]),R3 = int.Parse(parts[10]),R4 = int.Parse(parts[11]),
                        R5 = int.Parse(parts[12]),R6 = int.Parse(parts[13]),B1 = int.Parse(parts[14])
                    });
                }
                records.Reverse();
                // 批量插入
                string insertSql = @"
                     INSERT INTO lottery 
                                (No, Date, FR1, FR2, FR3, FR4, FR5, FR6,R1, R2, R3, R4, R5, R6, B1)
                                VALUES (@No, @Date, @FR1, @FR2, @FR3, @FR4, @FR5, @FR6,@R1, @R2, @R3, @R4, @R5, @R6,  @B1)";
                using (var conn = new MySqlConnection(connectionString))
                {
                     conn.Open();
                     foreach (var record in records.ToList())
                    {
                         int id = record.No; // 假设 No 是唯一标识
                        string checkSql = "SELECT COUNT(1) FROM lottery WHERE No = @Id";      // 1. 检查记录是否存在
                        int count = conn.ExecuteScalar<int>(checkSql, new { Id = id });
                        if (count != 0)
                        {
                            records.Remove(record);
                        }
                    }
                      int affectedRows = conn.Execute(insertSql, records);
                          Console.WriteLine($"成功插入 {affectedRows} 条记录。");
                   }
            }
            catch (Exception ex)
            {
                Console.WriteLine("错误: " + ex.Message);
            }
            Console.WriteLine("按任意键退出...");
        }

        /// <summary>
        /// 解析文本文件，返回 SuperLotto 对象列表
        /// </summary>
        public static void SevenLotteryParseFile()
        {
            try
            {
                var records = new List<SevenLottery>();
                string[] lines = File.ReadAllLines("7D.txt");
                foreach (var line in lines)
                {
                    string[] parts = line.Split('\t');
                    if (parts.Length != 17) continue; // 确保字段数正确
                    records.Add(new SevenLottery
                    {
                        No = int.Parse(parts[0]),Date = parts[1],FR1 = int.Parse(parts[2]),FR2 = int.Parse(parts[3]),FR3 = int.Parse(parts[4]),
                        FR4 = int.Parse(parts[5]),FR5 = int.Parse(parts[6]),FR6 = int.Parse(parts[7]),FR7 = int.Parse(parts[8]),
                        R1 = int.Parse(parts[9]),R2 = int.Parse(parts[10]),R3 = int.Parse(parts[11]),R4 = int.Parse(parts[12]),R5 = int.Parse(parts[13]),
                        R6 = int.Parse(parts[14]),R7 = int.Parse(parts[15]),
                        B1 = int.Parse(parts[16])
                    });
                }
               records.Reverse(); 

                // 批量插入
                string insertSql = @"
            INSERT INTO sevenlottery 
            (No, Date, FR1, FR2, FR3, FR4, FR5, FR6, FR7, R1, R2, R3, R4, R5, R6, R7, B1)
            VALUES  (@No, @Date, @FR1, @FR2, @FR3, @FR4, @FR5, @FR6, @FR7,@R1, @R2, @R3, @R4, @R5, @R6, @R7, @B1)";

                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    int affectedRows = conn.Execute(insertSql, records);
                    Console.WriteLine($"成功插入 {affectedRows} 条记录。");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("错误: " + ex.Message);
            }
            Console.WriteLine("按任意键退出...");
        }

        /// <summary>
        /// 解析文本文件，返回 SuperLotto 对象列表
        /// </summary>
        public static void Welfare3dParseFile()
        {    
            try
            {
                using (IDbConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    Console.WriteLine("数据库连接成功。");
                    var records = new List<Welfare3d>();
                    string[] lines = File.ReadAllLines("3D.txt");
                    int successCount = 0;

                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        string[] parts = line.Split('\t');
                        if (parts.Length < 3)
                        {
                            Console.WriteLine($"格式错误，跳过: {line}");
                            continue;
                        }
                        // 期号
                        if (!int.TryParse(parts[0], out int no))
                        {
                            Console.WriteLine($"期号解析失败: {parts[0]}");
                            continue;
                        }
                        // 开奖号码（例如 "1 4 4"）
                        string[] digits = parts[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (digits.Length != 3)
                        {
                            Console.WriteLine($"开奖号码格式错误: {parts[1]}");
                            continue;
                        }
                        if (!int.TryParse(digits[0], out int d1) ||
                            !int.TryParse(digits[1], out int d2) ||
                            !int.TryParse(digits[2], out int d3))
                        {
                            Console.WriteLine($"开奖号码数字解析失败: {parts[1]}");
                            continue;
                        }

                        // 和值
                        if (!int.TryParse(parts[2], out int sum))
                        {
                            Console.WriteLine($"和值解析失败: {parts[2]}");
                            continue;
                        }

                        records.Add(new Welfare3d
                        {
                            NO = no,
                            D1 = d1,
                            D2 = d2,
                            D3 = d3,
                            S = sum
                        });
                        successCount++;
                    }

                    records.Reverse();

                    // 插入数据（假设 ID 为自增列，因此不插入 ID）
                    string sql = @"
                            INSERT INTO welfare3d (NO, D1, D2, D3, S)
                            VALUES (@No, @D1, @D2, @D3, @S)";

                    conn.Execute(sql, records);
                    Console.WriteLine($"导入完成，成功插入 {successCount} 条记录。");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("错误: " + ex.Message);
            }
            Console.WriteLine("按任意键退出...");         
        }
        
        /// <summary>
        /// 解析文本文件，返回 SuperLotto 对象列表
        /// </summary>
        public static List<SuperLotto> ParseFile(string filePath)
        {
            var list = new List<SuperLotto>();
            using (var reader = new StreamReader(filePath, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                        string[] lines = File.ReadAllLines(filePath);
                       if (string.IsNullOrWhiteSpace(line)) continue;
                        string[] parts = line.Split('\t');
                        if (parts.Length < 14) // 共14列（含期号和日期）
                        {
                            Console.WriteLine($"格式错误，跳过: {line}");
                            continue;
                        }

                        // 解析各字段（注意：文件中的列顺序）
                        if (!int.TryParse(parts[0], out int no)) continue;
                        if (!int.TryParse(parts[2], out int fb1)) continue;
                        if (!int.TryParse(parts[3], out int fb2)) continue;
                        if (!int.TryParse(parts[4], out int fb3)) continue;
                        if (!int.TryParse(parts[5], out int fb4)) continue;
                        if (!int.TryParse(parts[6], out int fb5)) continue;
                        if (!int.TryParse(parts[7], out int b1)) continue;
                        if (!int.TryParse(parts[8], out int b2)) continue;
                        if (!int.TryParse(parts[9], out int b3)) continue;
                        if (!int.TryParse(parts[10], out int b4)) continue;
                        if (!int.TryParse(parts[11], out int b5)) continue;
                        if (!int.TryParse(parts[12], out int a1)) continue;
                        if (!int.TryParse(parts[13], out int a2)) continue;
                       
                    list.Add(new SuperLotto
                    {
                        NO = no,
                        FB1 = fb1,
                        FB2 = fb2,
                        FB3 = fb3,
                        FB4 = fb4,
                        FB5 = fb5,
                        B1 = b1,
                        B2 = b2,
                        B3 = b3,
                        B4 = b4,
                        B5 = b5,
                        A1 = a1,
                        A2 = a2
                    });
                }
            }

            Console.WriteLine($"共解析 {list.Count} 条有效数据。");
            return list;
        }

        /// <summary>
        /// 使用 Dapper 将数据插入数据库（带事务）
        /// </summary>
        public static void InsertRecords( List<SuperLotto> records)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                records.Reverse();
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string sql = @"INSERT INTO superlotto (NO, FB1, FB2, FB3, FB4, FB5,B1, B2, B3, B4, B5, A1, A2) 
                                   VALUES (@NO, @FB1, @FB2, @FB3, @FB4, @FB5, @B1, @B2, @B3, @B4, @B5, @A1, @A2)";

                        int affected = connection.Execute(sql, records, transaction);
                        transaction.Commit();

                        Console.WriteLine($"成功插入 {affected} 条记录。");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"插入失败: {ex.Message}");
                    }
                }
            }
        }

        public bool GeTtest()
        {
            Check();
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
                            AvgRData[0] = results.R1 + AvgRData[0];
                            AvgRData[1] = results.R2 + AvgRData[1];
                            AvgRData[2] = results.R3 + AvgRData[2];
                            AvgRData[3] = results.R4 + AvgRData[3];
                            AvgRData[4] = results.R5 + AvgRData[4];
                            AvgRData[5] = results.R6 + AvgRData[5];
                            AvgBData = results.B1 + AvgBData;

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
                                DateTime.Now.Ticks
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
                        var avgR1 = list .Average(x => x.R1);
                        var avgR2 = list .Average(x => x.R2);
                        var avgR3 = list .Average(x => x.R3);
                        var avgR4 = list .Average(x => x.R4);
                        var avgR5 = list .Average(x => x.R5);
                        var avgR6 = list .Average(x => x.R6);
                        var avgB1 = list .Average(x => x.B1);
                        LotterydataAvg? yyu = JsonSerializer.Deserialize<LotterydataAvg>(File.ReadAllText("avg.json"));

                        double davgR1 = Math.Abs(avgR1 - yyu.R1);
                        double davgR2 = Math.Abs(avgR2 - yyu.R2);
                        double davgR3 = Math.Abs(avgR3 - yyu.R3);
                        double davgR4 = Math.Abs(avgR4 - yyu.R4);
                        double davgR5 = Math.Abs(avgR5 - yyu.R5);
                        double davgR6 = Math.Abs(avgR6 - yyu.R6);
                        double davgB1 = Math.Abs(yyu.B1 - avgB1);

                        for (int i = 1; i < 26; i++)
                        {
                            if (davgR1 < 0.0025 - i * 0.0001 &&
                                                   davgR2 < 0.0025 - i * 0.0001 &&
                                                   davgR3 < 0.0025 - i * 0.0001 &&
                                                   davgR4 < 0.0025 - i * 0.0001 &&
                                                   davgR5 < 0.0025 - i * 0.0001 &&
                                                   davgR6 < 0.0025 - i * 0.0001 &&
                                                   davgB1 < 0.0025 - i * 0.0001)
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
                            DateTime.Now.Ticks
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
                            R1 = reds[0],R2 = reds[1],R3 = reds[2],
                            R4 = reds[3],R5 = reds[4],R6 = reds[5],B1 = blue,
                            Date = DateTime.Now.ToString("yyyy-MM-dd"),
                            CalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            DateTime.Now.Ticks
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
                    {
                        string sqlsel = $@"
                            SELECT 
                            AVG(R1) AS R1,AVG(R2) AS R2,AVG(R3) AS R3,
                            AVG(R4) AS R4,AVG(R5) AS R5,AVG(R6) AS R6,AVG(B1) AS B1
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
                                results.R1,results.R2,results.R3,
                                results.R4,results.R5,results.R6,results.B1,
                                Date = DateTime.Now.ToString("yyyy-MM-dd"),
                                CalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                DateTime.Now.Ticks
                            };
                            int rows = connection.Execute(sql, param);
                            Idex = Idex + 1;
                        }
                    }
                }
                catch (Exception  )
                {
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
                        R1 = ints[0],R2 = ints[1],R3 = ints[2],
                        R4 = ints[3],R5 = ints[4],R6 = ints[5],
                        B1 = ints[6],
                        Date = DateTime.Now.ToString("yyyy-MM-dd"),
                        CalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        DateTime.Now.Ticks,
                        No
                    };
                    int rows = connection.Execute(sqlreal, param);
                    Thread.Sleep(500);
                    int count = connection.QuerySingle<int>(sqlall);
                    Idex = count;
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
                                results.R1,
                                results.R2,
                                results.R3,
                                results.R4,
                                results.R5,
                                results.R6,
                                results.B1,
                                Date = DateTime.Now.ToString("yyyy-MM-dd"),
                                CalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                DateTime.Now.Ticks
                            };
                            int rows2 = connection.Execute(sql, param2);
                            Idex = Idex + 1;
                        }
                        StepOne(count - 2);
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
                                R1 = DataAvg[1].R1 - DataAvg[0].R1,
                                R2 = DataAvg[1].R2 - DataAvg[0].R2,
                                R3 = DataAvg[1].R3 - DataAvg[0].R3,
                                R4 = DataAvg[1].R4 - DataAvg[0].R4,
                                R5 = DataAvg[1].R5 - DataAvg[0].R5,
                                R6 = DataAvg[1].R6 - DataAvg[0].R6,
                                B1 = DataAvg[1].B1 - DataAvg[0].B1,
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
                                DateTime.Now.Ticks,
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

        public List<Lottery> GetAllLotteries()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT * FROM lottery";
                connection.Open();
                return connection.Query<Lottery>(sql).ToList();
            }
        }

        public   LotteryAvg GetAllLotteriesAvg()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT  
ROUND(AVG(FR1 * 1.0), 6)  as FR1 ,
ROUND(AVG(FR2 * 1.0), 6)  as FR2,
ROUND(AVG(FR3 * 1.0), 6)  as FR3,
ROUND(AVG(FR4 * 1.0), 6)  as FR4,
ROUND(AVG(FR5 * 1.0), 6)  as FR5,
ROUND(AVG(FR6 * 1.0), 6)  as FR6,  
ROUND(AVG(R1 * 1.0), 6)  as R1,
ROUND(AVG(R2 * 1.0), 6)  as R2,
ROUND(AVG(R3 * 1.0), 6)  as R3,
ROUND(AVG(R4 * 1.0), 6)  as R4,
ROUND(AVG(R5 * 1.0), 6)  as R5,
ROUND(AVG(R6 * 1.0), 6)  as R6,
ROUND(AVG(B1 * 1.0), 6)  as B1   
FROM lottery";
                connection.Open();

              LotteryAvg? lotteryAvg = connection.Query<LotteryAvg>(sql).FirstOrDefault();
            //    string sqlinsert = @"
            //INSERT INTO LotteryAvg
            //(FR1, FR2, FR3, FR4, FR5, FR6, R1, R2, R3, R4, R5, R6, B1)
            //VALUES
            //(@FR1, @FR2, @FR3, @FR4, @FR5, @FR6, @R1, @R2, @R3, @R4, @R5, @R6, @B1);";

            //    connection.Execute(sqlinsert, lotteryAvg);
           return lotteryAvg;
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
        public int R1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int R2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int R3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int R4 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int R5 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int R6 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int B1 { get; set; }

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
        public long Ticks { get; set; }
    }
}
