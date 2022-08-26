using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.Win32;
using ScreenplayClassifier.MVVM.Models;
using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class ClassificationViewModel : INotifyPropertyChanged
    {
        // Fields
        private ObservableCollection<string> browsedScreenplays;
        private ObservableCollection<ClassificationModel> classifiedScreenplays;
        private int selectedScreenplay;
        private bool canBrowse, canClear, canChoose, canProceed;
        private bool browseComplete, progressComplete, feedbackComplete;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MainViewModel MainViewModel { get; private set; }
        public ClassificationView ClassificationView { get; private set; }
        public ProgressView ProgressView { get; private set; }
        public GenresView PredictedGenresView { get; private set; }
        public GenresView ActualGenresView { get; private set; }

        public ObservableCollection<string> BrowsedScreenplays
        {
            get { return browsedScreenplays; }
            set
            {
                browsedScreenplays = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("BrowsedScreenplays"));
            }
        }

        public ObservableCollection<ClassificationModel> ClassifiedScreenplays
        {
            get { return classifiedScreenplays; }
            set
            {
                classifiedScreenplays = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ClassifiedScreenplays"));
            }
        }

        public int SelectedScreenplay
        {
            get { return selectedScreenplay; }
            set
            {
                selectedScreenplay = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedScreenplay"));
            }
        }

        public bool CanBrowse
        {
            get { return canBrowse; }
            set
            {
                canBrowse = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanBrowse"));
            }
        }

        public bool CanClear
        {
            get { return canClear; }
            set
            {
                canClear = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanClear"));
            }
        }

        public bool CanChoose
        {
            get { return canChoose; }
            set
            {
                canChoose = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanChoose"));
            }
        }

        public bool CanProceed
        {
            get { return canProceed; }
            set
            {
                canProceed = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CanProceed"));
            }
        }

        public bool BrowseComplete
        {
            get { return browseComplete; }
            set
            {
                browseComplete = value;

                if (browseComplete)
                    ((ProgressViewModel)ProgressView.DataContext).SetView(BrowsedScreenplays);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("BrowseComplete"));
            }
        }

        public bool ProgressComplete
        {
            get { return progressComplete; }
            set
            {
                progressComplete = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ProgressComplete"));
            }
        }

        public bool FeedbackComplete
        {
            get { return feedbackComplete; }
            set
            {
                feedbackComplete = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FeedbackComplete"));
            }
        }

        // Constructors
        public ClassificationViewModel()
        {
            BrowsedScreenplays = new ObservableCollection<string>();
            ClassifiedScreenplays = new ObservableCollection<ClassificationModel>();

            SelectedScreenplay = -1;
            CanBrowse = true;
            CanClear = true;
            CanChoose = false;
            CanProceed = false;

            BrowseComplete = false;
            ProgressComplete = false;
            FeedbackComplete = false;
        }

        // Methods
        #region Commands
        public Command CheckKeyCommand
        {
            get
            {
                return new Command(() =>
                {
                    ListView browsedScreenplaysListView = (ListView)ClassificationView.FindName("BrowsedScreenplaysListView");

                    // Validation
                    if (!CanBrowse)
                        return;

                    if (Keyboard.IsKeyDown(Key.Back))
                        for (int i = 0; i < browsedScreenplaysListView.Items.Count; i++)
                            if (browsedScreenplaysListView.SelectedItems.Contains(browsedScreenplaysListView.Items[i]))
                                BrowsedScreenplays.RemoveAt(i);

                    CanChoose = BrowsedScreenplays.Count > 0;
                    CanProceed = BrowsedScreenplays.Count > 0;
                });
            }
        }
        public Command BrowseScreenplaysCommand
        {
            get
            {
                return new Command(() =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    List<ScreenplayModel> alreadyBrowsedScreenplays = new List<ScreenplayModel>();

                    openFileDialog.Title = "Browse screenplays to classify";
                    openFileDialog.DefaultExt = "txt";
                    openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    openFileDialog.Multiselect = true;
                    openFileDialog.InitialDirectory = Environment.CurrentDirectory;
                    openFileDialog.ShowDialog();

                    for (int i = 0; i < openFileDialog.FileNames.Length; i++)
                        BrowsedScreenplays.Add(Path.GetFileNameWithoutExtension(openFileDialog.FileNames[i]));

                    CanChoose = BrowsedScreenplays.Count > 0;
                    CanProceed = BrowsedScreenplays.Count > 0;
                });
            }
        }
        public Command ClearScreenplaysCommand
        {
            get
            {
                return new Command(() =>
                {
                    BrowsedScreenplays.Clear();

                    CanBrowse = true;
                    CanChoose = false;
                    CanProceed = false;
                });
            }
        }
        public Command ProceedToClassificationCommand
        {
            get
            {
                return new Command(() =>
                {
                    CanBrowse = false;
                    CanClear = false;
                    CanChoose = false;
                    CanProceed = false;
                    BrowseComplete = true;
                });
            }
        }

        public Command SubmitFeedbackCommand
        {
            get
            {
                return new Command(() =>
                {
                    MessageBoxResult startOver;

                    if (!CanSubmit())
                        MessageBoxHandler.Show("Complete feedback for all screenplays", "Error", 3, MessageBoxImage.Error);
                    else
                    {
                        //MessageBoxHandler.Show("Feedback submitted successfuly", "Success", 3, MessageBoxImage.Information);

                        startOver = MessageBox.Show("Would you like to start over?", "something",
                            MessageBoxButton.YesNo, MessageBoxImage.Question);
                        FeedbackComplete = startOver == MessageBoxResult.No;
                    }
                });
            }
        }
        #endregion

        public void Init(ClassificationView classificationView, MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            ClassificationView = classificationView;

            ProgressView = (ProgressView)ClassificationView.FindName("ProgressView");
            ((ProgressViewModel)ProgressView.DataContext).Init(this, ProgressView);

            PredictedGenresView = (GenresView)ClassificationView.FindName("PredictedGenresView");
            ActualGenresView = (GenresView)ClassificationView.FindName("ActualGenresView");
        }

        public void SetView()
        {

        }

        public void ResetView()
        {
            BrowsedScreenplays.Clear();
            CanBrowse = true;
            CanClear = true;
            CanChoose = true;
        }

        private bool CanSubmit()
        {
            ScreenplayModel currentScreenplay = null;

            for (int i = 0; i < ClassifiedScreenplays.Count; i++)
            {
                currentScreenplay = ClassifiedScreenplays[i].Screenplay;
                if ((currentScreenplay.ActualGenre == "Unknown") || (currentScreenplay.ActualSubGenre1 == "Unknown")
                    || (currentScreenplay.ActualSubGenre2 == "Unknown"))
                    return false;
            }

            return true;
        }

        public void ClassifyScreenplays(ObservableCollection<string> screenplaysToClassify)
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptSource source = engine.CreateScriptSourceFromFile(FolderPaths.PYTHON + "Setup.py");
            MemoryStream errorsMemoryStream = new MemoryStream(), resultsMemoryStream = new MemoryStream();
            List<string> argv = new List<string>() { string.Empty };
            string classificationResultsStr;

            argv.AddRange(string.Join(" ", screenplaysToClassify).Split(" ", StringSplitOptions.RemoveEmptyEntries));

            engine.GetSysModule().SetVariable("argv", argv);
            engine.Runtime.IO.SetErrorOutput(errorsMemoryStream, Encoding.Default);
            engine.Runtime.IO.SetOutput(resultsMemoryStream, Encoding.Default);

            source.Execute(engine.CreateScope());

            classificationResultsStr = Encoding.Default.GetString(resultsMemoryStream.ToArray());
            ProcessResultsString(classificationResultsStr);
        }

        public void ProcessResultsString(string resultsStr)
        {
            string[] results = resultsStr.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            foreach (string result in results)
                ClassifiedScreenplays.Add(new ClassificationModel(new ScreenplayModel(result)));

            // TODO: COMPLETE (convert each result string into ClassificationModel to add to ClassifiedScreenplays)
        }
    }
}
