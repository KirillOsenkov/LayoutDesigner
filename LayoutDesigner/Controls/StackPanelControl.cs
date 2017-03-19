using System.Windows.Controls;
using System.Xml.Linq;

namespace GuiLabs.LayoutDesigner
{
    public class StackPanelControl : DragDropControl, IXamlWriter
    {
        public Orientation Orientation
        {
            get
            {
                return (Panel as DragDropStackPanel).Orientation;
            }
            set
            {
                (Panel as DragDropStackPanel).Orientation = value;
            }
        }

        public override string ToString()
        {
            return "StackPanel";
        }

        public XElement WriteXaml()
        {
            var result = XamlWriter.WriteCore(Panel);
            XamlWriter.AddProperties(this, result);
            result.Name = this.ToString();
            return result;
        }
    }
}
