using System;

namespace PPMdArchiver.Core.IO
{
    /// <summary>
    /// General IO interface for working with files and streams
    /// </summary>
    internal interface IIOHandler
    {
        void OpenInFile(string inputFile);
        void OpenOutFile(string outputFile);
        bool ReadBlock(byte[] buffer, out int bytesRead);
        bool WriteBlock(byte[] buffer, int length);
        void Close();
        bool IsEof { get; }
    }
}
