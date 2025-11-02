using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Consts
{
    public class Consts
    {
        public   static readonly string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public static readonly string SQLServerConnectionString = @"Server=192.168.2.70,1433; Database=Lottery;User Id=sa;Password=Zhouenlai@305;Encrypt=false;TrustServerCertificate=true;Pooling=true; Connect Timeout=30;Max Pool Size=100;";
        public static readonly string  SQLiteConnectionString = "Data Source=Lottery.db;Version=3;Pooling=True;Max Pool Size=100;";
        public static readonly string  MysqlconnectionString = "Server=localhost;Port=3306;Database=lottery;User ID=root;Password=201015;";
        public static readonly string  NpgsqlConnectionString = "Host=localhost;Port=5432;Database=lottery;Username=postgres;Password=201015";

        public static readonly  double[] E6 = new double[6] {(34*1)/7,(34*2)/7,(34*3)/7,(34*4)/7,(34*5)/7,(34*6)/7};
        public static readonly  double[] V5 = new double[5] {33/2,33/4,33/8,33/16,33/32};
        public static readonly double[] D13 = new double[13] {16.5,8.25,5.5,4.125,3.3,2.75,2.0625,1.65,1.5,1.375,1.1,1.03125,0.825};
        public static readonly int[] N13 = new int[13] {2,4,6,8,10,12,16,20,22,24,30,32,40};
        public static readonly int[] N33 = new int[33] {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33};
        public static readonly int[] N16 = new int[16] {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16};

    }
}
