using System;

namespace R128.Utils
{
    public static class Gain
    {
        /// <summary>
        /// Apply a gain to the buffer
        /// </summary>
        /// <param name="buffer">The buffer need to be processed</param>
        /// <param name="fromDb">Original level in dB</param>
        /// <param name="toDb">Target level in dB</param>
        public static void ApplyGain(double[][] buffer, double fromDb, double toDb)
        {
            double gain = toDb - fromDb;
            double k = Math.Pow(10, gain / 20);
            for (int channel = 0; channel < buffer.Length; channel++)
            {
                for (int sample = 0; sample < buffer[channel].Length; sample++)
                {
                    buffer[channel][sample] *= k;
                }
            }
        }
    }
}
