using System;

/*
 *    This file is a part of the TruePeakLimiter utils
 *    Copyright (C) 2020  Xuan525
 *
 *    This program is free software: you can redistribute it and/or modify
 *    it under the terms of the GNU General Public License as published by
 *    the Free Software Foundation, either version 3 of the License, or
 *    (at your option) any later version.
 *
 *    This program is distributed in the hope that it will be useful,
 *    but WITHOUT ANY WARRANTY; without even the implied warranty of
 *    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *    GNU General Public License for more details.
 *
 *    You should have received a copy of the GNU General Public License
 *    along with this program.  If not, see <https://www.gnu.org/licenses/>.
 *    
 *    Email : shanboxuan@me.com
 *    Github : https://github.com/xuan525
*/


namespace R128.Limiter
{
    public class TruePeakLimiter
    {
        public double LinearThreashold { get; set; }
        public int AttackSample { get; set; }
        public int ReleaseSample { get; set; }
        public int DelaySample => AttackSample;

        private double CurrentValue { get; set; }
        private double AttackStartValue { get; set; }
        private double AttackEndValue { get; set; }
        private int AttackPosition { get; set; }
        private double ReleaseStartValue { get; set; }
        private int ReleasePosition { get; set; }

        /// <summary>
        /// Constructor of TruePeakLimiter
        /// </summary>
        /// <param name="threasholdDb"></param>
        /// <param name="sampleRate"></param>
        /// <param name="attack"></param>
        /// <param name="release"></param>
        public TruePeakLimiter(double threasholdDb, double sampleRate, double attack, double release)
        {
            LinearThreashold = Math.Pow(10, threasholdDb / 20);
            AttackSample = (int)Math.Round(sampleRate * attack);
            ReleaseSample = (int)Math.Round(sampleRate * release);

            // init attack and release status
            CurrentValue = 1;
            AttackStartValue = -1;
            AttackEndValue = -1;
            AttackPosition = -1;
            ReleaseStartValue = -1;
            ReleasePosition = -1;
        }

        /// <summary>
        /// Process a whole buffer of samples
        /// </summary>
        /// <param name="buffer">The buffer need to be processed</param>
        /// <param name="threshold">Threshold of the limiter in dB</param>
        /// <param name="sampleRate">Sample rate of the samples</param>
        /// <param name="attack">Attack duration of the limiter in seconds</param>
        /// <param name="release">Release duration of the limiter in seconds</param>
        /// <param name="progressUpdated">ProgressUpdated event handler</param>
        /// <param name="envelopUpdated">EnvelopUpdated event handler</param>
        public static void ProcessBuffer(double[][] buffer, double threshold, double sampleRate, double attack, double release, Action<double, double> progressUpdated, Action<double> envelopUpdated)
        {
            TruePeakMeter[] truePeakMeters = new TruePeakMeter[buffer.Length];
            for (int i = 0; i < truePeakMeters.Length; i++)
            {
                truePeakMeters[i] = new TruePeakMeter();
            }
            TruePeakLimiter truePeakLimiter = new TruePeakLimiter(threshold, sampleRate, attack, release);

            int sampleCount = buffer[0].Length;
            for (int sample = 0; sample < sampleCount + truePeakLimiter.DelaySample; sample++)
            {
                progressUpdated?.Invoke(sample, sampleCount);

                // Look ahead
                double peak = 0;
                if (sample < sampleCount)
                {
                    for (int channel = 0; channel < buffer.Length; channel++)
                    {
                        double channelPeak = truePeakMeters[channel].NextTruePeak(buffer[channel][sample]);
                        if (channelPeak > peak)
                        {
                            peak = channelPeak;
                        }
                    }
                }

                // Gain
                double ratio = truePeakLimiter.ProcessNext(peak);
                int ratioSample = sample - truePeakLimiter.DelaySample;
                if (ratioSample >= 0)
                {
                    for (int channel = 0; channel < buffer.Length; channel++)
                    {
                        buffer[channel][ratioSample] *= ratio;
                    }
                }
                envelopUpdated?.Invoke(ratio);
            }
        }

        /// <summary>
        /// Process the next sample
        /// </summary>
        /// <param name="peak">the ture peak of the sample</param>
        /// <returns>The envelope ratio of the sample with a delay of DelaySample</returns>
        public double ProcessNext(double peak)
        {
            if (AttackPosition != -1 && AttackPosition < AttackSample)
            {
                AttackPosition++;
                double p = AttackEaseFunction((double)AttackPosition / AttackSample);
                double value = AttackStartValue - p * (AttackStartValue - AttackEndValue);
                CurrentValue = value;
            }
            else if (ReleasePosition != -1 && ReleasePosition < ReleaseSample)
            {
                ReleasePosition++;
                double p = ReleaseEaseFunction((double)ReleasePosition / ReleaseSample);
                double value = ReleaseStartValue + p * (1 - ReleaseStartValue);
                CurrentValue = value;
            }
            else
            {
                CurrentValue = 1;
            }

            if (peak * CurrentValue > LinearThreashold)
            {
                AttackStartValue = CurrentValue;
                AttackEndValue = LinearThreashold / peak;
                AttackPosition = 0;
                ReleaseStartValue = AttackEndValue;
                ReleasePosition = 0;
            }

            return CurrentValue;
        }

        private double ReleaseEaseFunction(double x)
        {
            return Math.Pow(x, 2);
        }

        private double AttackEaseFunction(double x)
        {
            return 1 - Math.Pow(1 - x, 2);
        }

    }
}
