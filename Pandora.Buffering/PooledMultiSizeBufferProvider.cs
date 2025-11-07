namespace Pandora.Buffering
{
    public class PooledMultiSizeBufferProvider : IBufferProvider
    {
        private readonly PooledSingleSizeBufferProvider[] _providers;

        private readonly PooledMultiSizeBufferSettings _strategy;

        private PooledMultiSizeBufferProvider(PooledSingleSizeBufferProvider[] providers, PooledMultiSizeBufferSettings strategy)
        {
            _providers = providers;
            _strategy = strategy;
        }

        public static PooledMultiSizeBufferProvider Create(PooledMultiSizeBufferSettings strategy)
        {
            if (strategy.Configurations.Count == 0)
            {
                throw new InvalidOperationException("Strategy is empty");
            }

            int totalMemorySize = strategy.GetTotalMemorySize();
            if (totalMemorySize == 0)
            {
                throw new InvalidOperationException("Required memory size is zero");
            }

            var subProviders = new PooledSingleSizeBufferProvider[strategy.Configurations.Count];
        
            int index = 0;
            foreach (var blockSize in strategy.SupportedSizes)
            {
                int blockCount = strategy.Configurations[blockSize];
                var subProvider = new PooledSingleSizeBufferProvider(blockSize, blockCount);
                subProviders[index++] = subProvider;
            }

            return new PooledMultiSizeBufferProvider(subProviders, strategy);
        }

        public BufferBlock Alloc(int size)
        {
            var subProvider = GetApproriateProvider(size);
            if (subProvider == null)
            {
                throw new Exception("Block size is too big and not supported");
            }

            var block = subProvider.Alloc(size, _strategy.AllocTimeout);
            if (block == null)
            {
                throw new OutOfMemoryException();
            }
        
            return block;
        }

        public void Dealloc(BufferBlock block)
        {
            throw new NotImplementedException();
        }

        private PooledSingleSizeBufferProvider GetApproriateProvider(int size)
        {
            foreach (var subProvider in _providers)
            {
                if (subProvider.CanSupport(size))
                {
                    return subProvider;
                }
            }

            return null;
        }
    }
}
