using System;
using System.IO;

namespace PPMdArchiver.Core.Utils
{
    /// <summary>
    /// Bit writer class to safely write bits into stream
    /// </summary>
    internal sealed class BitWriter : IDisposable
    {
        private readonly Stream _stream;
        private byte _buffer = 0;
        private int _bitsInBuffer = 0;
        private bool _disposed = false;

        public BitWriter(Stream stream)
        {
            if (stream == null || !stream.CanWrite)
                throw new ArgumentException("Stream must be writable.");
            _stream = stream;
        }

        public void WriteBit(byte bit)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(BitWriter));

            _buffer = (byte)(_buffer << 1);
            _buffer |= (byte)(bit & 1);
            _bitsInBuffer++;

            if (_bitsInBuffer == 8)
            {
                _stream.WriteByte(_buffer);
                _buffer = 0;
                _bitsInBuffer = 0;
            }
        }

        private void WriteBits(uint bits, int bitCount)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(BitWriter));

            if (bitCount < 0 || bitCount > 32)
                throw new ArgumentOutOfRangeException(nameof(bitCount), "bitCount must be between 0 and 32.");

            if (bitCount == 0)
                return;

            for (int i = bitCount - 1; i >= 0; i--)
            {
                byte bit = (byte)((bits >> i) & 1);

                _buffer = (byte)(_buffer << 1);
                _buffer |= bit;
                _bitsInBuffer++;

                if (_bitsInBuffer == 8)
                {
                    _stream.WriteByte(_buffer);
                    _buffer = 0;
                    _bitsInBuffer = 0;
                }
            }
        }

        public void Flush()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(BitWriter));

            if (_bitsInBuffer > 0)
            {
                _buffer <<= 8 - _bitsInBuffer;
                _stream.WriteByte(_buffer);
                _buffer = 0;
                _bitsInBuffer = 0;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Flush();
                _disposed = true;
            }
        }
    }
}
