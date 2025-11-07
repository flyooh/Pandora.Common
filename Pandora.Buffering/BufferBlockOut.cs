namespace Pandora.Buffering
{
    /// <summary>
    /// An outgoing buffer block.
    /// </summary>
    public class BufferBlockOut : DisposableObject
    {
        private BufferBlock _block;

        private int _cursor;

        private BufferBlockOut(BufferBlock block)
        {
            _block = block;
            _cursor = _block.Capacity;
        }

        /// <summary>
        /// Creates an OUT block.
        /// </summary>
        /// <param name="bufferProvider">The buffer provider.</param>
        /// <param name="blockSize">The block size.</param>
        /// <returns>BufferBlockOut object.</returns>
        public static BufferBlockOut Create(IBufferProvider bufferProvider, int blockSize = 1024)
        {
            var block = bufferProvider.Alloc(blockSize);
            return new BufferBlockOut(block);
        }

        public Span<byte> Data => _block.Span[_cursor..];

        /// <summary>
        /// Gets data count.
        /// </summary>
        public int DataCount => _block.Capacity - _cursor;

        public void Reset()
        {
            EnsureAlive();

            _cursor = _block.Capacity;
        }

        /// <summary>
        /// Moves a distance from back to front.
        /// </summary>
        /// <param name="count">The offset.</param>
        public Span<byte> PrepareWriting(int count)
        {
            EnsureAlive();

            _cursor -= count;
            Ensure.Assert(_cursor >= 0, "Writing is out of range");
            return _block.Span.Slice(_cursor, count);
        }

        /// <summary>
        /// Adds a byte into buffer.
        /// </summary>
        /// <param name="b">The value.</param>
        public void AddByte(byte b)
        {
            var span = PrepareWriting(sizeof(byte));
            span[0] = b;
        }

        /// <summary>
        /// Adds an array of bytes into buffer.
        /// </summary>
        /// <param name="data">The bytes</param>
        public void AddBytes(Span<byte> data)
        {
            data.CopyTo(PrepareWriting(data.Length));
        }

        public void AddInt16(short value)
        {
            BinaryPrimitives.WriteInt16BigEndian(PrepareWriting(sizeof(short)), value);
        }

        /// <summary>
        /// Adds an integral number to buffer.
        /// </summary>
        /// <param name="value">The integral number.</param>
        public void AddInt32(int value)
        {
            BinaryPrimitives.WriteInt32BigEndian(PrepareWriting(sizeof(int)), value);
        }

        /// <summary>
        /// Adds an integral number to buffer.
        /// </summary>
        /// <param name="value">The integral number.</param>
        public void AddInt64(long value)
        {
            BinaryPrimitives.WriteInt64BigEndian(PrepareWriting(sizeof(long)), value);
        }

        public void AddSingle(float value)
        { 
            BinaryPrimitives.WriteSingleBigEndian(PrepareWriting(sizeof(float)), value);
        }

        public void AddDouble(double value)
        {
            BinaryPrimitives.WriteDoubleBigEndian(PrepareWriting(sizeof(double)), value);
        }

        public void AddBoolean(bool value)
        { 
            AddByte(value ?  (byte)1 : (byte)0);
        }

        /// <summary>
        /// Adds a string to buffer.
        /// </summary>
        /// <param name="value">The string value.</param>
        public void AddString(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                byte[] data = Encoding.UTF8.GetBytes(value);
                AddBytes(data.AsSpan());
                AddInt32(data.Length);
            }
            else
            {
                AddInt32(0);
            }
        }

        protected override void DisposeManagedResources()
        {
            if (_block != null)
            {
                _block.Release();
                _block = null;
            }
        }
    }
}
