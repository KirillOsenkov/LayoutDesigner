using System.Windows.Controls;
using DynamicGeometry;

namespace GuiLabs.LayoutDesigner
{
    [PropertyGridName("TextBlock")]
    public class TextBlockProxy : FrameworkElementProxy<TextBlock>
    {
        public TextBlockProxy(TextBlock inner)
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
