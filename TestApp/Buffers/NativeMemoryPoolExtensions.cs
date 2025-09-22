using System;
using System.Buffers;

namespace TestApp.Buffers
{
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
}
