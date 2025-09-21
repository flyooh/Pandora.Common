using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TestApp
{
    /// <summary>
    /// 基于 NativeMemory 的高性能内存池实现
    /// 提供对非托管内存的高效分配和管理，避免 GC 压力
    /// </summary>
    /// <typeparam name="T">非托管类型</typeparam>
    public sealed class NativeMemoryPool<T> : IDisposable where T : unmanaged
    {
        // 存储不同大小桶的字典
        private readonly ConcurrentDictionary<int, Bucket> _buckets;

        // 默认配置
        private const int DefaultMaxBuffersPerBucket = 20;
        private const int MinBufferSizeBytes = 1024; // 1KB
        private const int MaxBufferSizeBytes = 128 * 1024 * 1024; // 128MB

        // 是否已释放
        private bool _disposed;

        // 单例实例
        private static readonly Lazy<NativeMemoryPool<T>> _shared =
            new Lazy<NativeMemoryPool<T>>(() => new NativeMemoryPool<T>());

        public static NativeMemoryPool<T> Shared => _shared.Value;

        public NativeMemoryPool()
        {
            _buckets = new ConcurrentDictionary<int, Bucket>();
            _disposed = false;
        }

        /// <summary>
        /// 租用指定大小的内存块
        /// </summary>
        /// <param name="minimumLength">最小长度（元素数量）</param>
        /// <returns>内存块所有者</returns>
        public IMemoryOwner<T> Rent(int minimumLength)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(NativeMemoryPool<T>));

            if (minimumLength < 0)
                throw new ArgumentOutOfRangeException(nameof(minimumLength));

            if (minimumLength == 0)
                return new EmptyMemoryOwner();

            // 计算实际需要分配的大小
            int actualSize = CalculateActualSize(minimumLength);

            // 获取或创建对应大小的桶
            var bucket = _buckets.GetOrAdd(actualSize,
                size => new Bucket(size, DefaultMaxBuffersPerBucket));

            // 从桶中租用内存
            return bucket.Rent();
        }

        /// <summary>
        /// 清理所有桶并释放所有内存
        /// </summary>
        public void Clear()
        {
            foreach (var bucket in _buckets.Values)
            {
                bucket.Clear();
            }
            _buckets.Clear();
        }

        /// <summary>
        /// 获取池统计信息
        /// </summary>
        public PoolStatistics GetStatistics()
        {
            var stats = new PoolStatistics();

            foreach (var bucket in _buckets.Values)
            {
                stats.TotalBuckets++;
                stats.TotalBuffers += bucket.Count;
                stats.TotalMemory += (long)bucket.Size * bucket.Count * Unsafe.SizeOf<T>();
            }

            return stats;
        }

        /// <summary>
        /// 释放所有资源
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                foreach (var bucket in _buckets.Values)
                {
                    bucket.Dispose();
                }
                _buckets.Clear();
                _disposed = true;
            }
        }

        // 计算实际需要分配的大小
        private int CalculateActualSize(int minimumLength)
        {
            // 计算字节大小
            int byteSize = minimumLength * Unsafe.SizeOf<T>();

            // 应用最小分配限制
            if (byteSize < MinBufferSizeBytes)
                byteSize = MinBufferSizeBytes;

            // 应用最大分配限制
            if (byteSize > MaxBufferSizeBytes)
                throw new ArgumentOutOfRangeException(
                    nameof(minimumLength),
                    $"Requested size is too large. Maximum allowed: {MaxBufferSizeBytes / Unsafe.SizeOf<T>()} elements");

            // 向上取整到最近的 4KB 边界（内存页大小）
            const int pageSize = 4096;
            int alignedByteSize = ((byteSize + pageSize - 1) / pageSize) * pageSize;

            // 转换回元素数量
            return alignedByteSize / Unsafe.SizeOf<T>();
        }

        // 内部桶类
        private class Bucket : IDisposable
        {
            private readonly ConcurrentStack<NativeMemoryBuffer> _buffers;
            private readonly int _size;
            private readonly int _maxBuffers;
            private bool _disposed;

            public int Size => _size;
            public int Count => _buffers.Count;

            public Bucket(int size, int maxBuffers)
            {
                _size = size;
                _maxBuffers = maxBuffers;
                _buffers = new ConcurrentStack<NativeMemoryBuffer>();
            }

            public IMemoryOwner<T> Rent()
            {
                if (_buffers.TryPop(out NativeMemoryBuffer buffer))
                {
                    return buffer;
                }

                // 如果没有可用缓冲区，分配新的
                return new NativeMemoryBuffer(_size, this.Return);
            }

            public void Return(NativeMemoryBuffer buffer)
            {
                if (_disposed || _buffers.Count >= _maxBuffers)
                {
                    // 如果桶已处置或已满，释放缓冲区
                    buffer.Dispose();
                    return;
                }

                _buffers.Push(buffer);
            }

            public void Clear()
            {
                while (_buffers.TryPop(out NativeMemoryBuffer buffer))
                {
                    buffer.Dispose();
                }
            }

            public void Dispose()
            {
                _disposed = true;
                Clear();
            }
        }

        // 空内存所有者（用于零长度请求）
        private sealed class EmptyMemoryOwner : IMemoryOwner<T>
        {
            public Memory<T> Memory => Memory<T>.Empty;

            public void Dispose()
            {
                // 空实现，无需释放任何资源
            }
        }

        // 本地内存缓冲区
        private unsafe sealed class NativeMemoryBuffer : IMemoryOwner<T>
        {
            private readonly void* _memoryPtr;
            private readonly int _length;
            private readonly Memory<T> _memory;
            private bool _disposed;
            private readonly Action<NativeMemoryBuffer> _returnCallback;
            private bool _returned;

            public int Length => _length;
            public Memory<T> Memory => _disposed ? throw new ObjectDisposedException("NativeMemoryBuffer") : _memory;

            public NativeMemoryBuffer(int length, Action<NativeMemoryBuffer> returnCallback)
            {
                _length = length;
                _disposed = false;

                // 计算字节大小
                int byteSize = length * Unsafe.SizeOf<T>();

                // 分配对齐的内存（64字节对齐，对SIMD操作友好）
                _memoryPtr = NativeMemory.AlignedAlloc((nuint)byteSize, 64);

                // 初始化内存为零
                NativeMemory.Clear(_memoryPtr, (nuint)byteSize);

                // 创建Memory<T>包装器
                _memory = CreateMemoryWrapper();
                _returnCallback = returnCallback;
            }

            // 创建Memory<T>包装器
            private unsafe Memory<T> CreateMemoryWrapper()
            {
                // 使用MemoryMarshal.CreateFromPinnedArray需要数组，但我们没有数组
                // 所以我们需要使用自定义的MemoryManager
                return new NativeMemoryManager(_memoryPtr, _length).Memory;
            }

            public void Dispose()
            {
                if (_returned) return;

                _returnCallback(this);
                _returned = true;
            }

            ~NativeMemoryBuffer()
            {
                if (!_disposed)
                {
                    // 在终结器中释放内存，防止内存泄漏
                    Dispose();
                }
            }

            // 自定义MemoryManager
            private sealed unsafe class NativeMemoryManager : MemoryManager<T>
            {
                private readonly void* _pointer;
                private readonly int _length;

                public NativeMemoryManager(void* pointer, int length)
                {
                    _pointer = pointer;
                    _length = length;
                }

                public override Span<T> GetSpan()
                {
                    return new Span<T>(_pointer, _length);
                }

                public override MemoryHandle Pin(int elementIndex = 0)
                {
                    // 内存已经是固定的，直接返回指针
                    return new MemoryHandle(Unsafe.Add<T>(_pointer, elementIndex));
                }

                public override void Unpin()
                {
                    // 无需操作，内存本来就是固定的
                }

                protected override void Dispose(bool disposing)
                {
                    // 由NativeMemoryBuffer负责释放内存
                }
            }
        }

        // 池统计信息
        public struct PoolStatistics
        {
            public int TotalBuckets;
            public int TotalBuffers;
            public long TotalMemory;

            public override string ToString()
            {
                return $"Buckets: {TotalBuckets}, Buffers: {TotalBuffers}, Memory: {TotalMemory / 1024 / 1024} MB";
            }
        }
    }

    // 使用示例和扩展方法
    public static class NativeMemoryPoolExtensions
    {
        // 租用内存并返回Span<T>
        public static Span<T> RentSpan<T>(this NativeMemoryPool<T> pool, int minimumLength) where T : unmanaged
        {
            var owner = pool.Rent(minimumLength);
            return owner.Memory.Span;
        }

        // 高性能复制方法
        public static unsafe void CopyToNative<T>(this ReadOnlySpan<T> source, NativeMemoryPool<T> pool, out IMemoryOwner<T> destination) where T : unmanaged
        {
            destination = pool.Rent(source.Length);
            Span<T> destSpan = destination.Memory.Span;

            fixed (T* sourcePtr = source)
            fixed (T* destPtr = destSpan)
            {
                Buffer.MemoryCopy(
                    sourcePtr,
                    destPtr,
                    destSpan.Length * sizeof(T),
                    source.Length * sizeof(T));
            }
        }
    }

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