using System;
using System.Windows;

namespace GuiLabs.LayoutDesigner
{
    public class DesignerApplication : Application
    {
        public DesignerApplication()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = new Designer();
        }

        private void Application_Exit(object sender, EventArgs e)
        {
            
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
        }
    }
}
