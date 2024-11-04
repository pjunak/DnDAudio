using System;

namespace MusicPlayerApp.Data
{
    public class TempoData
    {
        public double BPM { get; private set; }

        /// <summary>
        /// Calculates the Beats Per Minute (BPM) from energy peaks.
        /// </summary>
        public void CalculateBPM(double[] energy, int sampleRate)
        {
            int peakCount = 0;
            for (int i = 1; i < energy.Length - 1; i++)
            {
                if (energy[i] > energy[i - 1] && energy[i] > energy[i + 1] && energy[i] > 0.1)
                {
                    peakCount++;
                }
            }

            double durationSeconds = (double)energy.Length / sampleRate;
            BPM = (peakCount / durationSeconds) * 60;
        }
    }
}
