using System;
using System.IO;
using System.Windows.Forms;
using MusicPlayerApp.Audio;
using MusicPlayerApp.Logging;
using MusicPlayerApp.Data;
using MusicPlayerApp.LightControl;

namespace MusicPlayerApp.UI
{
    public partial class MainForm : Form
    {
        private MusicPlayer musicPlayer;
        private AudioAnalyzer audioAnalyzer;
        private SpectrogramView spectrogramView;
        private LightController lightController;
        private TempoData tempoData;
        private FolderTreeView folderTreeView;
        private ListBox playlistBox;
        private string currentFolderPath;

        public MainForm()
        {
            InitializeComponent();
            InitializeUIComponents();
            InitializeMusicPlayer();
            InitializeAnalyzerAndLightControl();
            Logger.Initialize();
            Logger.LogInfo("Application started.");

            // Set default window size and position
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new System.Drawing.Size(1200, 800);
        }

        private void InitializeComponent()
        {
            // Initialize main components and layout here.
            playlistBox = new ListBox { Location = new System.Drawing.Point(20, 60), Size = new System.Drawing.Size(300, 400) };
            this.Controls.Add(playlistBox);
        }

        /// <summary>
        /// Initializes all UI components and arranges layout.
        /// </summary>
        private void InitializeUIComponents()
        {
            // Initialize and arrange menu bar at the very top
            MenuStrip menuBar = new MenuStrip();
            ToolStripMenuItem settingsMenu = new ToolStripMenuItem("Settings");
            ToolStripMenuItem loadFolderMenu = new ToolStripMenuItem("Load Folder");
            loadFolderMenu.Click += (sender, args) => LoadFolder();
            settingsMenu.DropDownItems.Add(loadFolderMenu);
            menuBar.Items.Add(settingsMenu);
            this.MainMenuStrip = menuBar;
            this.Controls.Add(menuBar);

            // Folder TreeView setup on the left
            folderTreeView = new FolderTreeView { Dock = DockStyle.Left, Width = 200 };
            folderTreeView.AfterSelect += (sender, e) => LoadFolderContents(e.Node.FullPath);
            this.Controls.Add(folderTreeView);

            // Playlist Box setup on the right
            playlistBox = new ListBox { Dock = DockStyle.Right, Width = 300 };
            playlistBox.SelectedIndexChanged += (sender, args) => SelectSong();
            this.Controls.Add(playlistBox);

            // Playback buttons at bottom center
            FlowLayoutPanel playbackControls = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 50 };
            Button playButton = new Button { Text = "Play" };
            Button pauseButton = new Button { Text = "Pause" };
            Button stopButton = new Button { Text = "Stop" };
            Button nextButton = new Button { Text = "Next" };
            playButton.Click += (sender, args) => StartPlayback();
            pauseButton.Click += (sender, args) => musicPlayer.Pause();
            stopButton.Click += (sender, args) => StopPlayback();
            nextButton.Click += (sender, args) => musicPlayer.PlayNext();
            playbackControls.Controls.AddRange(new Control[] { playButton, pauseButton, stopButton, nextButton });
            this.Controls.Add(playbackControls);

            // Spectrogram view as a retractable window at the bottom
            spectrogramView = new SpectrogramView { Dock = DockStyle.Bottom, Height = 200 };
            spectrogramView.Click += (sender, e) => SeekPositionInSong(e); // Enable click-to-seek functionality
            this.Controls.Add(spectrogramView);
        }

        private void InitializeMusicPlayer()
        {
            musicPlayer = new MusicPlayer();
            tempoData = new TempoData();
            lightController = new LightController(tempoData);
        }

        /// <summary>
        /// Sets up audio analysis, light control, and connects events.
        /// </summary>
        private void InitializeAnalyzerAndLightControl()
        {
            audioAnalyzer = new AudioAnalyzer();
            audioAnalyzer.WaveformGenerated += (waveform) => spectrogramView.UpdateSpectrogram(waveform, null);
            audioAnalyzer.EnergyGenerated += (energy) =>
            {
                tempoData.CalculateBPM(energy, 44100); // Assuming 44.1kHz sample rate
                lightController.StartLighting();       // Starts lighting synchronized to BPM
            };
        }

        /// <summary>
        /// Opens a folder dialog to load music files.
        /// </summary>
        private void LoadFolder()
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    currentFolderPath = dialog.SelectedPath;
                    folderTreeView.LoadRootFolder(currentFolderPath); // Now loads nested folders
                }
            }
        }

        /// <summary>
        /// Loads selected folder contents into playlist.
        /// </summary>
        private void LoadFolderContents(string folderPath)
        {
            playlistBox.Items.Clear();
            foreach (var file in Directory.EnumerateFiles(folderPath, "*.mp3"))
            {
                playlistBox.Items.Add(Path.GetFileName(file));
                musicPlayer.AddTrack(file); // Load the actual file for playback
            }
        }

        /// <summary>
        /// Handles song selection from playlist.
        /// </summary>
        private void SelectSong()
        {
            musicPlayer.SetCurrentTrackIndex(playlistBox.SelectedIndex);
        }

        private void StartPlayback()
        {
            if (playlistBox.SelectedItem != null)
            {
                string selectedTrackPath = Path.Combine(currentFolderPath, playlistBox.SelectedItem.ToString());
                musicPlayer.Play();
                audioAnalyzer.StartAnalysis(selectedTrackPath);
                Logger.LogInfo($"Playing and analyzing: {selectedTrackPath}");
            }
            else
            {
                MessageBox.Show("Please select a track from the playlist.");
            }
        }

        private void StopPlayback()
        {
            musicPlayer.Stop();
            lightController.StopLighting();
            Logger.LogInfo("Playback and effects stopped.");
        }

        /// <summary>
        /// Seek to a new position in the song based on click position in the spectrogram.
        /// </summary>
        private void SeekPositionInSong(EventArgs e)
        {
            // Implementation to calculate and seek position based on click event in spectrogram
        }
    }
}
