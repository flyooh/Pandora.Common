namespace Pandora.Buffering
{
    /// <summary>
    /// A incoming buffer block.
    /// </summary>
    public class BufferBlockIn : DisposableObject
    {
        private BufferBlock _block;

        private int _cursorWR;

        private int _cursorRD;

        private BufferBlockIn(BufferBlock block)
        {
            _block = block;
            _cursorWR = 0;
            _cursorRD = 0;
        }

        /// <summary>
        /// Creates an IN block.
        /// </summary>
        /// <param name="bufferProvider">The buffer provider.</param>
        /// <param name="blockSize">The block size.</param>
        /// <returns>BufferBlockIn object.</returns>
        public static BufferBlockIn Create(IBufferProvider bufferProvider, int blockSize = 1024)
        {
            var block = bufferProvider.Alloc(blockSize);
            return new BufferBlockIn(block);
        }

        /// <summary>
        /// Gets data count.
        /// </summary>
        public int DataCount => _cursorWR;

        public void Reset()
        {
            EnsureAlive();

            _cursorWR = 0;
            _cursorRD = 0;
        }

        public Span<byte> Slice(int count)
        {
            return _block.Span.Slice(_cursorRD, count);
        }

        /// <summary>
        /// Addes an array of bytes to buffer.
        /// </summary>
        /// <param name="data">The data bytes.</param>
        public void AddBytes(Span<byte> data)
        {
            data.CopyTo(PrepareWriting(data.Length));
        }

        /// <summary>
        /// Reads an integral number from buffer.
        /// </summary>
        /// <returns>An integral number.</returns>
        public byte[] ReadBytes(int count)
        {
            return PrepareReading(count).ToArray();
        }

        public byte ReadByte()
        {
            var span = PrepareReading(sizeof(byte));
            return span[0];
        }

        public int ReadInt16()
        {
            return BinaryPrimitives.ReadInt16BigEndian(PrepareReading(sizeof(ushort)));
        }

        /// <summary>
        /// Reads an integral number from buffer.
        /// </summary>
        /// <returns>An integral number.</returns>
        public int ReadInt32()
        {
            return BinaryPrimitives.ReadInt32BigEndian(PrepareReading(sizeof(int)));
        }

        /// <summary>
        /// Reads an integral number from buffer.
        /// </summary>
        /// <returns>An integral number.</returns>
        public long ReadInt64()
        {
            return BinaryPrimitives.ReadInt64BigEndian(PrepareReading(sizeof(long)));
        }

        public float ReadFloat()
        {
            return BinaryPrimitives.ReadSingleBigEndian(PrepareReading(sizeof(float)));
        }

        public double ReadDouble()
        {
            EnsureAlive();

            return BinaryPrimitives.ReadDoubleBigEndian(PrepareReading(sizeof(double)));
        }

        public bool ReadBoolean()
        {
            byte value = ReadByte();
            return value != 0;
        }

        /// <summary>
        /// Reads a string from buffer.
        /// </summary>
        /// <returns>A string.</returns>
        public string ReadString()
        {
            int length = ReadInt32();
            if (length > 0)
            {
                return Encoding.UTF8.GetString(PrepareReading(length));
            }
            else
            {
                return string.Empty;
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

        private Span<byte> PrepareReading(int count)
        {
            EnsureAlive();

            int prevCursor = _cursorRD;
            _cursorRD += count;
            Ensure.Assert(_cursorRD <= _cursorWR, "Reading is out of range");
            return _block.Span.Slice(prevCursor, count);
        }

        private Span<byte> PrepareWriting(int count)
        {
            EnsureAlive();

            int prevCursor = _cursorWR;
            _cursorWR += count;
            Ensure.Assert(_cursorWR <= _block.Capacity, "Writing is out of range");
            return _block.Span.Slice(prevCursor, count);
        }
    }
}
