using System.Windows;

namespace GuiLabs.LayoutDesigner
{
    public interface IDragSource
    {
        void CompleteDrag();
        void AfterDropComplete();
        UIElement GetElementToDrag(Point cursor);
        void StartDrag(UIElement result);
        UIElement SourceContainer { get; }
        void Select(UIElement element);
    }
}
