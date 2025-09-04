using Dapper;
using Microsoft.Data.SqlClient;
using MathNet.Numerics.Statistics;

namespace Common
{
    public class SQLServerHelper
    {
        public SQLServerHelper() {
        }
        static string StrConnectionString  = "Server=localhost;Database=Lottery;User Id=sa;Password=Zhouenlai@305;" +
            "TrustServerCertificate=true;Pooling=true;Max Pool Size=30000;Min Pool Size=300;Connection Lifetime=300;packet size=1000";
        public static void GetTest2()
        {
          Parallel.For
          (
              0, 1000,
              new ParallelOptions { MaxDegreeOfParallelism = 3 },
              i =>
              {
                  for (int k = 0; k < 100; k++)
                  {
                     var  datalist =   DoubleColorBallGenerator.GenerateSQLTickets(3347).ToList();
                      BulkInsertLotteries(datalist);
                  }
              });
        }

        public void InsertLottery(Lottery lottery)
        {
            using (var connection = new SqlConnection(StrConnectionString))
            {
                string sql = @"INSERT INTO lottery 
                      (ID, No, Date, FR1, FR2, FR3, FR4, FR5, FR6, R1, R2, R3, R4, R5, R6, B1) 
                      VALUES 
                      (@ID, @No, @Date, @FR1, @FR2, @FR3, @FR4, @FR5, @FR6, 
                       @R1, @R2, @R3, @R4, @R5, @R6, @B1)";
                connection.Open();
                connection.Execute(sql, lottery);
            }
        }

        public List<Lottery> GetAllLotteries()
        {
            using (var connection = new SqlConnection(StrConnectionString))
            {
                string sql = "SELECT * FROM lotteryreal";
                connection.Open();
                return connection.Query<Lottery>(sql).ToList();
            }
        }

        public List<double> GetAllLotteriesSum()
        {
            using (var connection = new SqlConnection(StrConnectionString))
            {
                string sql = "SELECT   FR1+FR2 +FR3+FR4+FR5+FR6  AS SUMDATA  FROM lotteryreal";
                connection.Open();
                return connection.Query<double>(sql).ToList();
            }
        }
     

        //public List<LotteryAvg> GetAllLotteries()
        //{
        //    using (var connection = new SqlConnection(StrConnectionString))
        //    {
        //        string sql = "SELECT FR1+FR2 +FR3+FR4+FR5+FR6  AS SUMDATA  FROM LotteryAvg";
        //        connection.Open();
        //        return connection.Query<LotteryAvg>(sql).ToList();
        //    }
        //}

        public static  bool GetAllLotteriesAvg (int i)
        {
            using (var connection = new SqlConnection(StrConnectionString))
            {
                string sql = $@"SELECT 
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
                from Lottery  WHERE id  < 3350*{i+1}  and  id > 3350*{i}";
                connection.Open();

                LotteryAvg lotteryAvg = connection.Query<LotteryAvg>(sql).FirstOrDefault();
                string sqlinsert= @"
            INSERT INTO LotteryAvg
            (FR1, FR2, FR3, FR4, FR5, FR6, R1, R2, R3, R4, R5, R6, B1)
            VALUES
            (@FR1, @FR2, @FR3, @FR4, @FR5, @FR6, @R1, @R2, @R3, @R4, @R5, @R6, @B1);";

                connection.Execute(sqlinsert, lotteryAvg);
                return true ;
            }
        }


        public  static void BulkInsertLotteries(IEnumerable<Lottery> lotteries)
        {
            try
            {
                using (var connection = new SqlConnection(StrConnectionString))
                {
                    string sql = @"INSERT INTO lottery 
                      (ID, No, Date, FR1, FR2, FR3, FR4, FR5, FR6, R1, R2, R3, R4, R5, R6, B1) 
                      VALUES 
                      (@ID, @No, @Date, @FR1, @FR2, @FR3, @FR4, @FR5, @FR6, 
                       @R1, @R2, @R3, @R4, @R5, @R6, @B1)";
                    connection.Open();
                    connection.Execute(sql, lotteries);
                }
            }
            catch (Exception )
            {
            }
        }

        public void InsertWithTransaction(Lottery lottery)
        {
            using (var connection = new SqlConnection(StrConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string sql = @"INSERT INTO lottery 
                             (ID, No, Date, FR1, FR2, FR3, FR4, FR5, FR6, R1, R2, R3, R4, R5, R6, B1) 
                             VALUES 
                             (@ID, @No, @Date, @FR1, @FR2, @FR3, @FR4, @FR5, @FR6, 
                              @R1, @R2, @R3, @R4, @R5, @R6, @B1)";

                        connection.Execute(sql, lottery, transaction: transaction);

                        // You can add more operations here

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void UpdateLottery( )
        {
         
                // 示例数据（替换为你的数据）
                double[] data = { 227 ,194 ,206, 201 ,205, 212 ,208 ,192, 210, 190 ,217, 217, 195, 212, 227, 225 };

                // 1. 计算均值和标准差
                double mean = data.Mean();
                double stdDev = data.StandardDeviation();

                Console.WriteLine($"均值 (μ): {mean:F4}");
                Console.WriteLine($"标准差 (σ): {stdDev:F4}");


                //// 2. 生成正态分布的理论曲线
                //var normalDist = new Normal(mean, stdDev);
                //double[] xValues = Generate.LinearRange(mean - 3 * stdDev, mean + 3 * stdDev, 100);
                //double[] yValues = xValues.Select(x => normalDist.Density(x)).ToArray();

                //// 3. 绘制直方图与正态曲线
                //var plt = new ScottPlot.Plot(800, 600);
                //plt.Title("数据分布 vs 正态分布");
                //plt.XLabel("值");
                //plt.YLabel("频率/密度");

                //// 绘制直方图（数据分布）
                //var histogram = new ScottPlot.Statistics.Histogram(data, min: mean - 3 * stdDev, max: mean + 3 * stdDev);
                //plt.AddBar(histogram.Bins, histogram.Counts, fillColor: System.Drawing.Color.Blue, label: "数据直方图");

                //// 绘制正态曲线（理论分布）
                //plt.AddScatter(xValues, yValues, color: System.Drawing.Color.Red, label: "正态分布");

                //plt.Legend();
                //plt.SaveFig("normal_distribution.png");
                //Console.WriteLine("图表已保存为 normal_distribution.png");
            
        }

    }
    public class Lottery
    {
        public int ID { get; set; }
        public int No { get; set; }
        public string Date { get; set; } = "";
        public int FR1 { get; set; }
        public int FR2 { get; set; }
        public int FR3 { get; set; }
        public int FR4 { get; set; }
        public int FR5 { get; set; }
        public int FR6 { get; set; }
        public int R1 { get; set; }
        public int R2 { get; set; }
        public int R3 { get; set; }
        public int R4 { get; set; }
        public int R5 { get; set; }
        public int R6 { get; set; }
        public int B1 { get; set; }
    }

    public class LotteryAvg
    {
        public int ID { get; set; }
    
        public double  FR1 { get; set; }
        public double  FR2 { get; set; }
        public double  FR3 { get; set; }
        public double  FR4 { get; set; }
        public double  FR5 { get; set; }
        public double  FR6 { get; set; }
        public double  R1 { get; set; }
        public double  R2 { get; set; }
        public double  R3 { get; set; }
        public double  R4 { get; set; }
        public double  R5 { get; set; }
        public double  R6 { get; set; }
        public double  B1 { get; set; }
    }
}
