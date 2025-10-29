
using Common.DBHelper;
using System.Collections.Concurrent;

namespace Common.Services
{
    public class CommonServices
    {

        ConcurrentDictionary<int, double> D16  = new ConcurrentDictionary<int, double>();
        ConcurrentDictionary<int,double> D33 = new ConcurrentDictionary<int,double>();

        double[] E6 = new double[6] {
        (34*1)/7,(34*2)/7,(34*3)/7,
        (34*4)/7,(34*5)/7,(34*6)/7
        };

        double[] V5  = new double[5] {       
            33/2,33/4,33/8,33/16,33/32
        };


        double[]  D13 = new double[13] {
                                16.5,8.25,5.5,4.125,3.3,2.75,2.0625
                                ,1.65,1.5,1.375,1.1,1.03125,0.825
        };

        int[]  N13 = new int[13] {
            2,4,6,8,10,12,16,
            20,22,24,30,32,40
        };

        int[] N33 = new int[33] {
            1,2,3,4,5,6,7,8,9,
            10,11,12,13,14,15,16,17,18,19
            ,20,21,22,23,24,25,26,27,28,29
            ,30,31,32,33};

        int[] N16 = new int[16] {
            1,2,3,4,5,6,7,8,9,
            10,11,12,13,14,15,16};

        public CommonServices() {
            SQLServerHelper sQLServerHelper = new SQLServerHelper();
           List<Lottery> SDD  = sQLServerHelper.GetAllLotteries() ;
            int totalCount = SDD.Count;
            SDD.ForEach(lotteries => {
               List<int> num33 = N33.ToList();
               num33.RemoveAll(x => lotteries.FR1 == x || lotteries.FR2 == x || lotteries.FR3 == x || lotteries.FR4 == x || lotteries.FR5 == x || lotteries.FR6 == x);
                foreach (var item in num33)
                {
                     if (D33.ContainsKey(item))
                     {
                          D33[item] = D33[item] + (double) lotteries.ID / totalCount;
                     }
                     else
                     {
                          D33.TryAdd(item, (double)lotteries.ID/ totalCount);
                     }
                }
                List<int> num16 = N16.ToList();
                num16.RemoveAll(x => lotteries.B1 == x );
                foreach (var item in num16)
                {
                    if (D16.ContainsKey(item))
                    {
                        D16[item] = D16[item] + (double)lotteries.ID / totalCount;
                    }
                    else
                    {
                        D16.TryAdd(item, (double)lotteries.ID / totalCount);
                    }
                }
            });
        }
    
        public double[] GetE6()
        {
            return E6;
        }

        public ConcurrentDictionary<int, double> GetD16()
        {
            return D16;
        }
        public ConcurrentDictionary<int, double> GetD33()
        {
            return D33;
        }
    }
}
