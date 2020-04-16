namespace R128.Utils
{
    /// <summary>
    /// Porting from : https://github.com/mpavageau/LUFS-TruePeak/blob/master/source/AudioProcessing.cpp
    /// </summary>
    public class OverSampler4x
    {
        private static readonly double[] FilterPhase0 =
        {
            0.0017089843750f, 0.0109863281250f, -0.0196533203125f, 0.0332031250000f,
            -0.0594482421875f, 0.1373291015625f, 0.9721679687500f, -0.1022949218750f,
            0.0476074218750f, -0.0266113281250f, 0.0148925781250f, -0.0083007812500f
        };
        private static readonly double[] FilterPhase1 =
        {
            -0.0291748046875f, 0.0292968750000f, -0.0517578125000f, 0.0891113281250f,
            -0.1665039062500f, 0.4650878906250f, 0.7797851562500f, -0.2003173828125f,
            0.1015625000000f, -0.0582275390625f, 0.0330810546875f, -0.0189208984375f
        };
        private static readonly double[] FilterPhase2 =
        {
            -0.0189208984375f, 0.0330810546875f, -0.0582275390625f, 0.1015625000000f,
            -0.2003173828125f, 0.7797851562500f, 0.4650878906250f, -0.1665039062500f,
            0.0891113281250f, -0.0517578125000f, 0.0292968750000f, -0.0291748046875f
        };
        private static readonly double[] FilterPhase3 =
        {
            -0.0083007812500f, 0.0148925781250f, -0.0266113281250f, 0.0476074218750f,
            -0.1022949218750f, 0.9721679687500f, 0.1373291015625f, -0.0594482421875f,
            0.0332031250000f, -0.0196533203125f, 0.0109863281250f, 0.0017089843750f
        };
        private static readonly double[][] FilterPhases = { FilterPhase0, FilterPhase1, FilterPhase2, FilterPhase3 };
        private static readonly int NumPhases = FilterPhases.Length;
        private static readonly int NumCoeffs = FilterPhase0.Length;

        public double[] PrecedingBuffer { get; private set; }

        /// <summary>
        /// Constructor of the OverSampler4x
        /// </summary>
        public OverSampler4x()
        {
            PrecedingBuffer = new double[NumCoeffs];
        }

        /// <summary>
        /// Oversample the next sample
        /// </summary>
        /// <param name="sample">The sample need to be process</param>
        /// <returns>A array of samples after oversampled</returns>
        public double[] OverSampleNext(double sample)
        {
            // Shift buffer
            for (int i = NumCoeffs - 1; i > 0; i--)
            {
                PrecedingBuffer[i] = PrecedingBuffer[i - 1];
            }
            PrecedingBuffer[0] = sample;

            // Calc
            double[] results = new double[NumPhases];
            for (int phase = 0; phase < NumPhases; phase++)
            {
                double[] filterPhase = FilterPhases[phase];
                double sum = 0;
                for (int coeff = 0; coeff < NumCoeffs; coeff++)
                {
                    sum += PrecedingBuffer[coeff] * filterPhase[coeff];
                }
                results[phase] = sum;
            }
            return results;
        }
    }

}
