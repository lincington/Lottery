using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace Lottery
{
    public class DCBRepository
    {
        private readonly string _connectionString;

        public DCBRepository(string connectionString)
        {
            _connectionString = connectionString;
          //  GetConnection().Open();
        }

        private IDbConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        // 添加一条双色球记录
        public int Add(DCB ball)
        {
            const string sql = @"
        INSERT INTO ODCB 
            (R1, R2, R3, R4, R5, R6, B1, DAvg, sumdata )
        VALUES 
            (@R1, @R2, @R3, @R4, @R5, @R6, @B1, @DAvg, @sumdata )
        RETURNING id";

            using var conn = GetConnection();
            conn.Open();
            return conn.ExecuteScalar<int>(sql, ball);
        }


        public int Add(Dictionary<int,short> Red , Dictionary<int, short> Bule)
        {
            DcbSum dcbSum = new DcbSum();

            dcbSum.RN1 = Red[1];
            dcbSum.RN2 = Red[2];
            dcbSum.RN3 = Red[3];
            dcbSum.RN4 = Red[4]; 
            dcbSum.RN5 = Red[5];
            dcbSum.RN6 = Red[6];  
            dcbSum.RN7 = Red[7];
            dcbSum.RN8 = Red[8];
            dcbSum.RN9 = Red[9];
            dcbSum.RN10 = Red[10];
            dcbSum.RN11 = Red[11];
            dcbSum.RN12 = Red[12];
            dcbSum.RN13 = Red[13];
            dcbSum.RN14 = Red[14];
            dcbSum.RN15 = Red[15];  
            dcbSum.RN16 = Red[16];
            dcbSum.RN17 = Red[17];
            dcbSum.RN18 = Red[18];
            dcbSum.RN19 = Red[19];
            dcbSum.RN20 = Red[20];
            dcbSum.RN21 = Red[21];
            dcbSum.RN22 = Red[22];
            dcbSum.RN23 = Red[23];
            dcbSum.RN24 = Red[24];
            dcbSum.RN25 = Red[25];
            dcbSum.RN26 = Red[26];
            dcbSum.RN27 = Red[27];
            dcbSum.RN28 = Red[28];  
            dcbSum.RN29 = Red[29];
            dcbSum.RN30 = Red[30];
            dcbSum.RN31 = Red[31];
            dcbSum.RN32 = Red[32];
            dcbSum.RN33 = Red[33];



            dcbSum.BN1 = Bule[1];
            dcbSum.BN2 = Bule[2];


            dcbSum.BN3 = Bule[3];
            dcbSum.BN4 = Bule[4];
            dcbSum.BN5 = Bule[5];
            dcbSum.BN6 = Bule[6];
            dcbSum.BN7 = Bule[7];
            dcbSum.BN8 = Bule[8];
            dcbSum.BN9 = Bule[9];
            dcbSum.BN10 = Bule[10];
            dcbSum.BN11 = Bule[11];
            dcbSum.BN12 = Bule[12];
            dcbSum.BN13 = Bule[13];
            dcbSum.BN14 = Bule[14];
            dcbSum.BN15 = Bule[15];
            dcbSum.BN16 = Bule[16];

            InsertDcbSum(dcbSum);

            return 0; // 这里需要实现具体的插入逻辑，可能需要修改表结构以包含 OpenDate 和 IssueNumber 字段

        }
        public int InsertDcbSum(DcbSum record)
        {
           
                string sql = @"INSERT INTO dcbsum (
                        RN1, RN2, RN3, RN4, RN5, RN6, RN7, RN8, RN9, RN10,
                        RN11, RN12, RN13, RN14, RN15, RN16, RN17, RN18, RN19, RN20,
                        RN21, RN22, RN23, RN24, RN25, RN26, RN27, RN28, RN29, RN30,
                        RN31, RN32, RN33,
                        BN1, BN2, BN3, BN4, BN5, BN6, BN7, BN8, BN9, BN10,
                        BN11, BN12, BN13, BN14, BN15, BN16)
                      VALUES (
                        @RN1, @RN2, @RN3, @RN4, @RN5, @RN6, @RN7, @RN8, @RN9, @RN10,
                        @RN11, @RN12, @RN13, @RN14, @RN15, @RN16, @RN17, @RN18, @RN19, @RN20,
                        @RN21, @RN22, @RN23, @RN24, @RN25, @RN26, @RN27, @RN28, @RN29, @RN30,
                        @RN31, @RN32, @RN33,
                        @BN1, @BN2, @BN3, @BN4, @BN5, @BN6, @BN7, @BN8, @BN9, @BN10,
                        @BN11, @BN12, @BN13, @BN14, @BN15, @BN16)";

                using var conn = GetConnection();
                conn.Open();
                return conn.ExecuteScalar<int>(sql, record);
        }




  


        // 批量添加双色球记录
        public int AddBatch(IEnumerable<DCB> balls)
        {
            const string sql = @"
        INSERT INTO DCB 
            (R1, R2, R3, R4, R5, R6, Bl, Avg, Sum, IssueNumber, OpenDate)
        VALUES 
            (@R1, @R2, @R3, @R4, @R5, @R6, @Bl, @Avg, @Sum, @IssueNumber, @OpenDate)";

            using var conn = GetConnection();
            return conn.Execute(sql, balls);
        }

        // 根据ID获取双色球记录
        public DCB GetById(int id)
        {
            const string sql = "SELECT * FROM DCB WHERE id = @Id";

            using var conn = GetConnection();
            return conn.QueryFirstOrDefault<DCB>(sql, new { Id = id });
        }

        // 获取最新N期双色球记录
        public IEnumerable<DCB> GetLatest(int count)
        {
            const string sql = "SELECT * FROM DCB ORDER BY OpenDate DESC LIMIT @Count";

            using var conn = GetConnection();
            return conn.Query<DCB>(sql, new { Count = count });
        }

        // 获取指定红球出现的记录
        public IEnumerable<DCB> GetByRedBall(int ballNumber)
        {
            const string sql = @"
        SELECT * FROM DCB 
        WHERE R1 = @BallNumber OR R2 = @BallNumber OR R3 = @BallNumber OR
              R4 = @BallNumber OR R5 = @BallNumber OR R6 = @BallNumber
        ORDER BY OpenDate DESC";

            using var conn = GetConnection();
            return conn.Query<DCB>(sql, new { BallNumber = ballNumber });
        }

        // 获取指定蓝球出现的记录
        public IEnumerable<DCB> GetByBlueBall(int ballNumber)
        {
            const string sql = "SELECT * FROM DCB WHERE Bl = @BallNumber ORDER BY OpenDate DESC";

            using var conn = GetConnection();
            return conn.Query<DCB>(sql, new { BallNumber = ballNumber });
        }

        // 计算红球出现频率
        public Dictionary<int, int> GetRedBallFrequency()
        {
            var frequency = new Dictionary<int, int>();

            // 初始化1-33的红球
            for (int i = 1; i <= 33; i++)
            {
                frequency[i] = 0;
            }

            // 查询每个红球列的出现次数并累加
            for (int i = 1; i <= 6; i++)
            {
                string columnName = $"R{i}";
                string sql = $"SELECT {columnName} as ball, COUNT(*) as count FROM DCB GROUP BY {columnName}";

                using var conn = GetConnection();
                var results = conn.Query<(int ball, int count)>(sql);

                foreach (var result in results)
                {
                    frequency[result.ball] += result.count;
                }
            }

            return frequency;
        }

        // 计算统计信息
        public (decimal avgAvg, decimal avgSum, int totalRecords) GetStatistics()
        {
            const string sql = "SELECT AVG(Avg) as avgAvg, AVG(Sum) as avgSum, COUNT(*) as total FROM DCB";

            using var conn = GetConnection();
            return conn.QueryFirst<(decimal, decimal, int)>(sql);
        }
    }


    public class DCB
    {


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
        public double Davg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Sumdata { get; set; }
    }
 
    public class DoubleColorBallGenerator
    {
        private static readonly ThreadLocal<Random>  randomdd =  new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));

        // 生成一注双色球号码
        public static DCB GenerateTicket()
        {

           var  random = randomdd.Value!;
            // 红球是1-33选6个不重复的数字
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                HashSet<int> redBalls = new HashSet<int>();
                byte[] randomBytes = new byte[4];

                while (redBalls.Count < 6)
                {
                    rng.GetBytes(randomBytes);
                    int num = BitConverter.ToInt32(randomBytes, 0) & int.MaxValue;
                    num = 1 + (num % 33); // 1-33
                    redBalls.Add(num);
                }

                var IredBalls = redBalls.OrderBy(x => x);
                // 蓝球是1-16选1个数字
                var blueBall = random.Next(1, 17);

                return new DCB()
                {
                    B1 = blueBall,
                    R1 = IredBalls.ElementAt(0),
                    R2 = IredBalls.ElementAt(1),
                    R3 = IredBalls.ElementAt(2),
                    R4 = IredBalls.ElementAt(3),
                    R5 = IredBalls.ElementAt(4),
                    R6 = IredBalls.ElementAt(5),

                    Davg = redBalls.Average(),
                    Sumdata = redBalls.Sum(),
                };
            }
        }

        // 生成多注双色球号码
        public  static IEnumerable<DCB>  GenerateTickets(int count)
        {
            return Enumerable.Range(0, count)
                .Select(_ => GenerateTicket())
                .ToArray();
        }
    }
}