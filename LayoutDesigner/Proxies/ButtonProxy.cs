using System.Windows.Controls;
using DynamicGeometry;

namespace GuiLabs.LayoutDesigner
{
    [PropertyGridName("Button")]
    public class ButtonProxy : ContentControlProxy<Button>
    {
        public ButtonProxy(Button inner)
            : base(inner)
        {
        }
    }
}
