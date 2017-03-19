using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GuiLabs.LayoutDesigner
{
    public class DragDropStackPanel : DragDropPanel
    {
        private double Spacing { get; set; }
        private readonly List<Rect> childPositions = new List<Rect>();

        public DragDropStackPanel()
        {
            Spacing = UI.Spacing;
            Background = Brushes.StackPanelBackground;
        }

        private Orientation orientation;
        public Orientation Orientation
        {
            get
            {
                return orientation;
            }
            set
            {
                orientation = value;
                InvalidateMeasure();
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double primaryDimension = 2 * Spacing;
            double secondaryDimension = 0;

            foreach (var child in this.Children)
            {
                child.Measure(availableSize);
                primaryDimension += GetPrimaryDimension(child.DesiredSize) + Spacing;
                secondaryDimension = Math.Max(secondaryDimension, GetSecondaryDimension(child.DesiredSize));
            }

            return CreateSize(primaryDimension - Spacing, secondaryDimension + 2 * Spacing);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            MeasureOverride(finalSize);

            double primaryDimension = Spacing;
            double secondaryDimension = Spacing;
            double finalSizePrimaryDimension = GetPrimaryDimension(finalSize);
            double finalSizeSecondaryDimension = GetSecondaryDimension(finalSize);
            childPositions.Clear();

            foreach (var child in this.Children)
            {
                var primarySize = GetPrimaryDimension(child.DesiredSize);
                var secondarySize = finalSizeSecondaryDimension - 2 * Spacing;
                var childPosition = new Rect(
                    CreatePoint(primaryDimension, secondaryDimension),
                    CreateSize(primarySize, secondarySize));
                childPositions.Add(childPosition);
                child.Arrange(childPosition);
                primaryDimension += primarySize + Spacing;
            }

            return CreateSize(Math.Max(primaryDimension, finalSizePrimaryDimension), finalSizeSecondaryDimension);
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
            StackPanelDropLocation dropTarget = GetDropLocationCore(cursor);

            if (dropTarget != null && dropTarget.IsNearby())
            {
                dropTarget = null;
            }

            return dropTarget;
        }

        private StackPanelDropLocation GetDropLocationCore(Point cursor)
        {
            StackPanelDropLocation dropTarget = null;

            if (DragManager.DraggedElement.IsElementInSubtree(this))
            {
                return null;
            }

            if (Children.Count == 0)
            {
                dropTarget = new StackPanelDropLocation(
                    this,
                    0,
                    PointSituationRelativeToRect.Fill,
                    GetMarkerCoordinates(new Rect(), true));
            }

            int i = 0;
            foreach (FrameworkElement child in Children)
            {
                var classification = ClassifyPoint(cursor, childPositions[i]);
                if (classification == PointSituationRelativeToRect.Before)
                {
                    dropTarget = new StackPanelDropLocation(this, i, classification, GetMarkerCoordinates(childPositions[i], true));
                    return dropTarget;
                }
                if (i == Children.Count - 1 && classification == PointSituationRelativeToRect.After)
                {
                    dropTarget = new StackPanelDropLocation(this, i, classification, GetMarkerCoordinates(childPositions[i], false));
                    return dropTarget;
                }
                i++;
            }
            return dropTarget;
        }

        private Rect GetMarkerCoordinates(Rect rect, bool beforeTheRect)
        {
            var transform = this.TransformToVisual(Designer.Instance.Canvas);
            rect = transform.TransformBounds(rect);
            if (Orientation == Orientation.Vertical)
            {
                if (!beforeTheRect)
                {
                    rect.Y += rect.Height + Spacing;
                }
                rect.Height = 4;
                rect.Y -= (rect.Height + Spacing) / 2;
            }
            else
            {
                if (!beforeTheRect)
                {
                    rect.X += rect.Width + Spacing;
                }
                rect.Width = 4;
                rect.X -= (rect.Width + Spacing) / 2;
            }
            return rect;
        }

        private double GetPrimaryDimension(Size size)
        {
            return Orientation == Orientation.Vertical ? size.Height : size.Width;
        }

        private double GetSecondaryDimension(Size size)
        {
            return Orientation == Orientation.Vertical ? size.Width : size.Height;
        }

        private PointSituationRelativeToRect ClassifyPoint(Point point, Rect rect)
        {
            if (Orientation == Orientation.Vertical)
            {
                if (point.Y > rect.Top + rect.Height / 2.0)
                {
                    return PointSituationRelativeToRect.After;
                }
                else
                {
                    return PointSituationRelativeToRect.Before;
                }
            }
            else
            {
                if (point.X > rect.Left + rect.Width / 2.0)
                {
                    return PointSituationRelativeToRect.After;
                }
                else
                {
                    return PointSituationRelativeToRect.Before;
                }
            }
        }

        private Point CreatePoint(double primaryPosition, double secondaryPosition)
        {
            if (Orientation == Orientation.Vertical)
            {
                return new Point(secondaryPosition, primaryPosition);
            }
            else
            {
                return new Point(primaryPosition, secondaryPosition);
            }
        }

        private Size CreateSize(double primaryDimension, double secondaryDimension)
        {
            if (Orientation == Orientation.Vertical)
            {
                return new Size(secondaryDimension, primaryDimension);
            }
            else
            {
                return new Size(primaryDimension, secondaryDimension);
            }
        }
    }
}
