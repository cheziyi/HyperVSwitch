using System;
using System.Windows.Forms;

namespace HyperVSwitch
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var pi = new ProcessIcon())
            {
                pi.Display();
                Application.Run();
            }
        }
    }
}
