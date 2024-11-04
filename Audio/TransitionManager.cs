using System;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;

namespace MusicPlayerApp.Audio
{
    public class TransitionManager
    {
        private const int FadeDurationMs = 2000; // Duration of fade in milliseconds

        /// <summary>
        /// Applies fade-in effect by gradually increasing the volume.
        /// </summary>
        public async Task FadeIn(IWavePlayer player, AudioFileReader reader)
        {
            player.Volume = 0;
            player.Play();
            
            for (int i = 0; i <= 100; i++)
            {
                player.Volume = i / 100f;
                await Task.Delay(FadeDurationMs / 100);
            }
        }

        /// <summary>
        /// Applies fade-out effect by gradually decreasing the volume.
        /// </summary>
        public async Task FadeOut(IWavePlayer player)
        {
            for (int i = 100; i >= 0; i--)
            {
                player.Volume = i / 100f;
                await Task.Delay(FadeDurationMs / 100);
            }
            player.Stop();
        }

        /// <summary>
        /// Applies crossfade between two audio players.
        /// </summary>
        public async Task Crossfade(IWavePlayer player1, IWavePlayer player2, AudioFileReader nextReader)
        {
            player2.Init(nextReader);
            player2.Volume = 0;
            player2.Play();

            for (int i = 0; i <= 100; i++)
            {
                player1.Volume = (100 - i) / 100f;
                player2.Volume = i / 100f;
                await Task.Delay(FadeDurationMs / 100);
            }

            player1.Stop();
        }
    }
}
