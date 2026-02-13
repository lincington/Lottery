using Common.DBHelper;
using Common.Models;
using System.Collections.Concurrent;

namespace Common.Services
{
    public class CommonServices
    {
        private double[] E6 =Consts.Consts.E6;
        private double[] V5 = Consts.Consts.V5;
        private double[] D13 = Consts.Consts.D13;
        private int[] N13 =      Consts.Consts.N13;
        private int[] N33 =  Consts.Consts.N33;
        private int[] N16 = Consts.Consts.N16;

        ConcurrentDictionary<int, double> D16  = new ConcurrentDictionary<int, double>();
        ConcurrentDictionary<int,double> D33 = new ConcurrentDictionary<int,double>();

        public CommonServices() {
           // SQLServerHelper sQLServerHelper = new SQLServerHelper();
           //List<Lottery> SDD  = sQLServerHelper.GetAllLotteries() ;
            MysqlHelper mysqlHelper = new MysqlHelper();
            List<Lottery> SDD = mysqlHelper.GetAllLotteries();

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
