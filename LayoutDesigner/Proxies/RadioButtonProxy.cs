using System.Windows.Controls;
using DynamicGeometry;

namespace GuiLabs.LayoutDesigner
{
    [PropertyGridName("RadioButton")]
    public class RadioButtonProxy : ContentControlProxy<RadioButton>
    {
        public RadioButtonProxy(RadioButton inner)
            : base(inner)
        {
        }
    }
}
