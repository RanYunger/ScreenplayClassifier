﻿using ScreenplayClassifier.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScreenplayClassifier.MVVM.Views
{
    /// <summary>
    /// Interaction logic for FeedbackView.xaml
    /// </summary>
    public partial class ClassificationFeedbackView : UserControl
    {
        public ClassificationFeedbackView()
        {
            InitializeComponent();
            DataContext = new ClassificationFeedbackViewModel();
        }
    }
}