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