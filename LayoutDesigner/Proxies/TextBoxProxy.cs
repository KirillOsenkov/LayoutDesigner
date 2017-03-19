using System.Windows.Controls;
using DynamicGeometry;

namespace GuiLabs.LayoutDesigner
{
    [PropertyGridName("TextBox")]
    public class TextBoxProxy : ControlProxy<TextBox>
    {
        public TextBoxProxy(TextBox inner)
            : base(inner)
        {
        }

        [PropertyGridVisible]
        public string Text
        {
            get
            {
                return Element.Text;
            }
            set
            {
                Element.Text = value;
            }
        }
    }
}
