
namespace Common 
{
    public class DoubleColorBallGenerator
    {
        private static readonly ThreadLocal<Random> randomdd = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
        // 生成一注双色球号码
        public static DCB GenerateTicket()
        {

            var random = randomdd.Value!;
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
        public static IEnumerable<DCB> GenerateTickets(int count)
        {
            return Enumerable.Range(0, count)
                .Select(_ => GenerateTicket())
                .ToArray();
        }
        // 生成一注双色球号码
        static int ID = 136680094;
        static int NO  = 0;
        public static Lottery GenerateSQLTicket()
        {
            var random = randomdd.Value!;
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
                NO++;
                if (NO > 3344)
                {
                    NO = 0;
                }
                return new Lottery()
                {
                    ID = ID++,
                    FR1 = redBalls.ElementAt(0),
                    FR2 = redBalls.ElementAt(1),
                    FR3 = redBalls.ElementAt(2),
                    FR4 = redBalls.ElementAt(3),
                    FR5 = redBalls.ElementAt(4),
                    FR6 = redBalls.ElementAt(5),
                    Date = DateTime.Now.ToString("HH:mm:ss:ffff"),
                    No = NO,
                    B1 = blueBall,
                    R1 = IredBalls.ElementAt(0),
                    R2 = IredBalls.ElementAt(1),
                    R3 = IredBalls.ElementAt(2),
                    R4 = IredBalls.ElementAt(3),
                    R5 = IredBalls.ElementAt(4),
                    R6 = IredBalls.ElementAt(5),
                };
            }
        }

        // 生成多注双色球号码
        public static IEnumerable<Lottery> GenerateSQLTickets(int count)
        {
            return Enumerable.Range(0, count)
                .Select(_ => GenerateSQLTicket())
                .ToArray();
        }

    }
}
