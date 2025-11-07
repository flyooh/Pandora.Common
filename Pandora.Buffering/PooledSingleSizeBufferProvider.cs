namespace Pandora.Buffering;

public class PooledSingleSizeBufferProvider : DisposableObject, IBufferProvider
{
    private readonly Queue<BufferBlock> _blocks;

    private readonly SemaphoreSlim _semaphore;

    private readonly int _blockCount;

    private readonly int _blockSize;

    public PooledSingleSizeBufferProvider(int blockSize, int blockCount)
    {
        Ensure.Assert(blockCount > 0, "Invalid 'capacity");
        Ensure.Assert(blockSize > 0 && blockSize % 4 == 0, "Invalid block size");

        _blockCount = blockCount;
        _blockSize = blockSize;
        _blocks = new Queue<BufferBlock>();
        _semaphore = new SemaphoreSlim(blockCount, blockCount);

        InitializeBlocks();
    }

    public bool CanSupport(int size)
    {
        return _blockSize >= size;
    }

    public BufferBlock Alloc(int size)
    {
        EnsureAlive();

        _semaphore.Wait();

        lock (_blocks)
        {
            return _blocks.Dequeue();
        }
    }

    internal BufferBlock Alloc(int size, TimeSpan timeout)
    {
        EnsureAlive();

        if (!_semaphore.Wait(timeout))
        {
            return null;
        }

        lock (_blocks)
        {
            return _blocks.Dequeue();
        }
    }

    public void Dealloc(BufferBlock block)
    {
        EnsureAlive();

        lock (_blocks)
        {
            _blocks.Enqueue(block);
        }

        _semaphore.Release();
    }

    protected override void DisposeManagedResources()
    {
        _semaphore.Dispose();
    }

    private void InitializeBlocks()
    {
        var bytes = new byte[_blockSize * _blockCount];
        for (int i = 0, offset = 0; i < _blockCount; ++i, offset += _blockSize)
        {
            BufferBlock block = new BufferBlock(this, bytes.AsMemory(offset, _blockSize));
            _blocks.Enqueue(block);
        }
    }
}
