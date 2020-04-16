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
    public class TruePeakMeter
    {
        public OverSampler4x OverSampler { get; private set; }

        /// <summary>
        /// Constructor of the TruePeakMeter
        /// </summary>
        public TruePeakMeter()
        {
            OverSampler = new OverSampler4x();
        }

        /// <summary>
        /// Calulate the true peak of the next sample
        /// </summary>
        /// <param name="sample">Linear value of the next sample</param>
        /// <returns>The linear peak value of the next sample</returns>
        public double NextTruePeak(double sample)
        {
            double[] overSampledBuffer = OverSampler.OverSampleNext(sample);
            double maxLinear = double.NegativeInfinity;
            for (int i = 0; i < overSampledBuffer.Length; i++)
            {
                double absoluteValue = Math.Abs(overSampledBuffer[i]);
                if (absoluteValue > maxLinear)
                {
                    maxLinear = absoluteValue;
                }
            }
            return maxLinear;
        }

    }

}
