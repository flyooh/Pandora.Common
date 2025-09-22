using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace TestApp.Buffers
{
    /// <summary>
    /// 基于 NativeMemory 的高性能内存池实现
    /// 提供对非托管内存的高效分配和管理，避免 GC 压力
    /// </summary>
    /// <typeparam name="T">非托管类型</typeparam>
    public sealed class NativeMemoryPool<T> : IDisposable where T : unmanaged
    {
        // 存储不同大小桶的字典
        private readonly ConcurrentDictionary<int, NativeMemoryBucket<T>> _buckets;

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
            _buckets = new ConcurrentDictionary<int, NativeMemoryBucket<T>>();
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
                return new EmptyMemoryOwner<T>();

            // 计算实际需要分配的大小
            int actualSize = CalculateActualSize(minimumLength);

            // 获取或创建对应大小的桶
            var bucket = _buckets.GetOrAdd(actualSize,
                size => new NativeMemoryBucket<T>(size, DefaultMaxBuffersPerBucket));

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
        public NativeMemoryPoolStatistics GetStatistics()
        {
            var stats = new NativeMemoryPoolStatistics();

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
            int alignedByteSize = (byteSize + pageSize - 1) / pageSize * pageSize;

            // 转换回元素数量
            return alignedByteSize / Unsafe.SizeOf<T>();
        }
    }
}