using PPMdArchiver.Core.Utils;
using System;

namespace PPMdArchiver.Core.Encoding
{
    internal class ArithmeticDecoder : IArithmeticDecoder
    {
        private readonly BitReader _bitReader;
        private uint _low = 0;
        private uint _high = 0xFFFFFFFF;
        private uint _value = 0;

        public ArithmeticDecoder(BitReader bitReader)
        {
            _bitReader = bitReader ?? throw new ArgumentNullException(nameof(bitReader));
        }

        public void Initialize()
        {
            _low = 0;
            _high = 0xFFFFFFFF;
            _value = 0;

            //reading starting value of 32 bits
            for (int i = 0; i < 32; i++)
            {
                if (_bitReader.IsEof)
                    throw new InvalidOperationException("Insufficient data to initialize decoder.");
                _value = (_value << 1) | _bitReader.ReadBit();
            }
        }

        public uint DecodeSymbol(uint total, out uint symbolLow, out uint symbolHigh)
        {
            if (total == 0)
                throw new ArgumentException("Total cannot be zero.", nameof(total));

            ulong range = (ulong)(_high - _low + 1);
            uint scaledValue = (uint)((((ulong)_value - _low + 1) * total - 1) / range);

            // TODO: replace plugs with real methods from context model
            symbolLow = scaledValue; // <-- change
            symbolHigh = scaledValue + 1; // <-- change

            _high = _low + (uint)((range * symbolHigh) / total - 1);
            _low = _low + (uint)((range * symbolLow) / total);

            while (true)
            {
                if ((_high & 0x80000000) == (_low & 0x80000000))
                {
                    _low <<= 1;
                    _high = (_high << 1) | 1;

                    // Safely cast byte to uint 
                    uint bit = 0;
                    if (!_bitReader.IsEof)
                    {
                        bit = _bitReader.ReadBit(); 
                    }
                    // byte | byte
                    _value = (_value << 1) | bit;
                }
                else if ((_low & 0x40000000) != 0 && (_high & 0x40000000) == 0)
                {
                    _low &= 0x3FFFFFFF;
                    _high |= 0x40000000;
                    _value ^= 0x40000000;
                }
                else
                {
                    break;
                }
            }

            // TODO: change returning value after implementing context model
            return scaledValue; // change to: return symbolIndex;
        }
    }
}