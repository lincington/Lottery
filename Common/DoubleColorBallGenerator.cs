
using Common.DBHelper;
using Common.Models;

namespace Common 
{
    public class DoubleColorBallGenerator
    {
        private static readonly ThreadLocal<Random> randomdd = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
        // 生成一注双色球号码
        public static Lottery  GenerateTicket()
        {
            var random = randomdd.Value!;
            HashSet<int> redBalls = new HashSet<int>();
            while (redBalls.Count < 6)
            {
                int  num = random.Next(1, 34);
                redBalls.Add(num);
            }
            var IredBalls = redBalls.OrderBy(x => x);  //红球排序
            var blueBall = random.Next(1, 17);       // 蓝球是1-16选1个数字
            return new Lottery()
                {
                    B1 = blueBall,
                    R1 = IredBalls.ElementAt(0),
                    R2 = IredBalls.ElementAt(1),
                    R3 = IredBalls.ElementAt(2),
                    R4 = IredBalls.ElementAt(3),
                    R5 = IredBalls.ElementAt(4),
                    R6 = IredBalls.ElementAt(5),
                    FR1 = redBalls.ElementAt(0),
                    FR2 = redBalls.ElementAt(1),
                    FR3 = redBalls.ElementAt(2),
                    FR4 = redBalls.ElementAt(3),
                    FR5 = redBalls.ElementAt(4),
                    FR6 = redBalls.ElementAt(5),
                    ID = ID++,
                    No = NO++,
                    Date = DateTime.Now.AddMicroseconds(ID*2).ToString()
                };
        }
        public  static  List<int> GenerateRedBalls()
        {
            var random = randomdd.Value!;
            HashSet<int> redBalls = new HashSet<int>();
            while (redBalls.Count < 6)
            {
                int number = random.Next(1, 34); // 生成1到33之间的随机数
                redBalls.Add(number); // HashSet自动去重
            }
            return new List<int>(redBalls);
        }


        public static int GenerateBuleBalls()
        {
            var random = randomdd.Value!;
            return random.Next(1, 17) ;
        }
        // 生成多注双色球号码
        public static IEnumerable<Lottery> GenerateTickets(int count)
        {
            return Enumerable.Range(0, count)
                .Select(_ => GenerateTicket())
                .ToArray();
        }
        // 生成一注双色球号码
        public  static int ID = 0;
        public static int NO  = ID + 3001;
        public static Lottery GenerateSQLTicket()
        {
            var random = randomdd.Value!;
            HashSet<int> redBalls = new HashSet<int>();
            while (redBalls.Count < 6)
            {
                int number = random.Next(1, 34); // 生成1到33之间的随机数
                redBalls.Add(number); // HashSet自动去重
            }

            var IredBalls = redBalls.OrderBy(x => x);
                // 蓝球是1-16选1个数字
                var blueBall = random.Next(1, 17);
                NO++;
                if (NO > 3370)
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

        // 生成多注双色球号码
        public static IEnumerable<Lottery> GenerateSQLTickets(int count)
        {
            return Enumerable.Range(0, count)
                .Select(_ => GenerateSQLTicket())
                .ToArray();
        }

    }
}
