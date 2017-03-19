using System.Windows;

namespace GuiLabs.LayoutDesigner
{
    public interface IDropLocation
    {
        void Drop(IDragSource dragSource, UIElement draggedElement); 
        void ShowVisualCues();
        void HideVisualCues();
    }
}
