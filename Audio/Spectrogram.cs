using System;
using System.Numerics;
using System.Linq;
using NAudio.Dsp;

namespace MusicPlayerApp.Audio
{
    public class Spectrogram
    {
        public event Action<double[]> FrequencyDataGenerated;

        /// <summary>
        /// Processes waveform data and performs FFT to produce frequency data.
        /// </summary>
        public void ProcessSpectrogram(double[] waveform)
        {
            int fftLength = 1024; // Define the FFT length
            NAudio.Dsp.Complex[] fftBuffer = new NAudio.Dsp.Complex[fftLength];

            // Copy waveform data into the FFT buffer
            for (int i = 0; i < fftLength && i < waveform.Length; i++)
            {
                fftBuffer[i] = new NAudio.Dsp.Complex { X = (float)waveform[i], Y = 0 };
            }

            // Perform FFT
            FastFourierTransform.FFT(true, (int)Math.Log(fftLength, 2), fftBuffer);

            // Extract magnitude (frequency) data from FFT results
            double[] frequencyData = new double[fftLength / 2];
            for (int i = 0; i < fftLength / 2; i++)
            {
                frequencyData[i] = Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
            }

            // Raise an event with frequency data
            FrequencyDataGenerated?.Invoke(frequencyData);
        }
    }
}
