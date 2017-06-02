namespace Funcular.DotNetCore.IdGenerators.Enums
{
    /// <summary>
    /// Timestamp granularity; max is <see cref="Day"/>, min is <see cref="Ticks"/>.
    /// </summary>
    public enum TimestampResolution
    {
        /// <summary>
        /// Default, nothing chosen.
        /// </summary>
        None = 0,
        /// <summary>
        /// 24 hours
        /// </summary>
        Day = 4,
        /// <summary>
        /// One hour
        /// </summary>
        Hour = 8,
        /// <summary>
        /// One minute
        /// </summary>
        Minute = 16,
        /// <summary>
        /// One second
        /// </summary>
        Second = 32,
        /// <summary>
        /// One millisecond (1 thousandth seconds)
        /// </summary>
        Millisecond = 64,
        /// <summary>
        /// One microsecond (1 millionth seconds, 1000 microseconds)
        /// </summary>
        Microsecond = 128,
        /// <summary>
        /// One tick (1 ten millionth second)
        /// </summary>
        Ticks = 256
    }
}