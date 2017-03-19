using System.Windows.Controls;
using DynamicGeometry;

namespace GuiLabs.LayoutDesigner
{
    [PropertyGridName("StackPanel")]
    public class StackPanelControlProxy : DragDropControlProxy<StackPanelControl>
    {
        public StackPanelControlProxy(StackPanelControl element)
            : base(element)
        {
        }

        [PropertyGridVisible]
        public Orientation Orientation
        {
            get
            {
                return Element.Orientation;
            }
            set
            {
                Element.Orientation = value;
            }
        }
    }
}
