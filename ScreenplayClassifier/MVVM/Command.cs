using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScreenplayClassifier.MVVM
{
    public class Command : ICommand
    {
        // Properties
        public Action Content { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // Constructors
        public Command(Action action)
        {
            Content = action;
        }

        // Methods
        /// <summary>
        /// Indicates whether the command can be executed.
        /// </summary>
        /// <param name="parameter">The parameter(s) required for the command</param>
        /// <returns>true (the command can always be executed)</returns>
        public bool CanExecute(object parameter) { return true; }
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter(s) required for the command</param>
        public void Execute(object parameter) { Content(); }
    }
}
