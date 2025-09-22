using System;
using System.Buffers;
using System.Collections.Generic;

namespace TestApp.Buffers.Internal
{
    // 内部桶类
    internal class NativeMemoryBucket<T> : IDisposable where T : unmanaged
    {
        private readonly Stack<NativeMemoryBuffer<T>> _buffers;
        private readonly int _size;
        private readonly int _maxBuffers;
        private bool _disposed;

        public int Size => _size;
        public int Count => _buffers.Count;

        public NativeMemoryBucket(int size, int maxBuffers)
        {
            _size = size;
            _maxBuffers = maxBuffers;
            _buffers = new Stack<NativeMemoryBuffer<T>>();
        }

        public IMemoryOwner<T> Rent()
        {
            lock (_buffers)
            {
                if (_buffers.TryPop(out NativeMemoryBuffer<T> buffer))
                {
                    return buffer;
                }
            }

            // 如果没有可用缓冲区，分配新的
            return new NativeMemoryBuffer<T>(_size, Return);
        }

        public void Return(NativeMemoryBuffer<T> buffer)
        {
            lock (_buffers)
            {
                if (_disposed || _buffers.Count >= _maxBuffers)
                {
                    // 如果桶已处置或已满，释放缓冲区
                    buffer.Dispose();
                }
                else
                {
                    _buffers.Push(buffer);
                }
            }
        }

        public void Clear()
        {
            lock (_buffers)
            {
                while (_buffers.TryPop(out NativeMemoryBuffer<T> buffer))
                {
                    buffer.Dispose();
                }
            }
        }

        public void Dispose()
        {
            _disposed = true;
            Clear();
        }
    }
}
