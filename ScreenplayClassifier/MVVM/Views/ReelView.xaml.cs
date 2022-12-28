using ScreenplayClassifier.MVVM.ViewModels;
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
    /// Interaction logic for FolderView.xaml
    /// </summary>
    public partial class ReelView : UserControl
    {
        public ReelView()
        {
            InitializeComponent();
            DataContext = new ReelViewModel();
        }
    }
}
