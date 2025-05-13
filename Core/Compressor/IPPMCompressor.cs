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
        bool Compress(string inputFile, string outputFile);
        bool Decompress(string inputFile, string outputFile);
        void Reset();
    }
}
