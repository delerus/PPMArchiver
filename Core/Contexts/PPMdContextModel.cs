using System;
using System.Collections.Generic;
using System.Linq;

namespace PPMdArchiver.Core.Contexts
{
    internal class PPMdContextModel : IContextModel
    {
        private int _order;

        private readonly Dictionary<string, Dictionary<byte, uint>> _contextFrequencies;
        private readonly Dictionary<string, List<SymbolProbability>> _contextProbabilities;
        private readonly Dictionary<string, uint> _contextTotals;

        private string _currentContext;
        private const byte EscapeSymbol = 255;

        public PPMdContextModel()
        {
            _contextFrequencies = new Dictionary<string, Dictionary<byte, uint>>();
            _contextProbabilities = new Dictionary<string, List<SymbolProbability>>();
            _contextTotals = new Dictionary<string, uint>();
            _currentContext = "";
        }

        public void Initialize(int order, int memoryMb)
        {
            _order = order;
            Reset();
        }

        private void RebuildOrderedProbabilities(string context)
        {
            var probabilities = new List<SymbolProbability>();
            if (!_contextFrequencies.TryGetValue(context, out var frequencies))
            {
                frequencies = new Dictionary<byte, uint>();
                _contextFrequencies[context] = frequencies;
            }

            uint currentCumulativeFrequency = 0;
            foreach (var entry in frequencies.OrderBy(pair => pair.Key))
            {
                probabilities.Add(new SymbolProbability(entry.Key, entry.Value, currentCumulativeFrequency));
                currentCumulativeFrequency += entry.Value;
            }

            probabilities.Add(new SymbolProbability(EscapeSymbol, 1, currentCumulativeFrequency));
            currentCumulativeFrequency += 1;

            _contextProbabilities[context] = probabilities;
            _contextTotals[context] = currentCumulativeFrequency;
        }

        public bool TryGetSymbolProbability(byte symbol, out SymbolProbability probability)
        {
            if (!_contextProbabilities.TryGetValue(_currentContext, out var list))
            {
                probability = default;
                return false;
            }

            foreach (var p in list)
            {
                if (p.Symbol == symbol)
                {
                    probability = p;
                    return true;
                }
            }

            probability = default;
            return false;
        }

        public bool TryGetSymbolByScaledValue(uint scaledValue, out SymbolProbability probability)
        {
            if (!_contextTotals.TryGetValue(_currentContext, out uint total) || total == 0)
            {
                probability = default;
                return false;
            }

            foreach (var p in _contextProbabilities[_currentContext])
            {
                if (scaledValue >= p.CumulativeFrequency && scaledValue < p.CumulativeFrequency + p.Frequency)
                {
                    probability = p;
                    return true;
                }
            }

            probability = default;
            return false;
        }

        public void Update(byte symbol)
        {
            if (!_contextFrequencies.TryGetValue(_currentContext, out var frequencies))
            {
                frequencies = new Dictionary<byte, uint>();
                _contextFrequencies[_currentContext] = frequencies;
            }

            if (!frequencies.ContainsKey(symbol))
                frequencies[symbol] = 1;
            else
                frequencies[symbol]++;

            RebuildOrderedProbabilities(_currentContext);
        }


        public void SetContext(byte[] recentSymbols)
        {
            int length = Math.Min(recentSymbols.Length, _order);
            _currentContext = length > 0 ? System.Text.Encoding.ASCII.GetString(recentSymbols[^length..]) : "";

            if (!_contextFrequencies.ContainsKey(_currentContext))
            {
                _contextFrequencies[_currentContext] = new Dictionary<byte, uint>();
            }
            RebuildOrderedProbabilities(_currentContext);
        }

        public void Reset()
        {
            _contextFrequencies.Clear();
            _contextProbabilities.Clear();
            _contextTotals.Clear();
            _currentContext = "";
            _contextFrequencies[_currentContext] = new Dictionary<byte, uint>();
            RebuildOrderedProbabilities(_currentContext);
        }

        public uint GetTotalFrequency()
        {
            if (_contextTotals.TryGetValue(_currentContext, out uint total))
                return total;

            return 0;
        }

    }
}
