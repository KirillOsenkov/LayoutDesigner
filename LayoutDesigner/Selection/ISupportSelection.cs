using System.Windows;

namespace GuiLabs.LayoutDesigner
{
    public interface ISupportSelection
    {
        bool IsSelectionAllowed(UIElement candidateToBeSelected);
        void Select(UIElement element);
    }
}
