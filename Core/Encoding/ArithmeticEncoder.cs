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

        public void Initialize()
        {
            _low = 0;
            _high = 0xFFFFFFFF;
            _scale = 0;
        }

        public void EncodeSymbol(uint low, uint high, uint total)
        {
            if (low >= high || high > total || total == 0)
                throw new ArgumentException("Invalid probability range or total.");

            // Current range calculation
            ulong range = (ulong)(_high - _low + 1);

            // Narrowing range
            _high = _low + (uint)((range * high) / total - 1);
            _low = _low + (uint)((range * low) / total);

            while (true)
            {
                // Condition 1: If high bits match
                if ((_high & 0x80000000) == (_low & 0x80000000))
                {
                    _bitWriter.WriteBit((byte)((_high >> 31) & 1));
                    while (_scale > 0)
                    {
                        _bitWriter.WriteBit((byte)((~_high >> 31) & 1));
                        _scale--;
                    }

                    _low <<= 1;
                    _high = (_high << 1) | 1;
                }
                // Condition 2: If there is a carry 
                else if ((_low & 0x40000000) != 0 && (_high & 0x40000000) == 0)
                {
                    _scale++; 
                    _low &= 0x3FFFFFFF; 
                    _high |= 0x40000000; 
                }
                // Condition 3: If range is stable
                else
                {
                    break;
                }
            }
        }

        public void Flush()
        {
            _bitWriter.WriteBit((byte)((_low >> 31) & 1));
            _bitWriter.WriteBit((byte)((_low >> 30) & 1));
            while (_scale > 0)
            {
                _bitWriter.WriteBit((byte)((~_low >> 31) & 1));
                _scale--;
            }
            _bitWriter.Flush();
        }
    }
}
