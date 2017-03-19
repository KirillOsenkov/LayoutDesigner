using System.Windows;
using System.Windows.Controls;

namespace GuiLabs.LayoutDesigner
{
    public abstract class DragDropPanel : Panel
    {
        public abstract UIElement HitTest(Point point);

        public abstract IDropLocation GetDropLocation(Point cursor);
    }
}
