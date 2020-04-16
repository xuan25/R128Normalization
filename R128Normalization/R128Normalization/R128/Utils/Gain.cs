using System;

/*
 *    This file is a part of the R128Normalization utils
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
