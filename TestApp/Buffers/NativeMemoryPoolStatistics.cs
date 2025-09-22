using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Buffers
{
    // 池统计信息
    public class NativeMemoryPoolStatistics
    {
        public int TotalBuckets;
        public int TotalBuffers;
        public long TotalMemory;

        public override string ToString()
        {
            return $"Buckets: {TotalBuckets}, Buffers: {TotalBuffers}, Memory: {TotalMemory / 1024 / 1024} MB";
        }
    }
}
