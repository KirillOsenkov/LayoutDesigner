using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace GuiLabs.LayoutDesigner
{
    public class Root : Grid, IXamlWriter
    {
        public Root()
        {
            this.MinWidth = 400;
            this.MinHeight = 400;

            contentPanel = new ContentPanel()
            {
                Margin = new Thickness(16)
            };
            this.Children.Add(contentPanel);
            contentPanel.Children.Add(new GridControl());
        }

        ContentPanel contentPanel;

        public XElement WriteXaml()
        {
            var result = XamlWriter.WriteCore(contentPanel);
            if (result != null && result.HasElements)
            {
                result = result.Elements().First();
            }
            if (result != null && result.Name == "ContentPanel" && !result.HasElements)
            {
                result = null;
            }
            return result;
        }
    }
}
