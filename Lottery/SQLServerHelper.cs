using Dapper;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery
{
    public class SQLServerHelper
    {
        public SQLServerHelper() { }
        static string StrConnectionString  = "Server=192.168.198.129;Database=Lottery;User Id=sa;Password=Zhouenlai@305;TrustServerCertificate=true;";
        public static void GetTest2()
        {
            using (IDbConnection db = new SqlConnection(StrConnectionString))
            {
                try
                {
                    db.Open();
                }
                catch (Exception  ex)
                {
                    throw new Exception("连接数据库失败，请检查连接字符串", ex);
                }
            }
        }
    }
}
