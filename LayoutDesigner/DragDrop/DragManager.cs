using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GuiLabs.LayoutDesigner
{
    public class DragManager
    {
        public static UIElement DragCandidate { get; set; }
        public static IDragSource DragCandidateSponsor { get; set; }
        public static IDragSource DragSource { get; set; }
        public static UIElement DraggedElement { get; set; }
        public static IDropLocation DropTargetInfo { get; set; }

        private static Image Ghost;

        public static void StartDrag(IDragSource source, UIElement dragged)
        {
            DragSource = source;
            DraggedElement = dragged;
            CreateDraggedGhostImage(dragged);
        }

        private static void CreateDraggedGhostImage(UIElement dragged)
        {
            Ghost = new Image();
            Ghost.IsHitTestVisible = false;
            Ghost.Stretch = Stretch.None;
            WriteableBitmap wb = new WriteableBitmap(dragged, null);
            Ghost.Source = wb;
            Ghost.CacheMode = new BitmapCache();
            Designer.Instance.Canvas.Children.Add(Ghost);
            Ghost.Opacity = 0.4;
        }

        public static void Drop()
        {
            if (DraggedElement != null && DropTargetInfo != null)
            {
                using (Actions.Transaction())
                {
                    DropTargetInfo.Drop(DragSource, DraggedElement);
                }
            }
            UpdateDropTarget(null);
            if (Ghost != null)
            {
                Designer.Instance.Canvas.Children.Remove(Ghost);
                Ghost = null;
            }
            if (DraggedElement != null)
            {
                DraggedElement = null;
            }
            ResetDragCandidate();
        }

        public static void MouseMove(Point point)
        {
            if (Ghost != null)
            {
                Canvas.SetLeft(Ghost, point.X + 32);
                Canvas.SetTop(Ghost, point.Y + 32);
            }
        }

        public static void UpdateDropTarget(IDropLocation dropTarget)
        {
            if (DropTargetInfo != null)
            {
                DropTargetInfo.HideVisualCues();
            }
            DropTargetInfo = dropTarget;
            if (DropTargetInfo != null)
            {
                DropTargetInfo.ShowVisualCues();
            }
        }

        public static bool IsDragging
        {
            get
            {
                return DraggedElement != null;
            }
        }

        public static void ResetDragCandidate()
        {
            DragCandidate = null;
            DragCandidateSponsor = null;
        }
    }
}
