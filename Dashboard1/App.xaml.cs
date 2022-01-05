using System;
using System.Windows;
using System.Diagnostics;

namespace Dashboard1
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        protected override void OnStartup(StartupEventArgs e)
        {
            Process[] procesos = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (procesos.Length > 1)
            {
                //ya se está ejecutando, no hacemos nada
                MessageBox.Show("Application is already running.");
                Application.Current.Shutdown();
            }
            else
            {
                base.OnStartup(e);
            }
        }

    }
}
