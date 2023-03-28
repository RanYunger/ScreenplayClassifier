using ScreenplayClassifier.MVVM.Views;
using ScreenplayClassifier.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class MessageBoxViewModel : INotifyPropertyChanged
    {
        // Fields
        private string messageType, messageText;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties
        public MessageBoxView MessageBoxView { get; private set; }

        public string MessageType
        {
            get { return messageType; }
            set
            {
                messageType = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("MessageType"));
            }
        }

        public string MessageText
        {
            get { return messageText; }
            set
            {
                messageText = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("MessageText"));
            }
        }

        // Constructors
        public MessageBoxViewModel() { }

        // Methods
        #region Commands
        public Command ChooseYesCommand
        {
            get
            {
                return new Command(() =>
                {
                    MessageBox.QuestionResult = System.Windows.MessageBoxResult.Yes;
                    CloseCommand.Execute(null);
                });
            }
        }

        public Command CloseCommand
        {
            get { return new Command(() => MessageBoxView.Close()); }
        }

        public Command ChooseNoCommand
        {
            get
            {
                return new Command(() =>
                {
                    MessageBox.QuestionResult = System.Windows.MessageBoxResult.No;
                    CloseCommand.Execute(null);
                });
            }
        }
        #endregion

        /// <summary>
        /// Initiates the view model.
        /// </summary>
        /// <param name="messageBoxView">The view to obtain controls from</param>
        /// <param name="type">The message box's type</param>
        /// <param name="text">The message box's text</param>
        public void Init(MessageBoxView messageBoxView, string type, string text)
        {
            MessageBoxView = messageBoxView;
            MessageType = type;
            MessageText = text;
        }
    }
}