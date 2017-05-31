using System;

namespace Funcular.DotNetCore.IdGenerators
{
    public static class ConcurrentRandom
    {
        [ThreadStatic]
        private static Random _random;
        private static readonly object _lock = new object();
        private static long _lastValue;

        static ConcurrentRandom()
        {
        }

        public static long NextLong()
        {
            lock (_lock)
            {
                long value;
                do
                {
                    value = (long)(Random.NextDouble() * MaxRandom);
                } while (value == _lastValue);
                _lastValue = value;
                return value;
            }
        }

        public static Random Random
        {
            get
            {
                if (_random != null) 
                    return _random;
                return _random = new Random();
            }
        }
        public static long MaxRandom { get; set; }
    }
}