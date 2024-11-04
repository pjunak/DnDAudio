using System;
using System.IO;
using System.Collections.Generic;
using NAudio.Wave;
using MusicPlayerApp.Logging;


namespace MusicPlayerApp.Audio
{
    public class MusicPlayer
    {
        private IWavePlayer waveOutDevice;
        private AudioFileReader audioFileReader;
        private List<string> playlist;
        private int currentTrackIndex;
        private bool isPaused;

        public MusicPlayer()
        {
            playlist = new List<string>();
            currentTrackIndex = -1;
        }

        /// <summary>
        /// Loads audio files from a directory into the playlist.
        /// </summary>
        /// <param name="directoryPath">Path to the directory containing audio files.</param>
        public void LoadPlaylist(string directoryPath)
        {
            playlist.Clear();
            currentTrackIndex = 0;

            foreach (var file in Directory.EnumerateFiles(directoryPath, "*.mp3"))
            {
                playlist.Add(file);
            }

            Logger.LogInfo($"Loaded {playlist.Count} tracks from {directoryPath}");
        }

        /// <summary>
        /// Plays the current track in the playlist.
        /// </summary>
        public void Play()
        {
            if (currentTrackIndex >= 0 && currentTrackIndex < playlist.Count)
            {
                string trackPath = playlist[currentTrackIndex];
                if (waveOutDevice == null)
                {
                    waveOutDevice = new WaveOutEvent();
                    waveOutDevice.PlaybackStopped += OnPlaybackStopped;
                }

                if (audioFileReader != null)
                {
                    audioFileReader.Dispose();
                }

                audioFileReader = new AudioFileReader(trackPath);
                waveOutDevice.Init(audioFileReader);
                waveOutDevice.Play();
                isPaused = false;

                Logger.LogInfo($"Playing: {trackPath}");
            }
            else
            {
                Logger.LogError("Playlist is empty. Load tracks before playing.");
            }
        }

        /// <summary>
        /// Pauses the current track if it is playing.
        /// </summary>
        public void Pause()
        {
            if (waveOutDevice != null && !isPaused)
            {
                waveOutDevice.Pause();
                isPaused = true;
                Logger.LogInfo("Playback paused.");
            }
        }

        /// <summary>
        /// Resumes playback if paused.
        /// </summary>
        public void Resume()
        {
            if (waveOutDevice != null && isPaused)
            {
                waveOutDevice.Play();
                isPaused = false;
                Logger.LogInfo("Playback resumed.");
            }
        }

        /// <summary>
        /// Stops playback and releases audio resources.
        /// </summary>
        public void Stop()
        {
            if (waveOutDevice != null)
            {
                waveOutDevice.Stop();
            }
            DisposeAudioResources();
            Logger.LogInfo("Playback stopped.");
        }

        /// <summary>
        /// Moves to the next track in the playlist and plays it.
        /// </summary>
        public void PlayNext()
        {
            if (playlist.Count == 0)
            {
                Logger.LogError("Playlist is empty.");
                return;
            }

            currentTrackIndex = (currentTrackIndex + 1) % playlist.Count;
            Play();
        }

        /// <summary>
        /// Randomizes the order of tracks in the playlist.
        /// </summary>
        public void Shuffle()
        {
            Random random = new Random();
            for (int i = playlist.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                string temp = playlist[i];
                playlist[i] = playlist[j];
                playlist[j] = temp;
            }

            Logger.LogInfo("Playlist shuffled.");
        }

        /// <summary>
        /// Handles playback completion to move to the next track.
        /// </summary>
        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (!isPaused)
            {
                PlayNext();
            }
        }

        /// <summary>
        /// Disposes audio resources to release memory.
        /// </summary>
        private void DisposeAudioResources()
        {
            if (audioFileReader != null)
            {
                audioFileReader.Dispose();
                audioFileReader = null;
            }

            if (waveOutDevice != null)
            {
                waveOutDevice.Dispose();
                waveOutDevice = null;
            }
        }

        public void SetCurrentTrackIndex(int index)
        {
            if (index >= 0 && index < playlist.Count)
            {
                currentTrackIndex = index;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Track index is out of range.");
            }
        }

        public void AddTrack(string trackPath)
        {
            playlist.Add(trackPath);
        }
    }
}
