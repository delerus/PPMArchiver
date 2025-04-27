using System;

namespace PPMdArchiver.Core.Utils
{
    /// <summary>
    /// Bit reader class for reading bits from stream
    /// </summary>
    internal class BitReader : IDisposable
    {
        private readonly Stream _stream;
        private byte _buffer = 0;
        private int _bitsInBuffer = 0;

        public BitReader(Stream stream)
        {
            _stream = stream;
        }

        // TODO: Write logic to read single bit
        public byte ReadBit()
        {
            // Not implemented yet.

            return (byte)((_buffer >> _bitsInBuffer) & 1);
        }

        //Write logic to read multiple bits
        public uint ReadBits(int bitCount)
        {
            // Not implemented yet.

            return 1;
        }

        public bool IsEof => _stream.Position >= _stream.Length && _bitsInBuffer == 0;

        // TODO: Write dispose mechanism for BitReader class
        public void Dispose()
        {
            // Not implemented yet.
        }

    }
}
