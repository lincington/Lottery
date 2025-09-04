using Common;

namespace Lottery
{
    public class Program
    {     
        static void Main(string[] args)
        {
            int a = 4160;
            while (true)
            {
                a++;
                try
                {
                     SQLServerHelper.GetAllLotteriesAvg(a);
                    Console.WriteLine(a);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }          
            }
        } 
    }
}
