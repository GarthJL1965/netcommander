using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace netCommander
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                System.Threading.Thread.CurrentThread.CurrentCulture = Options.CultureInfo;
                System.Threading.Thread.CurrentThread.CurrentUICulture = Options.CultureInfo;

                main_window = new mainForm();
                Application.Run(main_window);
                //Application.Run(new mainForm());
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex, "Critical exception. Cannot continue.");
            }
        }

        private static Form main_window;
        public static Form MainWindow
        {
            get
            {
                return main_window;
            }
        }
    }
}
