using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace TestApp.Buffers
{
    // 自定义MemoryManager
    internal sealed unsafe class NativeMemoryManager<T> : MemoryManager<T> where T : unmanaged
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
