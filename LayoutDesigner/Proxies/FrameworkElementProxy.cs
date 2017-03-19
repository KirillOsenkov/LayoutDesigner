using System.Windows;
using DynamicGeometry;

namespace GuiLabs.LayoutDesigner
{
    public abstract class FrameworkElementProxy<T> 
        where T : FrameworkElement
    {
        public FrameworkElementProxy(T element)
        {
            Element = element;
        }

        protected T Element;

        [PropertyGridVisible]
        public string Name
        {
            get
            {
                return Element.Name;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                Element.Name = value;
            }
        }

        [PropertyGridVisible]
        public HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return Element.HorizontalAlignment;
            }
            set
            {
                Element.HorizontalAlignment = value;
                UpdateLayout();
            }
        }

        protected virtual void UpdateLayout()
        {
            Element.InvalidateMeasure();
            Element.InvalidateArrange();
        }

        [PropertyGridVisible]
        public VerticalAlignment VerticalAlignment
        {
            get
            {
                return Element.VerticalAlignment;
            }
            set
            {
                Element.VerticalAlignment = value;
                UpdateLayout();
            }
        }
    }
}
