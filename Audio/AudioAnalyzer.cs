using System;
using NAudio.Dsp;
using NAudio.Wave;
using MusicPlayerApp.Data;

namespace MusicPlayerApp.Audio
{
    public class AudioAnalyzer
    {
        private const int SampleSize = 2048;
        private WaveInEvent waveSource;
        private readonly TempoData tempoData;

        public event Action<double[]> WaveformGenerated;
        public event Action<double[]> EnergyGenerated;

        public AudioAnalyzer()
        {
            tempoData = new TempoData();
        }

        /// <summary>
        /// Analyzes the audio to detect tempo and energy in real-time.
        /// </summary>
        public void StartAnalysis(string audioFile)
        {
            using (AudioFileReader reader = new AudioFileReader(audioFile))
            {
                float[] buffer = new float[SampleSize];
                double[] waveform = new double[SampleSize];
                double[] energy = new double[SampleSize];

                int read;
                while ((read = reader.Read(buffer, 0, SampleSize)) > 0)
                {
                    for (int i = 0; i < read; i++)
                    {
                        waveform[i] = buffer[i];
                        energy[i] = Math.Abs(buffer[i]);
                    }

                    WaveformGenerated?.Invoke(waveform);
                    EnergyGenerated?.Invoke(energy);

                    // Calculate BPM or other metrics here
                    tempoData.CalculateBPM(energy, reader.WaveFormat.SampleRate);
                }
            }
        }
    }
}
