using System;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

namespace MusicPlayerApp.UI
{
    public class SpectrogramView : UserControl
    {
        private PlotView plotView;
        private LineSeries waveformSeries;
        private LineSeries energySeries;

        public SpectrogramView()
        {
            InitializePlotView();
        }

        /// <summary>
        /// Initializes the PlotView for displaying the spectrogram.
        /// </summary>
        private void InitializePlotView()
        {
            plotView = new PlotView
            {
                Dock = DockStyle.Fill
            };
            PlotModel model = new PlotModel { Title = "Spectrogram" };

            waveformSeries = new LineSeries { Title = "Waveform" };
            energySeries = new LineSeries { Title = "Energy" };

            model.Series.Add(waveformSeries);
            model.Series.Add(energySeries);
            plotView.Model = model;

            Controls.Add(plotView);
        }

        /// <summary>
        /// Updates the waveform and energy data for the spectrogram.
        /// </summary>
        /// <param name="waveform">Array of waveform values.</param>
        /// <param name="energy">Array of energy values.</param>
        public void UpdateSpectrogram(double[] waveform, double[] energy)
        {
            waveformSeries.Points.Clear();
            energySeries.Points.Clear();

            for (int i = 0; i < waveform.Length; i++)
            {
                waveformSeries.Points.Add(new DataPoint(i, waveform[i]));
                energySeries.Points.Add(new DataPoint(i, energy[i]));
            }

            plotView.InvalidatePlot(true);
        }
    }
}
