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
        bool TryGetSymbolProbability(byte symbol, out SymbolProbability probability);
        bool TryGetSymbolByScaledValue(uint scaledValue, out SymbolProbability probability);
        void Update(byte symbol);
        void SetContext(byte[] recentSymbols);
        void Reset();
        public uint GetTotalFrequency();
    }

    public readonly struct SymbolProbability
    {
        public byte Symbol { get; }
        public uint Frequency { get; }
        public uint CumulativeFrequency { get; }

        public SymbolProbability(byte symbol, uint frequency, uint cumulativeFrequency)
        {
            Symbol = symbol;
            Frequency = frequency;
            CumulativeFrequency = cumulativeFrequency;
        }
    }
}
