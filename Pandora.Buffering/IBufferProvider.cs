namespace Pandora.Buffering
{
    public interface IBufferProvider
    {
        BufferBlock Alloc(int size);

        void Dealloc(BufferBlock block);
    }
}
