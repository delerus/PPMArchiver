using System;

namespace PPMdArchiver.Core
{
    /// <summary>
    /// Managing interface for the whole archiver elements uniting all its elements
    /// </summary>
    internal interface IArchiveManager
    {
        void Initialize(string inputFile, string outputFile, bool compressMode);
        bool Process();
        string GetErrorMessage();
    }
}
