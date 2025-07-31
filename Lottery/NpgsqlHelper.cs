using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery
{
    internal class NpgsqlHelper
    {

        public static string ConnectionString { get; set; } = "Host=localhost;Port=5432;Database=lottery;Username=postgres;Password=201015";

        // PostgreSQL 连接字符串
  
        public static void GetTest ()
        {
           var repository = new DCBRepository(ConnectionString);
            Parallel.For
            (
                0, 1000,
                new ParallelOptions { MaxDegreeOfParallelism = 10 },
                i => {                      
                    DoubleColorBallGenerator.GenerateTickets(3300*5).ToList().ForEach(ticket =>
                    {
                        repository.Add(ticket);
                    });
                }
            );
        }   
    }
}
