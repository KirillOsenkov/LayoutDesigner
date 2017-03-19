using System.Windows;
using System.Windows.Controls;
using DynamicGeometry;

namespace GuiLabs.LayoutDesigner
{
    [PropertyGridName("Toolbox item")]
    public class ToolboxItem : StackPanel
    {
        public ToolboxItem(string imageFileName, string title)
        {
            Title = title;
            this.Orientation = Orientation.Horizontal;
            var image = Visuals.GetImage(imageFileName);
            this.Children.Add(image);
            this.Children.Add(new TextBlock()
            {
                Text = title,
                Margin = new Thickness(14, 0, 0, 0),
                FontSize = 12,
            });
        }

        public string Title { get; set; }
    }
}
