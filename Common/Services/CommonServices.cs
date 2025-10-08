
using System.Collections.Concurrent;

namespace Common.Services
{
    public class CommonServices
    {

        ConcurrentDictionary<int, double> D17  = new ConcurrentDictionary<int, double>();

        ConcurrentDictionary<int,double> D33 = new ConcurrentDictionary<int,double>();

        public CommonServices() { 
        
        }
    }
}
