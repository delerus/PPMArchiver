using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPMdArchiver.Core.Encoding
{
    internal interface IArithmeticDecoder
    {
        void Initialize(Stream input);
        uint DecodeSymbol(uint total);
    }
}
