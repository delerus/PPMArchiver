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

        public BitWriter(Stream stream)
        {
            _stream = stream;
        }

        public void WriteBit(byte bit)
        {
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

        public void Flush()
        {
            if (_bitsInBuffer > 0)
            {
                _buffer <<= 8 - _bitsInBuffer;
                _stream.WriteByte(_buffer);
                _buffer = 0;
                _bitsInBuffer = 0;
            }
        }

        public void Dispose() => Flush();
    }
}
