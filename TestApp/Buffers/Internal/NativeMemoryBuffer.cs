using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TestApp.Buffers.Internal
{
    // 本地内存缓冲区
    internal unsafe sealed class NativeMemoryBuffer<T> : IMemoryOwner<T> where T : unmanaged
    {
        private readonly void* _memoryPtr;
        private readonly int _length;
        private readonly Memory<T> _memory;
        private bool _disposed;
        private readonly Action<NativeMemoryBuffer<T>> _returnCallback;
        ////private bool _returned;

        public int Length => _length;
        public Memory<T> Memory => _disposed ? throw new ObjectDisposedException("NativeMemoryBuffer") : _memory;

        public NativeMemoryBuffer(int length, Action<NativeMemoryBuffer<T>> returnCallback)
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
        private Memory<T> CreateMemoryWrapper()
        {
            // 使用MemoryMarshal.CreateFromPinnedArray需要数组，但我们没有数组
            // 所以我们需要使用自定义的MemoryManager
            return new NativeMemoryManager<T>(_memoryPtr, _length).Memory;
        }

        public void Dispose()
        {
            ////if (_returned) return;

            _returnCallback(this);
            ////_returned = true;
        }

        ~NativeMemoryBuffer()
        {
            if (!_disposed)
            {
                // 在终结器中释放内存，防止内存泄漏
                Dispose();
            }
        }
    }
}
