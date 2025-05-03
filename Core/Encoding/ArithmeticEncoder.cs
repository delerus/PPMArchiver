using PPMdArchiver.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPMdArchiver.Core.Encoding
{
    internal class ArithmeticEncoder : IArithmeticEncoder
    {
        private readonly BitWriter _bitWriter;
        private uint _low = 0;
        private uint _high = 0xFFFFFFFF;
        private uint _scale = 0;

        public ArithmeticEncoder(BitWriter bitWriter)
        {
            _bitWriter = bitWriter;
        }

        public void Initialize(Stream output)
        {

        }

        public void EncodeSymbol(uint low, uint high, uint total)
        {

        }

        public void Flush()
        {

        }
    }
}
