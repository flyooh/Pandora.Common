using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Buffers
{
    // 使用示例
    public class NativeMemoryPoolExample
    {
        public void ProcessLargeData()
        {
            var pool = NativeMemoryPool<byte>.Shared;

            // 租用大内存
            using (IMemoryOwner<byte> buffer = pool.Rent(1024 * 1024)) // 1MB
            {
                // 使用内存
                Span<byte> span = buffer.Memory.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)(i % 256);
                }

                // 处理数据
                ProcessData(span);
            } // 自动释放内存
        }

        public void ProcessWithSpan()
        {
            var pool = NativeMemoryPool<float>.Shared;

            // 租用内存并获取Span
            Span<float> data = pool.RentSpan(1000000);

            try
            {
                // 使用Span进行操作
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = MathF.Sin(i * 0.01f);
                }

                // 处理数据
                ProcessData(data);
            }
            finally
            {
                // 注意：这里无法自动释放，需要手动管理
                // 更好的方式是使用using语句包装IMemoryOwner
            }
        }

        public void MonitorPoolUsage()
        {
            var pool = NativeMemoryPool<byte>.Shared;

            // 获取池统计信息
            var stats = pool.GetStatistics();
            Console.WriteLine($"Pool statistics: {stats}");

            // 定期清理池
            if (stats.TotalMemory > 100 * 1024 * 1024) // 100MB阈值
            {
                Console.WriteLine("Clearing pool due to high memory usage");
                pool.Clear();
            }
        }

        private void ProcessData(Span<byte> data)
        {
            // 数据处理逻辑
        }

        private void ProcessData(Span<float> data)
        {
            // 数据处理逻辑
        }
    }
}
