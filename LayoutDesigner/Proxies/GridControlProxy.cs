using DynamicGeometry;

namespace GuiLabs.LayoutDesigner
{
    [PropertyGridName("Grid")]
    public class GridControlProxy : DragDropControlProxy<GridControl>
    {
        public GridControlProxy(GridControl element)
            : base(element)
        {
        }
    }
}
