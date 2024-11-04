using System;
using System.Timers;
using MusicPlayerApp.Data;

namespace MusicPlayerApp.LightControl
{
    public class LightController
    {
        private readonly Timer lightEffectTimer;
        private readonly TempoData tempoData;

        public LightController(TempoData tempoData)
        {
            this.tempoData = tempoData;
            lightEffectTimer = new Timer();
            lightEffectTimer.Elapsed += OnLightEffectTriggered;
        }

        /// <summary>
        /// Starts the light effect synchronized with the music tempo.
        /// </summary>
        public void StartLighting()
        {
            if (tempoData.BPM > 0)
            {
                lightEffectTimer.Interval = 60000 / tempoData.BPM; // Sync with BPM
                lightEffectTimer.Start();
            }
        }

        /// <summary>
        /// Stops lighting effects.
        /// </summary>
        public void StopLighting()
        {
            lightEffectTimer.Stop();
        }

        /// <summary>
        /// Handles the lighting effect based on the timer.
        /// </summary>
        private void OnLightEffectTriggered(object sender, ElapsedEventArgs e)
        {
            // Simulate light effect (e.g., pulse, flash)
            Console.WriteLine("Light effect triggered at BPM interval!");
        }
    }
}
