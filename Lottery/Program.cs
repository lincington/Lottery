namespace Lottery
{
    public class Program
    {     
        static async Task Main(string[] args)
        {
            string json =  File.ReadAllText("A.txt").Replace("\r\n","\t");
            string[] jkl = json.Trim().Split('\t');

            List<int>  jsondata  = new List<int>();
            foreach (string jklItem in jkl) {
                jsondata.Add(int.Parse(jklItem));
            }

            while (true)
            {
                Random random = new Random();
                // 生成包含6个随机数字字符串的数组
                int[] randomStrings = new int[6];
                string datajson = "";
                for (int i = 0; i < 6; i++)
                {
                    randomStrings[i] = jsondata[random.Next(0, jkl.Length - 1)]; // 0-9的数字
                }
                IEnumerable<int> jkkd =  randomStrings.ToList().OrderBy(x => x);
                jkkd = jkkd.Distinct();
                if (jkkd.Count() ==6 &&  jkkd.Sum() > 90 && jkkd.Last()==33 && jkkd.ToArray()[4] == 29 && jkkd.Sum() < 130 && jkkd.First() <12)
                {
                    foreach (var item in jkkd)
                    {
                        datajson += item.ToString("00");
                    }
                    Console.WriteLine(datajson);
                }
            }
        }
    }
}
