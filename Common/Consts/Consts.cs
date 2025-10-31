using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Consts
{
    public class Consts
    {

        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        public  const  string SQLServerConnectionString = @"Server=192.168.2.70,1433; Database=Lottery;User Id=sa;Password=Zhouenlai@305;
            Encrypt=false;TrustServerCertificate=true;Pooling=true; Connect Timeout=30;Max Pool Size=100;";

        public const string  SQLiteConnectionString = "Data Source=Lottery.db;Version=3;Pooling=True;Max Pool Size=100;";

        public const string  MysqlconnectionString = "Server=localhost;Port=3306;Database=lottery;User ID=root;Password=201015;";

        public const string  NpgsqlConnectionString = "Host=localhost;Port=5432;Database=lottery;Username=postgres;Password=201015";




    }
}
