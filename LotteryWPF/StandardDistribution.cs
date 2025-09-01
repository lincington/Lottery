using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryWPF
{
    /// <summary>
    /// 提供正态分布，以及直方图
    /// </summary>
    public class StandardDistribution
        {
            #region 属性
            /// <summary>
            /// 最大值
            /// </summary>
            public double XsMax { get; set; }
            /// <summary>
            /// 最小值
            /// </summary>
            public double XsMin { get; set; }

            /// <summary>
            /// 组数量
            /// </summary>
            public int GroupCount { get; set; }

            /// <summary>
            /// 组距
            /// </summary>
            public double GroupLenth { get; set; }

            /// <summary>
            /// 样本数据
            /// </summary>
            public List<double> XDatas { get; set; }

            /// <summary>
            /// 方差
            /// </summary>
            public double Variance { get; private set; }

            /// <summary>
            /// 标准方差
            /// </summary>
            public double StandardVariance { get; private set; }

            /// <summary>
            /// 数学期望
            /// </summary>
            public double Average { get; private set; }

            /// <summary>
            ///  1/2π的平方根的值
            /// </summary>
            public static double InverseSqrt2PI = 1 / Math.Sqrt(2 * Math.PI);
            #endregion

            #region 构造方法
            public StandardDistribution(List<double> XDatas, int GroupCount)
            {
                this.XDatas = XDatas;
                XsMax = XDatas.Max();
                XsMin = XDatas.Min();
                this.GroupCount = GroupCount;
                GroupLenth = (XsMax - XsMin) / (GroupCount - 1);
                Average = XDatas.Average();
                Variance = GetVariance(XDatas);
                if (Variance == 0) throw new Exception("方差为0");//此时不需要统计 因为每个样本数据都相同，可以在界面做相应提示
                StandardVariance = Math.Sqrt(Variance);
            }
            #endregion

            /// <summary>
            /// 获取指定X值的Y值
            /// </summary>
            /// <param name="x"></param>
            /// <returns></returns>
            public double GetGaussianDistributionY(double x)
            {
                double Pow = -(Math.Pow(Math.Abs(x - Average), 2) / (2 * Variance));

                double result = (InverseSqrt2PI / StandardVariance) * Math.Pow(Math.E, Pow);

                return result;
            }

            /// <summary>
            /// 获取坐标
            /// </summary>
            /// <returns></returns>
            public List<Tuple<double, double>> GetGaussianDistributionYs()
            {
                List<Tuple<double, double>> XYs = new List<Tuple<double, double>>();

                Tuple<double, double> xy = null;

                foreach (double x in XDatas)
                {
                    xy = new Tuple<double, double>(x, GetGaussianDistributionY(x));
                    XYs.Add(xy);
                }
                return XYs;
            }
            /// <summary>
            /// 获取方差
            /// </summary>
            /// <param name="src"></param>
            /// <returns></returns>
            public static double GetVariance(List<double> src)
            {
                double average = src.Average();
                double SumOfSquares = 0;
                src.ForEach(x => { SumOfSquares += Math.Pow(x - average, 2); });
                return SumOfSquares / src.Count;//方差
            }

        }
    
}
