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

            Console.ReadLine();
        }
    }
}
