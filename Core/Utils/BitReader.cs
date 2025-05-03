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
        private bool _disposed = false;

        public BitReader(Stream stream)
        {
            if (stream == null || !stream.CanRead)
                throw new ArgumentException("Stream must be readable.");
            _stream = stream;
        }

        public byte ReadBit()
        {
           if (_disposed)
               throw new ObjectDisposedException(nameof(BitReader));

            if (_bitsInBuffer == 0)
            {
                int nextByte = _stream.ReadByte();
                if (nextByte == -1)
                    throw new EndOfStreamException("Reached end of stream while reading bit.");
                _buffer = (byte)nextByte;
                _bitsInBuffer = 8;
            }

            _bitsInBuffer--;
            return (byte)((_buffer >> _bitsInBuffer) & 1);
        }

        public uint ReadBits(int bitCount)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(BitReader));

            if (bitCount < 0 || bitCount > 32)
                throw new ArgumentOutOfRangeException(nameof(bitCount), "bitCount must be between 0 and 32.");

            if (bitCount == 0)
                return 0;

            uint result = 0;
            for (int i = 0; i < bitCount; i++)
            {
                result = (result << 1) | ReadBit();
            }

            return result;
        }

        public bool IsEof => _stream.Position >= _stream.Length && _bitsInBuffer == 0;

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
            }

        }

    }
}
