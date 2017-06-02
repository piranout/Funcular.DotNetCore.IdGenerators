using System;

namespace Funcular.DotNetCore.IdGenerators
{
    /// <summary>
    /// Thread-safe slim version of <see cref="System.Random"/>.
    /// Only produces Int64s.
    /// </summary>
    public static class ConcurrentRandom
    {
        [ThreadStatic]
        private static Random _random;
        private static readonly object _lock = new object();
        private static long _lastValue;

        static ConcurrentRandom()
        {
        }


        /// <summary>
        /// Like <see cref="Random.Next()"/>, but a) returns a 
        /// <see cref="long"/>  instead of an <see cref="int"/>, 
        /// and b) is thread safe.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Access to the underlying (non-thread-safe) <see cref="System.Random"/> instance.
        /// </summary>
        public static Random Random
        {
            get
            {
                if (_random != null) 
                    return _random;
                return _random = new Random();
            }
        }

        /// <summary>
        /// User-set max desired random value.
        /// </summary>
        public static long MaxRandom { get; set; }
    }
}