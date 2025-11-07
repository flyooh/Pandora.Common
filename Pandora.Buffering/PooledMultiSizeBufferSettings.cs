namespace Pandora.Buffering
{
    /// <summary>
    /// Represents a strategy managing buffer blocks.
    /// </summary>
    public class PooledMultiSizeBufferSettings
    {
        public PooledMultiSizeBufferSettings()
        {
            Configurations = new Dictionary<int, int>();
            SupportedSizes = new SortedSet<int>();
        }

        public Dictionary<int, int> Configurations { get; }

        public SortedSet<int> SupportedSizes { get; }

        public TimeSpan AllocTimeout { get; set; } = Timeout.InfiniteTimeSpan;

        public PooledMultiSizeBufferSettings AddConfig(int blockSize, int blockCount)
        { 
            Configurations[blockSize] = blockCount;
            SupportedSizes.Add(blockSize);
            return this;
        }

        public int GetTotalMemorySize()
        { 
            int totalMemory = 0;
            foreach (var kvp in Configurations)
            { 
                totalMemory += kvp.Value * kvp.Key;
            }

            return totalMemory;
        }
    }
}
