
using Common.DBHelper;
using System.Collections.Concurrent;

namespace Common.Services
{
    public class CommonServices
    {

        ConcurrentDictionary<int, double> D17  = new ConcurrentDictionary<int, double>();
        ConcurrentDictionary<int,double> D33 = new ConcurrentDictionary<int,double>();

        double[] E6 = new double[6] {
        (34*1)/7,(34*2)/7,(34*3)/7,
        (34*4)/7,(34*5)/7,(34*6)/7
        };

        double[] V5  = new double[5] {       
            33/2,33/4,33/8,33/16,33/32
        }; 

        int[] N33 = new int[33] {
            1,2,3,4,5,6,7,8,9,
            10,11,12,13,14,15,16,17,18,19
            ,20,21,22,23,24,25,26,27,28,29
            ,30,31,32,33};

        int[] N17 = new int[16] {
            1,2,3,4,5,6,7,8,9,
            10,11,12,13,14,15,16};

        public CommonServices() {

            SQLServerHelper sQLServerHelper = new SQLServerHelper();
            sQLServerHelper.GetAllLotteries().ForEach(lotteries => {
               List<int> num33 = N33.ToList();
               num33.RemoveAll(x => lotteries.FR1 == x || lotteries.FR2 == x || lotteries.FR3 == x || lotteries.FR4 == x || lotteries.FR5 == x || lotteries.FR6 == x);

                foreach (var item in num33)
                {
                     if (D33.ContainsKey(item))
                     {
                          D33[item] = D33[item] + (double) lotteries.ID / 3361;
                     }
                     else
                     {
                          D33.TryAdd(item, (double)lotteries.ID/3361);
                     }
                }
                List<int> num17 = N17.ToList();
                num17.RemoveAll(x => lotteries.B1 == x );
                foreach (var item in num17)
                {
                    if (D17.ContainsKey(item))
                    {
                        D17[item] = D17[item] + (double)lotteries.ID / 3361;
                    }
                    else
                    {
                        D17.TryAdd(item, (double)lotteries.ID / 3361);
                    }
                }
            });
        }
    
    
      public ConcurrentDictionary<int, double> GetD17()
        {
            return D17;
        }
        public ConcurrentDictionary<int, double> GetD33()
        {
            return D33;
        }
    }
}
