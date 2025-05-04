using System;
using System.IO;

namespace PPMdArchiver.Core.IO
{
    internal class IOHandler : IIOHandler
    {
        private FileStream _inputStream;
        private FileStream _outputStream;
        private bool _disposed = false;

        public IOHandler()
        {
            _inputStream = null;
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
            throw new NotImplementedException();
        }

        public bool WriteBlock(byte[] buffer, int length)
        {
            throw new NotImplementedException();
        }
        public void Close()
        {
            throw new NotImplementedException();
        }

        public bool IsEof => _inputStream == null || _inputStream.Position >= _inputStream.Length;
    }
}
