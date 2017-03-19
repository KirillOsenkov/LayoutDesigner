using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace GuiLabs.LayoutDesigner
{
    public class GridControl : DragDropControl, IXamlWriter
    {
        private DragDropGrid grid = new DragDropGrid();

        public GridControl()
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            ContentPanel cell = new ContentPanel();
            grid.Children.Add(cell);
        }

        protected override DragDropPanel CreatePanel()
        {
            return grid;
        }

        public override bool IsSelectionAllowed(UIElement candidateToBeSelected)
        {
            return !(candidateToBeSelected is ContentPanel);
        }

        public override string ToString()
        {
            return "Grid";
        }

        public XElement WriteXaml()
        {
            var result = XamlWriter.WriteCore(Panel);
            XamlWriter.AddProperties(this, result);
            result.Name = this.ToString();
            result.AddFirst(WriteRowDefinitions());
            result.AddFirst(WriteColumnDefinitions());
            return result;
        }

        private XElement WriteColumnDefinitions()
        {
            if (grid.ColumnDefinitions.Count == 0)
            {
                return null;
            }

            if (grid.ColumnDefinitions.Count == 1 && grid.ColumnDefinitions[0].Width.IsStar)
            {
                return null;
            }

            XElement result = new XElement("Grid.ColumnDefinitions");
            foreach (var columnDefinition in grid.ColumnDefinitions)
            {
                var columnXaml = new XElement("ColumnDefinition");
                if (columnDefinition.Width.IsAuto)
                {
                    columnXaml.Add(new XAttribute("Width", "Auto"));
                }
                result.Add(columnXaml);
            }

            return result;
        }

        private XElement WriteRowDefinitions()
        {
            if (grid.RowDefinitions.Count == 0)
            {
                return null;
            }

            if (grid.RowDefinitions.Count == 1 && grid.RowDefinitions[0].Height.IsStar)
            {
                return null;
            }

            XElement result = new XElement("Grid.RowDefinitions");
            foreach (var rowDefinition in grid.RowDefinitions)
            {
                var rowXaml = new XElement("RowDefinition");
                if (rowDefinition.Height.IsAuto)
                {
                    rowXaml.Add(new XAttribute("Height", "Auto"));
                }
                result.Add(rowXaml);
            }

            return result;
        }
    }
}
