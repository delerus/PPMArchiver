using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPMdArchiver.Core.Compressor
{
    internal interface IPPMCompressor
    {
        void Initialize(int order, int memoryMb);
        // TODO: write parameters for Compress function
        bool Compress();
        // TODO: write parameters for Decompress function
        bool Decompress();
        void Reset();

    }
}
