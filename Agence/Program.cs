using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Agence
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// 
        private static string appGuid = "c0h76b5a-12ab-45c5-b9d9-d693faa6e7k9";

        [STAThread]
        static void Main()
        {
            using (System.Threading.Mutex mutex = new System.Threading.Mutex(false, "Global\\" + appGuid))
            {
                if (!mutex.WaitOne(0, false))
                {
                    return;
                }
                try
                {
                    var culture = CultureInfo.GetCultureInfo("fr-FR");
                    CultureInfo.DefaultThreadCurrentCulture = culture;
                    CultureInfo.DefaultThreadCurrentUICulture = culture;
                }
                catch { }
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MasterForm());
            }
        }
    }
}
