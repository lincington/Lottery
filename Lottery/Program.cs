using Common;
using System.Threading.Tasks;

namespace Lottery
{
    public class Program
    {     
        static async Task Main(string[] args)
        {
            string json =  File.ReadAllText("A.txt").Replace("\r\n","\t");
            string[] jkl = json.Trim().Split('\t');
            while (true)
            {
                Random random = new Random();
                // 生成包含6个随机数字字符串的数组
                string[] randomStrings = new string[6];

                for (int i = 0; i < 6; i++)
                {
                    randomStrings[i] = jkl[random.Next(0, jkl.Length - 1)]; // 0-9的数字
                }
                File.AppendAllText("B.csv", string.Join(",", randomStrings) + "\r\n");
            }
        }
    }
}
