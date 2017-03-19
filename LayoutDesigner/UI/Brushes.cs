using System.Windows.Media;

namespace GuiLabs.LayoutDesigner
{
    public class Brushes
    {
        public static Brush Border = new SolidColorBrush(Colors.LightGray);
        public static Brush ContentBackground = new SolidColorBrush(Color.FromArgb(255, 230, 230, 230));
        public static Brush GridBackground = new SolidColorBrush(Color.FromArgb(255, 235, 235, 235));
        public static Brush StackPanelBackground = new SolidColorBrush(Color.FromArgb(255, 240, 240, 240));
        public static Brush DesignerBackground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        public static Brush SelectionCue = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        public static Brush InsertionPointLine = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
        public static Brush InsertionFill = new SolidColorBrush(Color.FromArgb(255, 220, 230, 255));
    }
}
