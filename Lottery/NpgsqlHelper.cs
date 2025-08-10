using CsvHelper;
using CsvHelper.Configuration;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery
{
    internal class NpgsqlHelper
    {
        public static string ConnectionString { get; set; } 
            = "Host=localhost;Port=5432;Database=lottery;Username=postgres;Password=201015";

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
        
        static object sd = new object();
        public static void GetTest2()
        {
            var repository = new DCBRepository(ConnectionString);
            Parallel.For
            (
                0, 1000,
                new ParallelOptions { MaxDegreeOfParallelism = 10 },
                i =>
                {
                    for (int k = 0; k < 100; k++)
                    {
                        List<int> Red = new();
                        List<int> Blue = new();
                        Dictionary<int, short> RedData = new();
                        Dictionary<int, short> BlueData = new();

                        DoubleColorBallGenerator.GenerateTickets(3334).ToList().ForEach(ticket =>
                        {
                            Red.Add(ticket.R1);
                            Red.Add(ticket.R2);
                            Red.Add(ticket.R3);
                            Red.Add(ticket.R4);
                            Red.Add(ticket.R5);
                            Red.Add(ticket.R6);
                            Blue.Add(ticket.B1);
                        });
                        for (int j = 1; j < 34; j++)
                        {
                            short jk = (short)Red.Where(x => x == j).Count();
                            RedData.Add(j, jk);
                            if (j < 17)
                            {
                                short lk = (short)Blue.Where(x => x == j).Count();
                                BlueData.Add(j, lk);
                            }
                        }

                        lock (sd)
                        {
                            repository.Add(RedData, BlueData);
                        }
                    }
                });
        }
    }
}
