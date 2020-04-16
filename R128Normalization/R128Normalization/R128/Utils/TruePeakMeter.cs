using System;

namespace R128.Utils
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
