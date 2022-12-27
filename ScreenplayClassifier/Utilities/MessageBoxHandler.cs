using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScreenplayClassifier.Utilities
{
    class MessageBoxHandler
    {
        // Fields
        private System.Timers.Timer timer;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        // Properties
        public string Caption { get; private set; }

        // Constructors
        public MessageBoxHandler(string text, string caption, byte timeout, MessageBoxImage image)
        {
            Caption = caption;
            if (timeout > 0)
            {
                timer = new System.Timers.Timer(Convert.ToDouble(timeout * 1000)); // interval in seconds
                timer.Elapsed += Timer_Elapsed;

                timer.Start();
                MessageBox.Show(text, caption, MessageBoxButton.OK, image);
            }
        }

        // Methods
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            IntPtr mbWnd = FindWindow("#32770", Caption);

            if (mbWnd != IntPtr.Zero)
                SendMessage(mbWnd, 0x0010, IntPtr.Zero, IntPtr.Zero);
            timer.Stop();
            timer.Dispose();
        }
        /// <summary>
        /// Displays a new Auto-closing MessageBox.
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="caption">The caption of the MessageBox</param>
        /// <param name="timeout">The amount of seconds for which the MessageBox will be displayed</param>
        /// <param name="image">The image to display in the MessageBox</param>
        public static void Show(string text, string caption, byte timeout, MessageBoxImage image)
        {
            new MessageBoxHandler(text, caption, timeout, image);
        }
    }
}
