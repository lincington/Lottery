using Common.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DBHelper
{
    public class SqliteHelper
    {

      
        string connectionString = $"Data Source={AppDomain.CurrentDomain.BaseDirectory}/Data/sqlite.db";

        public SqliteHelper()
        {
        }

        public List<Lottery> GetAllLotteries()
        {
            string databaseFilePath = connectionString; 

            using (var connection = new SqliteConnection(connectionString))
            {
                string sql = "SELECT * FROM lottery";
                connection.Open();
                return connection.Query<Lottery>(sql).ToList();
            }
        }
    }
}
