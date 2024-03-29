﻿using ScreenplayClassifier.MVVM.ViewModels;
using ScreenplayClassifier.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ScreenplayClassifier.Utilities
{
    public static class MessageBox
    {
        // Properties
        public static MessageBoxResult QuestionResult;

        // Methods
        /// <summary>
        /// Shows an error message box.
        /// </summary>
        /// <param name="errorText">The error's text</param>
        public static void ShowError(string errorText)
        {
            MessageBoxView messageBox = new MessageBoxView();

            ((MessageBoxViewModel)messageBox.DataContext).Init(messageBox, "Error", errorText);

            messageBox.Show();
        }

        /// <summary>
        /// Shows an information message box.
        /// </summary>
        /// <param name="informationText">The information's text</param>
        public static void ShowInformation(string informationText)
        {
            MessageBoxView messageBox = new MessageBoxView();

            ((MessageBoxViewModel)messageBox.DataContext).Init(messageBox, "Information", informationText);

            messageBox.Show();
        }

        /// <summary>
        /// Shows a question message box.
        /// </summary>
        /// <param name="questionText">The question's text</param>
        /// <param name="hasWarning">The indication whether the question has a warning</param>
        /// <returns>True if the the message box's question was answered with "Yes", False otherwise</returns>
        public static bool ShowQuestion(string questionText, bool hasWarning)
        {
            MessageBoxView messageBox = new MessageBoxView();

            ((MessageBoxViewModel)messageBox.DataContext).Init(messageBox, hasWarning ? "Warning" : "Question", questionText);

            // Clears the previous question's result and blocks until the new question is answered
            QuestionResult = MessageBoxResult.None;
            messageBox.ShowDialog();

            return QuestionResult == MessageBoxResult.Yes;
        }
    }
}