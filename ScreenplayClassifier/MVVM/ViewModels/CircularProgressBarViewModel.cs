using System;
using System.Collections.Generic;
using System.Text;

namespace ScreenplayClassifier.MVVM.ViewModels
{
    public class CircularProgressBarViewModel
    {
        // Fields

        // Properties

        // Constructors

        // Methods
        #region Commands
        public Command ProgressCompleteCommand
        {
            get
            {
                return new Command(() =>
                {

                });
            }
        }
        #endregion
    }
}
