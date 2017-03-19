using System.Windows;
using System.Windows.Controls;
using DynamicGeometry;

namespace GuiLabs.LayoutDesigner
{
    [PropertyGridName("Parent Grid")]
    public class GridItemProxy
    {
        private DragDropGrid grid;
        private FrameworkElement item;

        public GridItemProxy(DragDropGrid grid, FrameworkElement item)
        {
            this.grid = grid;
            this.item = item;
        }

        [PropertyGridVisible]
        [PropertyGridName("Auto-fit grid row height")]
        public bool AutoFitGridRowHeight
        {
            get
            {
                var rowDefinition = GetRowDefinition();
                return rowDefinition.Height.IsAuto;
            }
            set
            {
                var rowDefinition = GetRowDefinition();
                rowDefinition.Height = value ? GridLength.Auto : new GridLength(1, GridUnitType.Star);
                grid.InvalidateMeasure();
            }
        }

        [PropertyGridVisible]
        [PropertyGridName("Auto-fit grid column width")]
        public bool AutoFitGridColumnWidth
        {
            get
            {
                var columnDefinition = GetColumnDefinition();
                return columnDefinition.Width.IsAuto;
            }
            set
            {
                var columnDefinition = GetColumnDefinition();
                columnDefinition.Width = value ? GridLength.Auto : new GridLength(1, GridUnitType.Star);
                grid.InvalidateMeasure();
            }
        }

        private ColumnDefinition GetColumnDefinition()
        {
            int column = Grid.GetColumn(item);
            var columnDefinition = grid.ColumnDefinitions[column];
            return columnDefinition;
        }

        private RowDefinition GetRowDefinition()
        {
            int row = Grid.GetRow(item);
            var rowDefinition = grid.RowDefinitions[row];
            return rowDefinition;
        }
    }
}
