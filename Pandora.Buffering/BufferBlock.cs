namespace Pandora.Buffering
{
    /// <summary>
    /// Represents a memory block.
    /// </summary>
    public class BufferBlock
    {
        private Memory<byte> _buffer;

        private IBufferProvider _provider;

        /// <summary>
        /// Initializes a new instance of the BufferBlock class.
        /// </summary>
        /// <param name="provider">The parent provider.</param>
        /// <param name="buffer">The buffer.</param>
        public BufferBlock(IBufferProvider provider, Memory<byte> buffer)
        {
            this._provider = provider;
            this._buffer = buffer;
        }

        /// <summary>
        /// Gets the buffer.
        /// </summary>
        public Span<byte> Span => _buffer.Span;

        /// <summary>
        /// Gets capacility of the memory block.
        /// </summary>
        public int Capacity => _buffer.Length;

        /// <summary>
        /// Releases this block.
        /// </summary>
        public void Release()
        {
            _provider.Dealloc(this);
        }
    }
}
