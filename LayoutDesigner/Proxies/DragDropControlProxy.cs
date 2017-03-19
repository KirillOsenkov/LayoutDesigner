
namespace GuiLabs.LayoutDesigner
{
    public class DragDropControlProxy<T> : FrameworkElementProxy<T>
        where T : DragDropControl
    {
        public DragDropControlProxy(T element)
            : base(element)
        {
        }

        protected override void UpdateLayout()
        {
            base.UpdateLayout();
            Element.Panel.InvalidateMeasure();
            Element.Panel.InvalidateArrange();
        }
    }
}
