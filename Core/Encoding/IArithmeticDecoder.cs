using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPMdArchiver.Core.Encoding
{
    internal interface IArithmeticDecoder
    {
        void Initialize();
        uint GetScaledValue(uint total); 
        void UpdateRange(uint symbolLow, uint symbolHigh, uint total); 
    }
}
