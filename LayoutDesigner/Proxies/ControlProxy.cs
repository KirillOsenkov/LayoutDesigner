using System.Windows.Controls;

namespace GuiLabs.LayoutDesigner
{
    public abstract class ControlProxy<T> : FrameworkElementProxy<T>
        where T : Control
    {
        public ControlProxy(T element)
            : base(element)
        {
        }
    }
}
