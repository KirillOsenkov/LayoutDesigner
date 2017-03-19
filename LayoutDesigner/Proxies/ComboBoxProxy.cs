using System.Windows.Controls;
using DynamicGeometry;

namespace GuiLabs.LayoutDesigner
{
    [PropertyGridName("ComboBox")]
    public class ComboBoxProxy : ControlProxy<ComboBox>
    {
        public ComboBoxProxy(ComboBox inner)
            : base(inner)
        {
        }
    }
}
