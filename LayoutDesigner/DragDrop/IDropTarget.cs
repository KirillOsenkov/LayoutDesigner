using System.Windows;

namespace GuiLabs.LayoutDesigner
{
    public interface IDropTarget
    {
        IDropLocation GetDropLocation(Point cursor);
        UIElement TargetContainer { get; }
    }
}
