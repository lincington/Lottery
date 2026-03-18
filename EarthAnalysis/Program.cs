using System.Diagnostics;
using System.Runtime.InteropServices;

namespace EarthAnalysis
{
    /// <summary>
    /// 必须与Rust端的结构体完全匹配 [citation:2][citation:9]
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LotteryResult
    {
        public byte Red1;
        public byte Red2;
        public byte Red3;
        public byte Red4;
        public byte Red5;
        public byte Red6;
        public byte Blue;

        public override string ToString()
        {
            return $"红球: {Red1:D2} {Red2:D2} {Red3:D2} {Red4:D2} {Red5:D2} {Red6:D2}  蓝球: {Blue:D2}";
        }
    }

    /// <summary>
    /// 调用Rust动态库的P/Invoke方法 [citation:3][citation:7]
    /// </summary>
    public static class LotteryImageslibNative 
    {
        #if WINDOWS
                private const string DllName = "imageslib.dll";
        #elif LINUX
                private const string DllName = "imageslib.so";
        #elif MACOS
                private const string DllName = "imageslib.dylib";
        #else
                private const string DllName = "imageslib.dll";
        #endif
        
        [DllImport(DllName, EntryPoint = "csharp_process", CallingConvention = CallingConvention.Cdecl)]
        public static extern void csharp_process();

        [DllImport(DllName, EntryPoint = "hello_rust", CallingConvention = CallingConvention.Cdecl)]
        public static extern void HelloRust();

        [DllImport(DllName, EntryPoint = "return_value", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReturnValue();

    }

    /// <summary>
    /// 调用Rust动态库的P/Invoke方法 [citation:3][citation:7]
    /// </summary>
    public static class LotteryNative 
    {
#if WINDOWS
                private const string DllName = "Rust.dll";
#elif LINUX
                private const string DllName = "Rust.so";
#elif MACOS
                private const string DllName = "Rust.dylib";
#else
        private const string DllName = "Rust.dll";
#endif
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern LotteryResult generate_lottery();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr generate_multiple(uint count);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void free_lottery_results(IntPtr ptr, uint count);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr generate_formatted_string();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void free_string(IntPtr ptr);

        [DllImport(DllName, EntryPoint = "csharp_process", CallingConvention = CallingConvention.Cdecl)]
        public static extern void csharp_process();

        [DllImport(DllName, EntryPoint = "hello_rust", CallingConvention = CallingConvention.Cdecl)]
        public static extern void HelloRust();

        [DllImport(DllName, EntryPoint = "return_value", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReturnValue(byte r1, byte r2, byte r3, byte r4, byte r5, byte r6, byte b1);

    }
    
    
    
    public class Program
    {
       public  static  void  Main(string[] args)
        {
            try
            {
                Console.WriteLine("Calling LotteryImageslibNative from C#");
                LotteryImageslibNative.csharp_process();
              for (int i = 0; i < 100000; i++)
               {
                Stopwatch stopwatch = Stopwatch.StartNew();
                 Task task=   Task.Run( () =>    { 
                              int JKL = LotteryNative.ReturnValue(1, 2, 3, 4, 5, 6, 7);
                     Console.WriteLine($"Result from Rust: {JKL}");
                 } );
                    Task.WaitAll(task); 
                    stopwatch.Stop();
                    string path = @"Lottery.txt";
                    string content = stopwatch.ElapsedMilliseconds + Environment.NewLine;
                    Console.WriteLine(stopwatch.ElapsedMilliseconds);             
                    File.AppendAllText(path, content);

                    stopwatch.Reset();
                 }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling Rust library: {ex.Message}");
            }   
            finally
            {        
            }
           
            AnalyzeLottery(20);
        }
       public  static void AnalyzeLottery(int generateCount)
        {
            
        }
    }
}
