using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace EarthAnalysis
{
    public class LockTask 
    {

      private readonly  object lockA = new object();
    private readonly    object lockB = new object();

        void TestDeadLock()
        {
            // 线程1：先锁A，再锁B
            new Thread(() =>
            {
                lock (lockA)
                {
                    Thread.Sleep(100); // 让死锁更容易触发
                    lock (lockB)
                    {
                        Console.WriteLine("线程1执行完成");
                    }
                }
            }).Start();

            // 线程2：先锁B，再锁A → 死锁！
            new Thread(() =>
            {
                lock (lockB)
                {
                    Thread.Sleep(100);
                    lock (lockA)
                    {
                        Console.WriteLine("线程2执行完成");
                    }
                }
            }).Start();
        }


        void FixByOrder()
        {
            new Thread(() =>
            {
                lock (lockA)
                    lock (lockB)
                    {
                        Console.WriteLine("线程1执行完成");
                    }
            }).Start();

            new Thread(() =>
            {
                // 统一顺序：先A后B
                lock (lockA)
                    lock (lockB)
                    {
                        Console.WriteLine("线程2执行完成");
                    }
            }).Start();
        }

        void FixByTryLock()
        {
            new Thread(() => Work(1, lockA, lockB)).Start();
            new Thread(() => Work(2, lockB, lockA)).Start();
        }

        void Work(int id, object firstLock, object secondLock)
        {
            if (Monitor.TryEnter(firstLock, 500))
            {
                try
                {
                    if (Monitor.TryEnter(secondLock, 500))
                    {
                        try
                        {
                            Console.WriteLine($"线程{id}执行完成");
                        }
                        finally
                        {
                            Monitor.Exit(secondLock);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"线程{id}获取第二个锁失败");
                    }
                }
                finally
                {
                    Monitor.Exit(firstLock);
                }
            }
            else
            {
                Console.WriteLine($"线程{id}获取第一个锁失败");
            }
        }

        private readonly SemaphoreSlim _asyncLock = new SemaphoreSlim(1);

        async Task SafeAsyncMethod()
        {
            await _asyncLock.WaitAsync();
            try
            {
                // 你的异步逻辑：读写PLC、数据库、串口
                await Task.Delay(100);
            }
            finally
            {
                _asyncLock.Release();
            }
        }
    }

    /// <summary>
    ///  标准锁最佳实践（杜绝 90% 锁错误）
    /// </summary>
    public class DeviceService
    {
        // 1. 必须 private readonly
        private readonly object _lockObj = new object();

        private int _runCount;

        public void DoWork()
        {
            // 2. 临界区尽量短
            lock (_lockObj)
            {
                _runCount++;
            }

            // 耗时操作放锁外面
            //HeavyIoOperation();
        }

        // 错误示范永远不要写：
        // lock(this)
        // lock("mylock")
        // lock(123)
    }

    /// <summary>
    /// 异步线程安全（async/await 专用）
    /// lock 不能跨 await，必须用 SemaphoreSlim
    /// </summary>
    public class AsyncDataService
    {
        private readonly SemaphoreSlim _asyncLock = new SemaphoreSlim(1);

        public async Task SaveDataAsync(string data)
        {
            // 异步加锁
            await _asyncLock.WaitAsync();

            try
            {
                // 异步 IO：数据库/网络/PLC
                await File.WriteAllTextAsync("data.txt", data);
            }
            finally
            {
                _asyncLock.Release();
            }
        }
    }


    /// <summary>
    /// 原子操作（无锁、高性能计数）
    /// 适合状态标记、计数器、启停标志
    /// </summary>
    public class InterlockedExample
    {

        private int _isRunning = 0;
        private int _errorCount = 0;
        public bool Start()
        {
            // 原子替换：0→1，成功返回旧值0
            return Interlocked.CompareExchange(ref _isRunning, 1, 0) == 0;
        }

        public void Stop()
        {
            Interlocked.Exchange(ref _isRunning, 0);
        }

        public void IncrementError()
        {
            // 原子自增
            Interlocked.Increment(ref _errorCount);
        }

       

    }

        /// <summary>
        ///     线程安全字典（上位机设备状态缓存）
        /// </summary>
    public class ConcurrentDictionaryExample
    { 
        private readonly ConcurrentDictionary<string, string> _data = new ConcurrentDictionary<string, string>();
        public void AddOrUpdate(string key, string value)
        {
            _data.AddOrUpdate(key, value, (k, oldValue) => value);
        }
        public bool TryGetValue(string key, out string value)
        {
            return _data.TryGetValue(key, out value);
        }

        ConcurrentDictionary<string, float> _deviceValues = new();

        public void UpdateDeviceValue()
        {
            // 写入
            _deviceValues["Temperature"] = 25.5f;

            // 读取
            if (_deviceValues.TryGetValue("Temperature", out var temp))
            {
                // UI 显示
            }
        }

}

    /// <summary>
    /// 生产者消费者模型（数据采集最稳方案）
    /// 上位机采集线程 → 入队；处理线程 → 单线程消费无锁、不死锁、不丢数据
    /// </summary>
    public class BlockingCollectionExample
    {
        BlockingCollection<double> _dataQueue = new();

        // 生产者（PLC/串口采集线程）
        void ProducerThread()
        {
            while (true)
            {
                var data = 23.6;    ///    ReadFromPlc();
                _dataQueue.Add(data);
            }
        }

        // 消费者（单线程处理）
        void ConsumerThread()
        {
            foreach (var data in _dataQueue.GetConsumingEnumerable())
            {
                 //  ProcessData(data); // 单线程天然安全
            }
        }
    }

    /// <summary>
    ///  ReaderWriterLockSlim（读多写少） 
    ///  适合配置、参数、实时数据展示
    /// </summary>
    public class ReaderWriterLockSlimExample
    {
        private int _value = 0;

        ReaderWriterLockSlim _rwLock = new();

        // 读（多人可同时读）
        public float GetValue()
        {
            _rwLock.EnterReadLock();
            try { return _value; }
            finally { _rwLock.ExitReadLock(); }
        }

        // 写（独占）
        public void SetValue(int vg)
        {
            _rwLock.EnterWriteLock();
            try { _value = vg; }
            finally { _rwLock.ExitWriteLock(); }
        }
    }

    /// <summary>
    /// 带超时的 TryLock（工业上位机必备）
    /// 绝不卡死界面，适合设备控制
    /// </summary>
    public class MonitorTryEnterExample
    {

        private readonly object _lockObj = new object();
        public void Demo()
        {
            if (Monitor.TryEnter(_lockObj, 300)) // 300ms 超时
            {
                try
                {
                    // 操作串口/PLC
                }
                finally
                {
                    Monitor.Exit(_lockObj);
                }
            }
            else
            {
                //Logger.Warn("获取资源超时，设备忙");
            }
        }

    }


 

}
