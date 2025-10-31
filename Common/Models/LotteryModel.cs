namespace Common.Models
{
    public class LotteryModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string No { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Date { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public int FR1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int FR2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int FR3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int FR4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int FR5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int FR6 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int R1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int R2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int R3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int R4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int R5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int R6 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int B1 { get; set; }
    }
    public class Lottery
    {
        public int ID { get; set; }
        public int No { get; set; }
        public string Date { get; set; } = "";
        public int FR1 { get; set; }
        public int FR2 { get; set; }
        public int FR3 { get; set; }
        public int FR4 { get; set; }
        public int FR5 { get; set; }
        public int FR6 { get; set; }
        public int R1 { get; set; }
        public int R2 { get; set; }
        public int R3 { get; set; }
        public int R4 { get; set; }
        public int R5 { get; set; }
        public int R6 { get; set; }
        public int B1 { get; set; }
    }
    public class LotteryAvg
    {
        public int ID { get; set; }
        public double FR1 { get; set; }
        public double FR2 { get; set; }
        public double FR3 { get; set; }
        public double FR4 { get; set; }
        public double FR5 { get; set; }
        public double FR6 { get; set; }
        public double R1 { get; set; }
        public double R2 { get; set; }
        public double R3 { get; set; }
        public double R4 { get; set; }
        public double R5 { get; set; }
        public double R6 { get; set; }
        public double B1 { get; set; }
    }
    public class LotteryD : LotteryAvg
    {
        public double SUM { get; set; }
    }
    public class LotteryRedSum
    {
        public int Id { get; set; }
        public int? Rn1 { get; set; }
        public int? Rn2 { get; set; }
        public int? Rn3 { get; set; }
        public int? Rn4 { get; set; }
        public int? Rn5 { get; set; }
        public int? Rn6 { get; set; }
        public int? Rn7 { get; set; }
        public int? Rn8 { get; set; }
        public int? Rn9 { get; set; }
        public int? Rn10 { get; set; }
        public int? Rn11 { get; set; }
        public int? Rn12 { get; set; }
        public int? Rn13 { get; set; }
        public int? Rn14 { get; set; }
        public int? Rn15 { get; set; }
        public int? Rn16 { get; set; }
        public int? Rn17 { get; set; }
        public int? Rn18 { get; set; }
        public int? Rn19 { get; set; }
        public int? Rn20 { get; set; }
        public int? Rn21 { get; set; }
        public int? Rn22 { get; set; }
        public int? Rn23 { get; set; }
        public int? Rn24 { get; set; }
        public int? Rn25 { get; set; }
        public int? Rn26 { get; set; }
        public int? Rn27 { get; set; }
        public int? Rn28 { get; set; }
        public int? Rn29 { get; set; }
        public int? Rn30 { get; set; }
        public int? Rn31 { get; set; }
        public int? Rn32 { get; set; }
        public int? Rn33 { get; set; }
        public int? SumRn { get; set; }
        public double? AvgRn { get; set; }
    }
    public class LotteryBlueSum
    {
        public int? Id { get; set; }
        public int? Bn1 { get; set; }
        public int? Bn2 { get; set; }
        public int? Bn3 { get; set; }
        public int? Bn4 { get; set; }
        public int? Bn5 { get; set; }
        public int? Bn6 { get; set; }
        public int? Bn7 { get; set; }
        public int? Bn8 { get; set; }
        public int? Bn9 { get; set; }
        public int? Bn10 { get; set; }
        public int? Bn11 { get; set; }
        public int? Bn12 { get; set; }
        public int? Bn13 { get; set; }
        public int? Bn14 { get; set; }
        public int? Bn15 { get; set; }
        public int? Bn16 { get; set; }
        public int? SumBn { get; set; }
        public double? AvgBn { get; set; }
    }
    public class LotteryNow
    {
        public int Id { get; set; }
        public string Numdata { get; set; } = "";
        public double? Redavg { get; set; }
        public double? Blueavg { get; set; }
    }
    public class LotteryMatrix
    {
        public int Id { get; set; }
        public int? MinM { get; set; }
        public int? MaxM { get; set; }
    }

}
