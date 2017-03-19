using System.Windows.Controls;
using System.Windows.Input;

namespace GuiLabs.LayoutDesigner
{
    public partial class HelpWindow : UserControl
    {
        public HelpWindow()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += HelpWindow_MouseLeftButtonDown;
        }

        void HelpWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var parent = this.Parent as Panel;
            parent.Children.Remove(this);
        }
    }
}
