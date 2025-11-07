namespace Pandora.Buffering
{
    public class DefaultBufferProvider : IBufferProvider
    {
        public readonly static IBufferProvider Instance = new DefaultBufferProvider();

        public BufferBlock Alloc(int size)
        {
            byte[] bytes = new byte[size];
            return new BufferBlock(this, bytes.AsMemory());
        }

        public void Dealloc(BufferBlock block)
        {
            // Release the object and let GC to handle it.
        }
    }
}
