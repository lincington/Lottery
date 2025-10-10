using Dapper;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Common.DBHelper
{
    public class SQLServerHelper
    {
        public SQLServerHelper() {
        }
        static string StrConnectionString  = "Server=192.168.1.70,1433; Database=Lottery;User Id=sa;Password=Zhouenlai@305;" +
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

        public static List<(int,double)> GetAverage(int NUM,string Numdata,string ThData)
        {  
            List<(int, double)>  values = new List<(int, double)>();
            try
            {
                using (var connection = new SqlConnection(StrConnectionString))
                {   
                    connection.Open();
                    for (int i = 1; i < NUM; i++)
                    {
                        string sql = $@"SELECT ROUND(AVG({Numdata} * 1.0), 6)   FROM  lotteryreal WHERE    ID > (3361-{NUM-i})";
                        double json = connection.Query<double>(sql).FirstOrDefault();

                        if (json.ToString("0.0")== ThData)
                        {
                            Console.WriteLine(Numdata+ "    " + (NUM -i).ToString()  );
                        }

                         values.Add((NUM-i, json));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return values;
        }

        public double  GetAverageData(int NUM, string Numdata)
        {
            double  values = 0;
            try
            {
                using (var connection = new SqlConnection(StrConnectionString))
                {
                    connection.Open();
                    string sql = $@"SELECT ROUND(AVG({Numdata} * 1.0), 6)   FROM  lotteryreal WHERE    ID > (3361-{NUM})";
                    values = connection.Query<double>(sql).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return values;
        }



        public List<double> GetAllLotteriesSum()
        {
            List<double> doubles = new List<double>();
            using (var connection = new SqlConnection(StrConnectionString))
            {
                connection.Open();

                for (int J = 1; J < 7; J++)
                {
                    string sql = $"SELECT   FR{J}  AS SUMDATA  FROM lotteryreal  ";
                    doubles.AddRange(connection.Query<double>(sql).ToList());
                }
            }
            return doubles;
        }

        public int Insert(LotteryMatrix matrix)
        {
            using (IDbConnection connection = new SqlConnection(StrConnectionString))
            {
                string sql = @"
                INSERT INTO LotteryMatrix (minm, maxm)
                OUTPUT INSERTED.id
                VALUES (@MinM, @MaxM)";

                return connection.ExecuteScalar<int>(sql, matrix);
            }
        }

        // 插入单条记录（异步版本）
        public async Task<int> InsertAsync(LotteryMatrix matrix)
        {
            using (IDbConnection connection = new SqlConnection(StrConnectionString))
            {
                string sql = @"
                INSERT INTO LotteryMatrix (minm, maxm)
                OUTPUT INSERTED.id
                VALUES (@MinM, @MaxM)";

                return await connection.ExecuteScalarAsync<int>(sql, matrix);
            }
        }

        // 批量插入
        public int InsertBatch(IEnumerable<LotteryMatrix> matrices)
        {
            using (IDbConnection connection = new SqlConnection(StrConnectionString))
            {
                string sql = @"
                INSERT INTO LotteryMatrix (minm, maxm)
                VALUES (@MinM, @MaxM)";

                return connection.Execute(sql, matrices);
            }
        }

        // 批量插入（异步版本）
        public async Task<int> InsertBatchAsync(IEnumerable<LotteryMatrix> matrices)
        {
            using (IDbConnection connection = new SqlConnection(StrConnectionString))
            {
                string sql = @"
                INSERT INTO LotteryMatrix (minm, maxm)
                VALUES (@MinM, @MaxM)";

                return await connection.ExecuteAsync(sql, matrices);
            }
        }


        public   List<LotteryMatrix>  GetAllAsync()
        {
            using var connection = new SqlConnection(StrConnectionString);
            var sql = "SELECT * FROM LotteryMatrix";
            connection.Open();
            return connection.Query<LotteryMatrix>(sql).ToList();
        }



        public  async Task MatrixRedBlue()
        {

            while (true)
            {
                Lottery ld = DoubleColorBallGenerator.GenerateSQLTicket();
                // 创建 1x6 行向量
                var rowVector = Matrix<double>.Build.Dense(1, 6,
                    new double[] { ld.FR1, ld.FR2, ld.FR3, ld.FR4, ld.FR5, ld.FR6 });
                // 创建 6x1 列向量
                var colVector = Matrix<double>.Build.Dense(6, 1,
                    new double[] { ld.R1, ld.R2, ld.R3, ld.R4, ld.R5, ld.R6 });
                // 矩阵乘法: 1x6 * 6x1 = 1x1 (标量)
                var result1 = rowVector.Multiply(colVector);
                int total1 = 0;
                for (int i = 0; i < result1.RowCount; i++)
                {
                    for (int j = 0; j < result1.ColumnCount; j++)
                    {
                        total1 += (int)result1[i, j];
                    }
                }
                // 矩阵乘法: 6x1 * 1x6 = 6x6
                var result2 = colVector.Multiply(rowVector);
                int tota2 = 0;
                for (int i = 0; i < result2.RowCount; i++)
                {
                    for (int j = 0; j < result2.ColumnCount; j++)
                    {
                        tota2 += (int)result2[i, j];
                    }
                }
                Console.WriteLine(" minnum "+ total1 +  "maxnum  " + tota2);

                await InsertAsync(new LotteryMatrix() { MinM = total1, MaxM = tota2 });
            }
        }

        public async Task<bool> GetAllLotteriesPrint()
        {

            List<double> doubles = new List<double>();
            using (var connection = new SqlConnection(StrConnectionString))
            {
                connection.Open();

                for (int i = 0; i < 3348; i++)
                {
                    for (int j = 1; j < 7; j++)
                    {
                        string sql = $"SELECT   FR{j}  AS SUMDATA  FROM lotteryreal  WHERE  ID ={i + 1}";
                        List<double> DD = connection.Query<double>(sql).ToList();
                        if (DD.Count > 0)
                        {
                            doubles.AddRange(DD);
                        }
                    }
                }
            }

            List<double> ASdoubles = new List<double>();
            using (var connection = new SqlConnection(StrConnectionString))
            {
                connection.Open();
                string sql = $"SELECT   B1  AS SUMDATA  FROM lotteryreal ";
                ASdoubles.AddRange(connection.Query<double>(sql).ToList());
            } 
            
            int doublesdata = doubles.Count-32+6;
            int ASdoublesdata = ASdoubles.Count-15+1;

            List<LotteryNow> lotteries = new List<LotteryNow>();
            while (true)
            {
               Random random = new Random();


               Lottery ld =  DoubleColorBallGenerator.GenerateSQLTicket();
              int red =  random.Next(doublesdata);
              int blue = random.Next(ASdoublesdata);
                int redsum=0, bluesum=0;
                for (int i=0;i<33-6;i++)
                {
                    redsum +=(int)doubles[red + i];
                }

                redsum = redsum +ld.R1 + ld.R2+ ld.R3+ld.R4+ld.R5+ ld.R6;

                for (int i = 0; i < 16-1; i++)
                {
                    bluesum += (int)ASdoubles[blue + i];
                }
                bluesum += ld.B1;
                
                   lotteries.Add(new LotteryNow() { Numdata = ld.R1.ToString("00") + "-" + ld.R2.ToString("00") + "-" + ld.R3.ToString("00") + "-" + ld.R4.ToString("00") + "-" + ld.R5.ToString("00") + "-" + ld.R6.ToString("00") + "-" + ld.B1.ToString("00"),
                    Blueavg = (double)bluesum / 16,
                    Redavg = (double)redsum / 33
                });
             
          
                if (lotteries.Count > 1000)
                {
                    Console.WriteLine(lotteries.Count);
                    BulkInsertLotteryNow(lotteries);
                    lotteries.Clear();
                }

                
            }
        }
        public int InsertAndReturnId(LotteryNow lottery)
        {
            using (var connection = new SqlConnection(StrConnectionString))
            {
                var sql = @"INSERT INTO LotteryNow (numdata, redavg, blueavg) 
                    OUTPUT INSERTED.id
                    VALUES (@Numdata, @Redavg, @Blueavg)";
                connection.Open();
                return connection.QuerySingle<int>(sql, lottery);
            }
        }

        public void BulkInsertLotteryNow(List<LotteryNow> lotteries)
        {
                using var connection = new SqlConnection(StrConnectionString);
                var sql = @"INSERT INTO LotteryNow (numdata, redavg, blueavg) 
                    VALUES (@Numdata, @Redavg, @Blueavg)";
                connection.Open();
                connection.Execute(sql, lotteries);
        }


        public async Task<List<double>> GetAllLotteriesDealWith()
        {

            List<double>  doubles = new List<double>();

            using var connection = new SqlConnection(StrConnectionString);
            
            connection.Open();

            for (int i = 0; i < 3348; i++)
            {
                for (int j = 1; j < 7; j++)
            {  
                string sql = $"SELECT   FR{j}  AS SUMDATA  FROM lotteryreal  WHERE  ID ={i+1}";
                List<double> DD = connection.Query<double>(sql).ToList();
                if (DD.Count > 0)
                {
                    doubles.AddRange(DD);
                }
            }
            }
            
            int K = 1 ,J = 1;
            while (true)
            {

                if (doubles.Count > 32)
                {
                    LotteryRedSum lotteryRedSum = new LotteryRedSum();
                    lotteryRedSum.Id = K++;
                    lotteryRedSum.Rn1 = (int)doubles[0];
                    lotteryRedSum.Rn2 = (int)doubles[1];
                    lotteryRedSum.Rn3 = (int)doubles[2];
                    lotteryRedSum.Rn4 = (int)doubles[3];
                    lotteryRedSum.Rn5 = (int)doubles[4];
                    lotteryRedSum.Rn6 = (int)doubles[5];
                    lotteryRedSum.Rn7 = (int)doubles[6];
                    lotteryRedSum.Rn8 = (int)doubles[7];
                    lotteryRedSum.Rn9 = (int)doubles[8];
                    lotteryRedSum.Rn10 = (int)doubles[9];
                    lotteryRedSum.Rn11 = (int)doubles[10];
                    lotteryRedSum.Rn12 = (int)doubles[11];
                    lotteryRedSum.Rn13 = (int)doubles[12];
                    lotteryRedSum.Rn14 = (int)doubles[13];
                    lotteryRedSum.Rn15 = (int)doubles[14];
                    lotteryRedSum.Rn16 = (int)doubles[15];
                    lotteryRedSum.Rn17 = (int)doubles[16];
                    lotteryRedSum.Rn18 = (int)doubles[17];
                    lotteryRedSum.Rn19 = (int)doubles[18];
                    lotteryRedSum.Rn20 = (int)doubles[19];
                    lotteryRedSum.Rn21 = (int)doubles[20];
                    lotteryRedSum.Rn22 = (int)doubles[21];
                    lotteryRedSum.Rn23 = (int)doubles[22];
                    lotteryRedSum.Rn24 = (int)doubles[23];
                    lotteryRedSum.Rn25 = (int)doubles[24];
                    lotteryRedSum.Rn26 = (int)doubles[25];
                    lotteryRedSum.Rn27 = (int)doubles[26];
                    lotteryRedSum.Rn28 = (int)doubles[27];
                    lotteryRedSum.Rn29 = (int)doubles[28];
                    lotteryRedSum.Rn30 = (int)doubles[29];
                    lotteryRedSum.Rn31 = (int)doubles[30];
                    lotteryRedSum.Rn32 = (int)doubles[31];
                    lotteryRedSum.Rn33 = (int)doubles[32];

                    lotteryRedSum.SumRn =
                       +lotteryRedSum.Rn1
                       + lotteryRedSum.Rn2
                       + lotteryRedSum.Rn3
                       + lotteryRedSum.Rn4
                       + lotteryRedSum.Rn5
                       + lotteryRedSum.Rn6
                       + lotteryRedSum.Rn7
                       + lotteryRedSum.Rn8
                       + lotteryRedSum.Rn9
                       + lotteryRedSum.Rn10
                       + lotteryRedSum.Rn11
                       + lotteryRedSum.Rn12
                       + lotteryRedSum.Rn13
                       + lotteryRedSum.Rn14
                       + lotteryRedSum.Rn15
                       + lotteryRedSum.Rn16
                       + lotteryRedSum.Rn17
                       + lotteryRedSum.Rn18
                       + lotteryRedSum.Rn19
                       + lotteryRedSum.Rn20
                       + lotteryRedSum.Rn21
                       + lotteryRedSum.Rn22
                       + lotteryRedSum.Rn23
                       + lotteryRedSum.Rn24
                       + lotteryRedSum.Rn25
                       + lotteryRedSum.Rn26
                       + lotteryRedSum.Rn27
                       + lotteryRedSum.Rn28
                       + lotteryRedSum.Rn29
                       + lotteryRedSum.Rn30
                       + lotteryRedSum.Rn31
                       + lotteryRedSum.Rn32
                       + lotteryRedSum.Rn33;

                    lotteryRedSum.AvgRn = lotteryRedSum.SumRn / 33;
                    await InsertAsync(lotteryRedSum);
                    doubles.RemoveRange(0, 32);
                }
                else
                {
                    break;
                }
            }
            Console.WriteLine(doubles.Count);
            Console.WriteLine(doubles.ToArray().ToString());

            List<double> ASdoubles  = new List<double>();
            using (var connectiond = new SqlConnection(StrConnectionString))
            {
               connectiond.Open();
               string sql = $"SELECT   B1  AS SUMDATA  FROM lotteryreal ";
               ASdoubles.AddRange(connection.Query<double>(sql).ToList());
             }

            while (true)
            {
                if (ASdoubles.Count > 15)
                {
                    LotteryBlueSum lotteryRedSum = new LotteryBlueSum();
                    lotteryRedSum.Id = J++;
                    lotteryRedSum.Bn1 = (int)ASdoubles[0];
                    lotteryRedSum.Bn2 = (int)ASdoubles[1];
                    lotteryRedSum.Bn3 = (int)ASdoubles[2];
                    lotteryRedSum.Bn4 = (int)ASdoubles[3];
                    lotteryRedSum.Bn5 = (int)ASdoubles[4];
                    lotteryRedSum.Bn6 = (int)ASdoubles[5];
                    lotteryRedSum.Bn7 = (int)ASdoubles[6];
                    lotteryRedSum.Bn8 = (int)ASdoubles[7];
                    lotteryRedSum.Bn9 = (int)ASdoubles[8];
                    lotteryRedSum.Bn10 = (int)ASdoubles[9];
                    lotteryRedSum.Bn11 = (int)ASdoubles[10];
                    lotteryRedSum.Bn12 = (int)ASdoubles[11];
                    lotteryRedSum.Bn13 = (int)ASdoubles[12];
                    lotteryRedSum.Bn14 = (int)ASdoubles[13];
                    lotteryRedSum.Bn15 = (int)ASdoubles[14];
                    lotteryRedSum.Bn16 = (int)ASdoubles[15];

                    lotteryRedSum.SumBn =
                      +lotteryRedSum.Bn1
                      + lotteryRedSum.Bn2
                      + lotteryRedSum.Bn3
                      + lotteryRedSum.Bn4
                      + lotteryRedSum.Bn5
                      + lotteryRedSum.Bn6
                      + lotteryRedSum.Bn7
                      + lotteryRedSum.Bn8
                      + lotteryRedSum.Bn9
                      + lotteryRedSum.Bn10
                      + lotteryRedSum.Bn11
                      + lotteryRedSum.Bn12
                      + lotteryRedSum.Bn13
                      + lotteryRedSum.Bn14
                      + lotteryRedSum.Bn15
                      + lotteryRedSum.Bn16;

                    lotteryRedSum.AvgBn = lotteryRedSum.SumBn / 16;
                    await InsertAsync(lotteryRedSum);
                    ASdoubles.RemoveRange(0, 15);
                }
                else
                {
                    break;
                }

            }

            Console.WriteLine(ASdoubles.Count);
            Console.WriteLine(ASdoubles.ToArray().ToString());
            return doubles;
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



        // Insert single record
        public async Task<int> InsertAsync(LotteryRedSum entity)
        {
            const string sql = @"
            INSERT INTO [dbo].[Lotteryredsum] 
            (id, rn1, rn2, rn3, rn4, rn5, rn6, rn7, rn8, rn9, rn10, 
             rn11, rn12, rn13, rn14, rn15, rn16, rn17, rn18, rn19, rn20, 
             rn21, rn22, rn23, rn24, rn25, rn26, rn27, rn28, rn29, rn30, 
             rn31, rn32, rn33, sumrn, avgrn)
            VALUES 
            (@Id, @Rn1, @Rn2, @Rn3, @Rn4, @Rn5, @Rn6, @Rn7, @Rn8, @Rn9, @Rn10, 
             @Rn11, @Rn12, @Rn13, @Rn14, @Rn15, @Rn16, @Rn17, @Rn18, @Rn19, @Rn20, 
             @Rn21, @Rn22, @Rn23, @Rn24, @Rn25, @Rn26, @Rn27, @Rn28, @Rn29, @Rn30, 
             @Rn31, @Rn32, @Rn33, @SumRn, @AvgRn)";

            using var connection = new SqlConnection(StrConnectionString);
            connection.Open();
            return await connection.ExecuteAsync(sql, entity);
        }

        // Bulk insert multiple records
        public async Task<int> BulkInsertAsync(IEnumerable<LotteryRedSum> entities)
        {
            const string sql = @"
            INSERT INTO [dbo].[Lotteryredsum] 
            (id, rn1, rn2, rn3, rn4, rn5, rn6, rn7, rn8, rn9, rn10, 
             rn11, rn12, rn13, rn14, rn15, rn16, rn17, rn18, rn19, rn20, 
             rn21, rn22, rn23, rn24, rn25, rn26, rn27, rn28, rn29, rn30, 
             rn31, rn32, rn33, sumrn, avgrn)
            VALUES 
            (@Id, @Rn1, @Rn2, @Rn3, @Rn4, @Rn5, @Rn6, @Rn7, @Rn8, @Rn9, @Rn10, 
             @Rn11, @Rn12, @Rn13, @Rn14, @Rn15, @Rn16, @Rn17, @Rn18, @Rn19, @Rn20, 
             @Rn21, @Rn22, @Rn23, @Rn24, @Rn25, @Rn26, @Rn27, @Rn28, @Rn29, @Rn30, 
             @Rn31, @Rn32, @Rn33, @SumRn, @AvgRn)";

            using var connection = new SqlConnection(StrConnectionString);
            return await connection.ExecuteAsync(sql, entities);
        }

        // Insert with transaction
        public async Task<int> InsertWithTransactionAsync(LotteryRedSum entity)
        {
            using var connection = new SqlConnection(StrConnectionString);
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                const string sql = @"
                INSERT INTO [dbo].[Lotteryredsum] 
                (id, rn1, rn2, rn3, rn4, rn5, rn6, rn7, rn8, rn9, rn10, 
                 rn11, rn12, rn13, rn14, rn15, rn16, rn17, rn18, rn19, rn20, 
                 rn21, rn22, rn23, rn24, rn25, rn26, rn27, rn28, rn29, rn30, 
                 rn31, rn32, rn33, sumrn, avgrn)
                VALUES 
                (@Id, @Rn1, @Rn2, @Rn3, @Rn4, @Rn5, @Rn6, @Rn7, @Rn8, @Rn9, @Rn10, 
                 @Rn11, @Rn12, @Rn13, @Rn14, @Rn15, @Rn16, @Rn17, @Rn18, @Rn19, @Rn20, 
                 @Rn21, @Rn22, @Rn23, @Rn24, @Rn25, @Rn26, @Rn27, @Rn28, @Rn29, @Rn30, 
                 @Rn31, @Rn32, @Rn33, @SumRn, @AvgRn)";

                var result = await connection.ExecuteAsync(sql, entity, transaction);
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<int> InsertAsync(LotteryBlueSum entity)
        {
            const string sql = @"
            INSERT INTO [dbo].[Lotterybluesum] 
            (bn1, bn2, bn3, bn4, bn5, bn6, bn7, bn8, bn9, bn10, 
             bn11, bn12, bn13, bn14, bn15, bn16, sumbn, avgbn)
            VALUES 
            (@Bn1, @Bn2, @Bn3, @Bn4, @Bn5, @Bn6, @Bn7, @Bn8, @Bn9, @Bn10, 
             @Bn11, @Bn12, @Bn13, @Bn14, @Bn15, @Bn16, @SumBn, @AvgBn)";

            using var connection = new SqlConnection(StrConnectionString);
            return await connection.ExecuteAsync(sql, entity);
        }

        // Bulk insert multiple records
        public async Task<int> BulkInsertAsync(IEnumerable<LotteryBlueSum> entities)
        {
            const string sql = @"
            INSERT INTO [dbo].[Lotterybluesum] 
            (bn1, bn2, bn3, bn4, bn5, bn6, bn7, bn8, bn9, bn10, 
             bn11, bn12, bn13, bn14, bn15, bn16, sumbn, avgbn)
            VALUES 
            (@Bn1, @Bn2, @Bn3, @Bn4, @Bn5, @Bn6, @Bn7, @Bn8, @Bn9, @Bn10, 
             @Bn11, @Bn12, @Bn13, @Bn14, @Bn15, @Bn16, @SumBn, @AvgBn)";

            using var connection = new SqlConnection(StrConnectionString);
            return await connection.ExecuteAsync(sql, entities);
        }

        // Insert with transaction
        public async Task<int> InsertWithTransactionAsync(LotteryBlueSum entity)
        {
            using var connection = new SqlConnection(StrConnectionString);
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                const string sql = @"
                INSERT INTO [dbo].[Lotterybluesum] 
                (bn1, bn2, bn3, bn4, bn5, bn6, bn7, bn8, bn9, bn10, 
                 bn11, bn12, bn13, bn14, bn15, bn16, sumbn, avgbn)
                VALUES 
                (@Bn1, @Bn2, @Bn3, @Bn4, @Bn5, @Bn6, @Bn7, @Bn8, @Bn9, @Bn10, 
                 @Bn11, @Bn12, @Bn13, @Bn14, @Bn15, @Bn16, @SumBn, @AvgBn)";

                var result = await connection.ExecuteAsync(sql, entity, transaction);
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }






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

        public static void InsertWithTransaction(Lottery lottery)
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
        public static void UpdateLottery( )
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
    public class LotteryD: LotteryAvg
    {
        public double SUM  { get; set; }
    }

    public class LotteryRedSum
    {
        public int Id { get; set; }
        public int? Rn1 { get; set; }
        public int? Rn2 { get; set; }
        public int? Rn3 { get; set; }
        public int? Rn4 { get; set; }
        public int? Rn5 { get; set; }
        public int? Rn6 { get; set; }
        public int? Rn7 { get; set; }
        public int? Rn8 { get; set; }
        public int? Rn9 { get; set; }
        public int? Rn10 { get; set; }
        public int? Rn11 { get; set; }
        public int? Rn12 { get; set; }
        public int? Rn13 { get; set; }
        public int? Rn14 { get; set; }
        public int? Rn15 { get; set; }
        public int? Rn16 { get; set; }
        public int? Rn17 { get; set; }
        public int? Rn18 { get; set; }
        public int? Rn19 { get; set; }
        public int? Rn20 { get; set; }
        public int? Rn21 { get; set; }
        public int? Rn22 { get; set; }
        public int? Rn23 { get; set; }
        public int? Rn24 { get; set; }
        public int? Rn25 { get; set; }
        public int? Rn26 { get; set; }
        public int? Rn27 { get; set; }
        public int? Rn28 { get; set; }
        public int? Rn29 { get; set; }
        public int? Rn30 { get; set; }
        public int? Rn31 { get; set; }
        public int? Rn32 { get; set; }
        public int? Rn33 { get; set; }
        public int? SumRn { get; set; }
        public double? AvgRn { get; set; }
    }


    public class LotteryBlueSum
    {
        public int? Id { get; set; }
        public int? Bn1 { get; set; }
        public int? Bn2 { get; set; }
        public int? Bn3 { get; set; }
        public int? Bn4 { get; set; }
        public int? Bn5 { get; set; }
        public int? Bn6 { get; set; }
        public int? Bn7 { get; set; }
        public int? Bn8 { get; set; }
        public int? Bn9 { get; set; }
        public int? Bn10 { get; set; }
        public int? Bn11 { get; set; }
        public int? Bn12 { get; set; }
        public int? Bn13 { get; set; }
        public int? Bn14 { get; set; }
        public int? Bn15 { get; set; }
        public int? Bn16 { get; set; }
        public int? SumBn { get; set; }
        public double? AvgBn { get; set; }
    }

    public class LotteryNow
    {
        public int Id { get; set; }
        public string Numdata { get; set; } = "";
        public double? Redavg { get; set; }
        public double? Blueavg { get; set; }
    }


    public class LotteryMatrix
    {
        public int Id { get; set; }
        public int? MinM { get; set; }
        public int? MaxM { get; set; }
    }

}
