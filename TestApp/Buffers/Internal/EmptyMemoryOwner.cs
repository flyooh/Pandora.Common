using System;
using System.Buffers;

namespace TestApp.Buffers.Internal
{
    // 空内存所有者（用于零长度请求）
    internal class EmptyMemoryOwner<T> : IMemoryOwner<T> where T : unmanaged
    {
        public Memory<T> Memory => Memory<T>.Empty;

        public void Dispose()
        {
        }
    }
}
