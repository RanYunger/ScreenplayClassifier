using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace ScreenplayClassifier.MVVM.Models
{
    public class ProgressModel : INotifyPropertyChanged
    {
        // Fields
        private BackgroundWorker backgroundWorker;
        private System.Timers.Timer durationTimer;
        private TimeSpan duration;
        private int percentage;
        private string description;
        private bool isPaused, isComplete;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public BackgroundWorker BackgroundWorker
        {
            get { return backgroundWorker; }
            set
            {
                backgroundWorker = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("BackgroundWorker"));
            }
        }

        public System.Timers.Timer DurationTimer
        {
            get { return durationTimer; }
            set
            {
                durationTimer = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DurationTimer"));
            }
        }

        public TimeSpan Duration
        {
            get { return duration; }
            set
            {
                duration = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Duration"));
            }
        }

        public int Percentage
        {
            get { return percentage; }
            set
            {
                percentage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Percentage"));
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Description"));
            }
        }
        public bool IsPaused
        {
            get { return isPaused; }
            set
            {
                isPaused = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsPaused"));
            }
        }

        public bool IsComplete
        {
            get { return isComplete; }
            set
            {
                isComplete = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsComplete"));
            }
        }

        // Constructors
        public ProgressModel()
        {
            DurationTimer = new System.Timers.Timer(1000);
            DurationTimer.Elapsed += DurationTimer_Elapsed;

            BackgroundWorker = new BackgroundWorker();
            BackgroundWorker.WorkerReportsProgress = BackgroundWorker.WorkerSupportsCancellation = true;
            BackgroundWorker.DoWork += BackgroundWorker_DoWork;
            BackgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            BackgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;

            Percentage = 0;
            Description = "Reading screenplay...";
            IsPaused = false;
        }

        // Methods
        private void DurationTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Duration = Duration.Add(new TimeSpan(0, 0, 1));
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsComplete = true;
            durationTimer.Stop();

            if (IsPaused)
                Description = "Paused!";
            else
                Description = e.Error != null ? "Error: " + e.Error.Message : "Complete!";
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Percentage = e.ProgressPercentage;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            for (int i = 1; i <= 10; i++)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    // Perform a time consuming operation and report progress.
                    Thread.Sleep(500);
                    worker.ReportProgress(i * 10);
                }
            }
        }
    }
}
