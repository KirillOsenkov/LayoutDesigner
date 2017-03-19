using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GuiLabs.LayoutDesigner
{
    public class DragDropGrid : DragDropPanel
    {
        public readonly List<ColumnDefinition> ColumnDefinitions = new List<ColumnDefinition>();
        public readonly List<RowDefinition> RowDefinitions = new List<RowDefinition>();

        private double Spacing { get; set; }
        private double MinCellSize { get; set; }
        private double[] columns;
        private double[] rows;
        private Size[] childSizes;
        private readonly List<Rect> childPositions = new List<Rect>();

        public DragDropGrid()
        {
            Spacing = UI.Spacing;
            MinCellSize = Spacing;
            Background = Brushes.GridBackground;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            childSizes = new Size[this.Children.Count];

            for (int i = 0; i < childSizes.Length; i++)
            {
                this.Children[i].Measure(availableSize);
                childSizes[i] = this.Children[i].DesiredSize;
            }

            columns = new double[ColumnDefinitions.Count];
            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                columns[i] = MinCellSize;
            }

            rows = new double[RowDefinitions.Count];
            for (int i = 0; i < RowDefinitions.Count; i++)
            {
                rows[i] = MinCellSize;
            }

            for (int i = 0; i < childSizes.Length; i++)
            {
                FrameworkElement child = this.Children[i] as FrameworkElement;
                if (child == null)
                {
                    continue;
                }

                int column = Grid.GetColumn(child);
                int columnSpan = Grid.GetColumnSpan(child);

                if (columnSpan <= 1)
                {
                    columns[column] = Math.Max(columns[column], childSizes[i].Width);
                }
                else
                {
                    double width = (childSizes[i].Width - ((columnSpan - 1) * Spacing)) / columnSpan;
                    for (int j = 0; j < columnSpan; j++)
                    {
                        columns[column + j] = Math.Max(columns[column + j], width);
                    }
                }

                int row = Grid.GetRow(child);
                int rowSpan = Grid.GetRowSpan(child);

                if (rowSpan <= 1)
                {
                    rows[row] = Math.Max(rows[row], childSizes[i].Height);
                }
                else
                {
                    double height = (childSizes[i].Height - ((rowSpan - 1) * Spacing)) / rowSpan;
                    for (int j = 0; j < rowSpan; j++)
                    {
                        rows[row + j] = Math.Max(rows[row + j], height);
                    }
                }
            }

            var totalWidth = columns.Sum() + (columns.Length + 1) * Spacing;
            var totalHeight = rows.Sum() + (rows.Length + 1) * Spacing;

            return new Size(totalWidth, totalHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            MeasureOverride(finalSize);

            DistributeAvailableHorizontalSpace(finalSize);
            DistributeAvailableVerticalSpace(finalSize);

            double[] cellLeft = new double[columns.Length];
            double[] cellTop = new double[rows.Length];

            double totalWidth = Spacing;
            for (int i = 0; i < columns.Length; i++)
            {
                cellLeft[i] = totalWidth;
                totalWidth += columns[i] + Spacing;
            }

            double totalHeight = Spacing;
            for (int i = 0; i < rows.Length; i++)
            {
                cellTop[i] = totalHeight;
                totalHeight += rows[i] + Spacing;
            }

            childPositions.Clear();

            for (int i = 0; i < this.Children.Count; i++)
            {
                FrameworkElement child = this.Children[i] as FrameworkElement;
                if (child == null)
                {
                    continue;
                }

                int column = Grid.GetColumn(child);
                int columnSpan = Grid.GetColumnSpan(child);
                int row = Grid.GetRow(child);
                int rowSpan = Grid.GetRowSpan(child);

                var childPosition = new Rect();
                childPosition.X = cellLeft[column];
                childPosition.Y = cellTop[row];
                childPosition.Width = GetAvailableWidth(column, columnSpan);
                childPosition.Height = GetAvailableHeight(row, rowSpan);
                childPositions.Add(childPosition);
                child.Arrange(childPosition);
            }

            return new Size(totalWidth, totalHeight);
        }

        private double GetAvailableWidth(int column, int columnSpan)
        {
            double result = 0;
            for (int i = 0; i < columnSpan; i++)
            {
                result += columns[column + i];
            }
            result += (columnSpan - 1) * Spacing;
            return result;
        }

        private double GetAvailableHeight(int row, int rowSpan)
        {
            double result = 0;
            for (int i = 0; i < rowSpan; i++)
            {
                result += rows[row + i];
            }
            result += (rowSpan - 1) * Spacing;
            return result;
        }

        private void DistributeAvailableHorizontalSpace(Size finalSize)
        {
            double extraWidth = finalSize.Width - DesiredSize.Width;
            if (extraWidth <= 0)
            {
                return;
            }

            double totalPie = 0;
            foreach (var columnDefinition in ColumnDefinitions)
            {
                if (columnDefinition.Width.IsStar)
                {
                    totalPie += columnDefinition.Width.Value;
                }
            }

            if (totalPie == 0)
            {
                return;
            }

            for (int i = 0; i < columns.Length; i++)
            {
                var columnDefinition = ColumnDefinitions[i];
                if (columnDefinition.Width.IsStar)
                {
                    columns[i] += extraWidth / totalPie * columnDefinition.Width.Value;
                }
            }
        }

        private void DistributeAvailableVerticalSpace(Size finalSize)
        {
            double extraHeight = finalSize.Height - DesiredSize.Height;
            if (extraHeight <= 0)
            {
                return;
            }

            double totalPie = 0;
            foreach (var rowDefinition in RowDefinitions)
            {
                if (rowDefinition.Height.IsStar)
                {
                    totalPie += rowDefinition.Height.Value;
                }
            }

            if (totalPie == 0)
            {
                return;
            }

            for (int i = 0; i < rows.Length; i++)
            {
                var rowDefinition = RowDefinitions[i];
                if (rowDefinition.Height.IsStar)
                {
                    rows[i] += extraHeight / totalPie * rowDefinition.Height.Value;
                }
            }
        }

        public override UIElement HitTest(Point point)
        {
            int i = 0;
            foreach (FrameworkElement child in Children)
            {
                if (childPositions[i].Contains(point))
                {
                    return child;
                }
                i++;
            }
            return null;
        }

        public override IDropLocation GetDropLocation(Point cursor)
        {
            var dropTarget = GetDropLocationCore(cursor);
            return dropTarget;
        }

        private IDropLocation GetDropLocationCore(Point cursor)
        {
            IDropLocation dropTarget = null;

            if (DragManager.DraggedElement.IsElementInSubtree(this))
            {
                return null;
            }

            if (this.ColumnDefinitions.Count == 1
                && this.RowDefinitions.Count == 1
                && this.Children.Count == 1
                && this.Children[0] is ContentPanel
                && (this.Children[0] as ContentPanel).Children.Count == 0)
            {
                return null;
            }

            Tuple<int, PointSituationRelativeToRect, double> columnInfo = GetColumn(cursor);
            Tuple<int, PointSituationRelativeToRect, double> rowInfo = GetRow(cursor);

            if (rowInfo.Item2 == PointSituationRelativeToRect.Fill
                && (columnInfo.Item2 == PointSituationRelativeToRect.Before || columnInfo.Item2 == PointSituationRelativeToRect.After))
            {
                Rect rect = new Rect();
                rect.X = columnInfo.Item3;
                rect.Y = rowInfo.Item3;
                rect.Width = Spacing;
                rect.Height = rows[rowInfo.Item1];
                rect = GetMarkerCoordinates(rect);
                dropTarget = new InsertColumnGridDropLocation(
                    this,
                    columnInfo.Item1,
                    rowInfo.Item1,
                    columnInfo.Item2 == PointSituationRelativeToRect.After,
                    rect);
                return dropTarget;
            }

            if (columnInfo.Item2 == PointSituationRelativeToRect.Fill
                && (rowInfo.Item2 == PointSituationRelativeToRect.Before || rowInfo.Item2 == PointSituationRelativeToRect.After))
            {
                Rect rect = new Rect();
                rect.X = columnInfo.Item3;
                rect.Y = rowInfo.Item3;
                rect.Width = columns[columnInfo.Item1];
                rect.Height = Spacing;
                rect = GetMarkerCoordinates(rect);
                dropTarget = new InsertRowGridDropLocation(
                    this,
                    columnInfo.Item1,
                    rowInfo.Item1,
                    rowInfo.Item2 == PointSituationRelativeToRect.After,
                    rect);
                return dropTarget;
            }

            return dropTarget;
        }

        private Tuple<int, PointSituationRelativeToRect, double> GetColumn(Point cursor)
        {
            double total = Spacing;

            for (int i = 0; i < columns.Length; i++)
            {
                if (cursor.X < total)
                {
                    return Tuple.Create(i, PointSituationRelativeToRect.Before, total - Spacing);
                }
                total += columns[i];
                if (cursor.X < total)
                {
                    return Tuple.Create(i, PointSituationRelativeToRect.Fill, total - columns[i]);
                }
                total += Spacing;
            }

            return Tuple.Create(columns.Length - 1, PointSituationRelativeToRect.After, total - Spacing);
        }

        private Tuple<int, PointSituationRelativeToRect, double> GetRow(Point cursor)
        {
            double total = Spacing;

            for (int i = 0; i < rows.Length; i++)
            {
                if (cursor.Y < total)
                {
                    return Tuple.Create(i, PointSituationRelativeToRect.Before, total - Spacing);
                }
                total += rows[i];
                if (cursor.Y < total)
                {
                    return Tuple.Create(i, PointSituationRelativeToRect.Fill, total - rows[i]);
                }
                total += Spacing;
            }

            return Tuple.Create(rows.Length - 1, PointSituationRelativeToRect.After, total - Spacing);
        }

        private Rect GetMarkerCoordinates(Rect rect)
        {
            var transform = this.TransformToVisual(Designer.Instance.Canvas);
            rect = transform.TransformBounds(rect);
            return rect;
        }

        public ContentPanel FindContentPanel(int column, int row)
        {
            foreach (FrameworkElement child in this.Children)
            {
                int childColumn = Grid.GetColumn(child);
                int childRow = Grid.GetRow(child);

                if (column == childColumn && row == childRow)
                {
                    var contentPanel = child as ContentPanel;
                    return contentPanel;
                }
            }

            return null;
        }

        public int FindEmptyRow()
        {
            bool[,] matrix = GetOccupancyMatrixExcludingEmptyContentPanels();

            for (int j = this.RowDefinitions.Count - 1; j >= 0; j--)
            {
                bool allZeros = true;
                for (int i = 0; i < this.ColumnDefinitions.Count; i++)
                {
                    if (matrix[i, j])
                    {
                        allZeros = false;
                    }
                }
                if (allZeros)
                {
                    return j;
                }
            }

            return -1;
        }

        public int FindEmptyColumn()
        {
            bool[,] matrix = GetOccupancyMatrixExcludingEmptyContentPanels();

            for (int i = this.ColumnDefinitions.Count - 1; i >= 0; i--)
            {
                bool allZeros = true;
                for (int j = 0; j < this.RowDefinitions.Count; j++)
                {
                    if (matrix[i, j])
                    {
                        allZeros = false;
                    }
                }
                if (allZeros)
                {
                    return i;
                }
            }
            return -1;
        }

        public void InsertColumnDefinitionCore(ColumnDefinition columnDefinition, int columnIndex)
        {
            foreach (FrameworkElement child in this.Children)
            {
                int childColumn = Grid.GetColumn(child);
                if (childColumn >= columnIndex)
                {
                    Grid.SetColumn(child, childColumn + 1);
                }
            }

            this.ColumnDefinitions.Insert(columnIndex, columnDefinition);

            for (int i = 0; i < this.RowDefinitions.Count; i++)
            {
                ContentPanel cell = new ContentPanel();
                Grid.SetColumn(cell, columnIndex);
                Grid.SetRow(cell, i);
                this.Children.Add(cell);
            }
        }

        public void RemoveColumnDefinitionCore(int columnIndex)
        {
            this.ColumnDefinitions.RemoveAt(columnIndex);

            foreach (FrameworkElement child in this.Children.ToArray())
            {
                int childColumn = Grid.GetColumn(child);
                if (childColumn == columnIndex)
                {
                    this.Children.Remove(child);
                }
                if (childColumn > columnIndex)
                {
                    Grid.SetColumn(child, childColumn - 1);
                }
            }
        }

        public void InsertRowDefinitionCore(RowDefinition rowDefinition, int rowIndex)
        {
            foreach (FrameworkElement child in this.Children)
            {
                int childRow = Grid.GetRow(child);
                if (childRow >= rowIndex)
                {
                    Grid.SetRow(child, childRow + 1);
                }
            }

            this.RowDefinitions.Insert(rowIndex, rowDefinition);

            for (int i = 0; i < this.ColumnDefinitions.Count; i++)
            {
                ContentPanel cell = new ContentPanel();
                Grid.SetColumn(cell, i);
                Grid.SetRow(cell, rowIndex);
                this.Children.Add(cell);
            }
        }

        public void RemoveRowDefinitionCore(int rowIndex)
        {
            this.RowDefinitions.RemoveAt(rowIndex);

            foreach (FrameworkElement child in this.Children.ToArray())
            {
                int childRow = Grid.GetRow(child);
                if (childRow == rowIndex)
                {
                    this.Children.Remove(child);
                }
                if (childRow > rowIndex)
                {
                    Grid.SetRow(child, childRow - 1);
                }
            }
        }

        private bool[,] GetOccupancyMatrixExcludingEmptyContentPanels()
        {
            bool[,] matrix = new bool[this.ColumnDefinitions.Count, this.RowDefinitions.Count];

            foreach (FrameworkElement child in this.Children)
            {
                ContentPanel contentPanel = child as ContentPanel;
                if (contentPanel != null && contentPanel.Children.Count == 0)
                {
                    continue;
                }
                int column = Grid.GetColumn(child);
                int row = Grid.GetRow(child);
                matrix[column, row] = true;
            }
            return matrix;
        }

        private bool[,] GetOccupancyMatrix()
        {
            bool[,] matrix = new bool[this.ColumnDefinitions.Count, this.RowDefinitions.Count];

            foreach (FrameworkElement child in this.Children)
            {
                int column = Grid.GetColumn(child);
                int row = Grid.GetRow(child);
                matrix[column, row] = true;
            }
            return matrix;
        }
    }
}
