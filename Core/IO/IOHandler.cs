using System;
using System.IO;

namespace PPMdArchiver.Core.IO
{
    internal class IOHandler : IIOHandler, IDisposable
    {
        private FileStream _inputStream;
        private FileStream _outputStream;
        private bool _disposed = false;

        public IOHandler()
        {
            _inputStream = null;
            _outputStream = null;
        }

        public void OpenInFile(string inputFile)
        {
            if (string.IsNullOrEmpty(inputFile))
                throw new ArgumentException("Input file path cannot be null or empty.", nameof(inputFile));

            if (_inputStream != null)
                throw new InvalidOperationException("Input file stream is already open.");

            try
            {
                _inputStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (IOException ex)
            {
                throw new IOException($"Failed to open input file: {inputFile}", ex);
            }
        }

        public void OpenOutFile(string outputFile)
        {
            if (string.IsNullOrEmpty(outputFile))
                throw new ArgumentException("Output file path cannot be null or empty.", nameof(outputFile));

            if (_outputStream != null)
                throw new InvalidOperationException("Output file stream is already open.");

            try
            {
                _outputStream = new FileStream(outputFile, FileMode.CreateNew, FileAccess.Write);
            }
            catch (IOException ex)
            {
                throw new IOException($"Failed to create output file: {outputFile}", ex);
            }
        }

        public bool ReadBlock(byte[] buffer, out int bytesRead)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IOHandler));

            if (_inputStream == null)
                throw new InvalidOperationException("Input stream is not open.");

            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            try
            {
                bytesRead = _inputStream.Read(buffer, 0, buffer.Length);
                return bytesRead > 0;
            }
            catch (IOException ex)
            {
                throw new IOException("Failed to read from input stream.", ex);
            }
        }

        public bool WriteBlock(byte[] buffer, int length)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IOHandler));

            if (_outputStream == null)
                throw new InvalidOperationException("Output stream is not open.");

            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            if (length < 0 || length > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(length));

            try
            {
                _outputStream.Write(buffer, 0, length);
                return true;
            }
            catch (IOException ex)
            {
                throw new IOException("Failed to write to output stream.", ex);
            }
        }
        public void Close()
        {
            if (_disposed)
                return;

            _inputStream?.Dispose();
            _outputStream?.Dispose();
            _inputStream = null;
            _outputStream = null;
        }

        public bool IsEof => _inputStream == null || _inputStream.Position >= _inputStream.Length;

        public void Dispose()
        {
            if (!_disposed)
            {
                Close();
                _disposed = true;
            }
        }
    }
}
