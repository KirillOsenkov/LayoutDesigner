using System.Windows.Controls;
using DynamicGeometry;

namespace GuiLabs.LayoutDesigner
{
    [PropertyGridName("CheckBox")]
    public class CheckBoxProxy : ContentControlProxy<CheckBox>
    {
        public CheckBoxProxy(CheckBox inner)
            : base(inner)
        {
        }
    }
}
