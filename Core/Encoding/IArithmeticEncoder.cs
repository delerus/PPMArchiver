using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPMdArchiver.Core.Encoding
{
    internal interface IArithmeticEncoder
    {
        void Initialize(Stream output);
        void EncodeSymbol(uint low, uint high, uint total);
        void Flush();
    }
}
