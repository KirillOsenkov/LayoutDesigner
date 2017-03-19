using System.Windows.Controls;
using DynamicGeometry;

namespace GuiLabs.LayoutDesigner
{
    public abstract class ContentControlProxy<T> : ControlProxy<T>
        where T : ContentControl
    {
        public ContentControlProxy(T inner)
            : base(inner)
        {
        }

        [PropertyGridVisible]
        public string Content
        {
            get
            {
                return Element.Content.ToString();
            }
            set
            {
                Element.Content = value;
            }
        }
    }
}
