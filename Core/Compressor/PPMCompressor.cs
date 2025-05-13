using PPMdArchiver.Core.Contexts;
using PPMdArchiver.Core.Encoding;
using PPMdArchiver.Core.IO;
using PPMdArchiver.Core.Utils;
using System;
using System.IO;

namespace PPMdArchiver.Core.Compressor
{
    internal class PPMCompressor : IPPMCompressor
    {
        private IContextModel _contextModel;
        private const int BufferSize = 4096;

        public void Initialize(int order, int memoryMb)
        {
            _contextModel = new PPMdContextModel();
            _contextModel.Initialize(order, memoryMb);
        }

        public bool Compress(string inputFile, string outputFile)
        {
            throw new NotImplementedException();
        }

        public bool Decompress(string inputFile, string outputFile)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
