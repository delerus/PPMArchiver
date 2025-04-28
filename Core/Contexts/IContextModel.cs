using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPMdArchiver.Core.Contexts
{
    internal interface IContextModel
    {
        void Initialize(int order, int memoryMb);
        void Reset();
        // TODO: Define more functions for context model interface
    }

    public readonly struct SymbolProbability
    {
        public byte Symbol { get; }
        public uint Frequency { get; }
        public uint CumulativeFrequency { get; }
    }
}
