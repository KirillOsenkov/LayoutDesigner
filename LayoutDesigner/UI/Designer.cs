using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DynamicGeometry;
using GuiLabs.Undo;

namespace GuiLabs.LayoutDesigner
{
    public class Designer : Grid
    {
        public static Designer Instance;

        public PropertyGrid GridPropertyGrid { get; set; }
        public Canvas Canvas { get; set; }
        public ActionManager ActionManager { get; set; }
        public RecycleBin RecycleBin { get; set; }

        private PropertyGrid PropertyGrid { get; set; }
        private XmlPane Xaml { get; set; }
        private Root Root { get; set; }
        private FrameworkElement undoButton;
        private FrameworkElement redoButton;
        private Image fullScreen;
        private Image exitFullScreen;

        public Designer()
        {
            ActionManager = new ActionManager();
            ActionManager.CollectionChanged += ActionManager_CollectionChanged;

            Background = Brushes.DesignerBackground;

            this.MouseLeftButtonDown += Designer_MouseLeftButtonDown;
            this.MouseMove += Designer_MouseMove;
            this.MouseLeftButtonUp += Designer_MouseLeftButtonUp;
            this.KeyDown += Designer_KeyDown;
            this.SizeChanged += Designer_SizeChanged;

            // 1. ToolBox

            var toolBox1 = new Toolbox();
            Factory.AddToolBoxItem(toolBox1, "Grid");
            Factory.AddToolBoxItem(toolBox1, "StackPanel");
            toolBox1.HorizontalAlignment = HorizontalAlignment.Stretch;
            toolBox1.Margin = new Thickness(0, 0, 0, 16);

            var toolBox2 = new Toolbox();
            toolBox2.HorizontalAlignment = HorizontalAlignment.Stretch;
            Factory.FillToolbox(toolBox2);

            var toolBox = new StackPanel();
            toolBox.Margin = new Thickness(0, 16, 48, 0);
            toolBox.HorizontalAlignment = HorizontalAlignment.Right;
            toolBox.VerticalAlignment = VerticalAlignment.Top;
            toolBox.Children.Add(toolBox1);
            toolBox.Children.Add(toolBox2);

            // 2. Root

            Root = new Root();

            // 3. PropertyGrid

            var propertyGrids = new StackPanel();
            propertyGrids.MinWidth = 200;
            propertyGrids.Margin = new Thickness(48, 16, 0, 0);
            propertyGrids.HorizontalAlignment = HorizontalAlignment.Left;
            propertyGrids.VerticalAlignment = VerticalAlignment.Top;

            PropertyGrid = new PropertyGrid(false);
            propertyGrids.Children.Add(PropertyGrid);

            GridPropertyGrid = new PropertyGrid(false);
            GridPropertyGrid.Margin = new Thickness(0, 32, 0, 0);
            propertyGrids.Children.Add(GridPropertyGrid);

            // Recycle Bin

            RecycleBin = new RecycleBin();
            RecycleBin.HorizontalAlignment = HorizontalAlignment.Left;
            RecycleBin.VerticalAlignment = VerticalAlignment.Top;
            RecycleBin.UpdateOpacity();

            // Undo/Redo
            undoButton = Visuals.GetImage("Undo32.png");
            redoButton = Visuals.GetImage("Redo32.png");
            undoButton.MouseLeftButtonDown += undoButton_MouseLeftButtonDown;
            redoButton.MouseLeftButtonDown += redoButton_MouseLeftButtonDown;
            redoButton.Margin = new Thickness(16, 0, 0, 0);
            UpdateUndoRedo();
            var undoRedo = new StackPanel();
            undoRedo.Children.Add(undoButton);
            undoRedo.Children.Add(redoButton);
            undoRedo.HorizontalAlignment = HorizontalAlignment.Center;
            undoRedo.VerticalAlignment = VerticalAlignment.Top;
            undoRedo.Margin = new Thickness(16);
            undoRedo.Orientation = Orientation.Horizontal;

            // Central part

            var centralPart = new Grid();
            // rows
            var topRow = new RowDefinition();
            topRow.Height = new GridLength(1, GridUnitType.Star);
            var centerRow = new RowDefinition();
            centerRow.Height = GridLength.Auto;
            var bottomRow = new RowDefinition();
            bottomRow.Height = new GridLength(1, GridUnitType.Star);
            centralPart.RowDefinitions.Add(topRow);
            centralPart.RowDefinitions.Add(centerRow);
            centralPart.RowDefinitions.Add(bottomRow);

            // columns
            var leftColumn = new ColumnDefinition();
            leftColumn.Width = new GridLength(1, GridUnitType.Star);
            var centerColumn = new ColumnDefinition();
            centerColumn.Width = GridLength.Auto;
            var rightColumn = new ColumnDefinition();
            rightColumn.Width = new GridLength(1, GridUnitType.Star);
            centralPart.ColumnDefinitions.Add(leftColumn);
            centralPart.ColumnDefinitions.Add(centerColumn);
            centralPart.ColumnDefinitions.Add(rightColumn);

            centralPart.Children.Add(toolBox);
            centralPart.Children.Add(Root);
            centralPart.Children.Add(propertyGrids);
            centralPart.Children.Add(RecycleBin);
            Grid.SetColumn(toolBox, 0);
            Grid.SetColumn(Root, 1);
            Grid.SetColumn(propertyGrids, 2);
            Grid.SetRow(toolBox, 1);
            Grid.SetRow(Root, 1);
            Grid.SetRow(propertyGrids, 1);
            Grid.SetColumn(RecycleBin, 2);
            Grid.SetRow(RecycleBin, 2);

            var scrollViewer = new ScrollViewer();
            scrollViewer.BorderBrush = null;
            scrollViewer.BorderThickness = new Thickness();
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.Content = centralPart;

            this.Children.Add(scrollViewer);
            this.Children.Add(undoRedo);

            Xaml = new XmlPane();
            Xaml.HorizontalAlignment = HorizontalAlignment.Left;
            Xaml.VerticalAlignment = VerticalAlignment.Top;
            Xaml.Margin = new Thickness(16);
            Canvas.SetZIndex(Xaml, -1);
            this.Children.Add(Xaml);
            UpdateXaml();

            string uri = "http://blogs.msdn.com/b/kirillosenkov";
            var link = new HyperlinkButton() 
            { 
                Content = uri,
                FontSize = 18,
                FontFamily = new FontFamily("Arial"),
                Margin = new Thickness(16),
                TargetName = "_blank",
            };
            link.NavigateUri = new Uri(uri);
            var linkToolbar = new StackPanel();
            linkToolbar.Children.Add(link);
            linkToolbar.HorizontalAlignment = HorizontalAlignment.Right;
            linkToolbar.VerticalAlignment = VerticalAlignment.Bottom;
            this.Children.Add(linkToolbar);

            var help = Visuals.GetImage("help.png");
            help.MouseLeftButtonDown += help_MouseLeftButtonDown;
            help.Margin = new Thickness(16, 0, 0, 0);

            fullScreen = Visuals.GetImage("FullScreen32.png");
            exitFullScreen = Visuals.GetImage("ExitFullScreen32.png");
            fullScreen.MouseLeftButtonDown += fullScreen_MouseLeftButtonDown;
            exitFullScreen.MouseLeftButtonDown += new MouseButtonEventHandler(exitFullScreen_MouseLeftButtonDown);
            exitFullScreen.Visibility = Visibility.Collapsed;
            var topToolbar = new StackPanel();
            topToolbar.Children.Add(fullScreen);
            topToolbar.Children.Add(exitFullScreen);
            topToolbar.Children.Add(help);
            topToolbar.VerticalAlignment = VerticalAlignment.Top;
            topToolbar.HorizontalAlignment = HorizontalAlignment.Right;
            topToolbar.Orientation = Orientation.Horizontal;
            topToolbar.Margin = new Thickness(16);
            this.Children.Add(topToolbar);

            Canvas = new Canvas();
            Canvas.IsHitTestVisible = false;
            this.Children.Add(Canvas);

            Instance = this;
        }

        void fullScreen_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            exitFullScreen.Visibility = Visibility.Visible;
            fullScreen.Visibility = Visibility.Collapsed;
            Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;
        }

        void exitFullScreen_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            exitFullScreen.Visibility = Visibility.Collapsed;
            fullScreen.Visibility = Visibility.Visible;
            Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;
        }

        void help_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Children.Add(new HelpWindow());
        }

        private void UpdateUndoRedo()
        {
            double opacity = UI.GrayedOpacity;
            if (ActionManager.CanUndo)
            {
                undoButton.Opacity = 1;
            }
            else
            {
                undoButton.Opacity = opacity;
            }

            if (ActionManager.CanRedo)
            {
                redoButton.Opacity = 1;
            }
            else
            {
                redoButton.Opacity = opacity;
            }
        }

        void redoButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Redo();
        }

        private void Redo()
        {
            if (ActionManager.CanRedo)
            {
                SelectionManager.ClearSelection();
                ActionManager.Redo();
            }
        }

        void undoButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Undo();
        }

        private void Undo()
        {
            if (ActionManager.CanUndo)
            {
                SelectionManager.ClearSelection();
                ActionManager.Undo();
            }
        }

        private void ActionManager_CollectionChanged(object sender, System.EventArgs e)
        {
            Designer.Instance.UpdateXaml();
            Dispatcher.BeginInvoke(SelectionManager.UpdateSelection);
            UpdateUndoRedo();
        }

        private void Designer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SelectionManager.ClearSelection();
        }

        private void Designer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                SelectionManager.ClearSelection();
                GridPropertyGrid.Show(null, null);
            }
        }

        private void Designer_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Escape || (e.Key == Key.Z && Keyboard.Modifiers.HasFlag(ModifierKeys.Control)) || e.Key == Key.Left)
                && ActionManager.CanUndo)
            {
                Undo();
                return;
            }

            if (((e.Key == Key.Y && Keyboard.Modifiers.HasFlag(ModifierKeys.Control)) || e.Key == Key.Right)
                && ActionManager.CanRedo)
            {
                Redo();
                return;
            }

            if (e.Key == Key.Delete)
            {
                SelectionManager.Delete();
                return;
            }

            if (e.Key == Key.C && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                Xaml.Copy();
                return;
            }
        }

        private void Designer_MouseMove(object sender, MouseEventArgs e)
        {
            DragManager.MouseMove(e.GetPosition(this));
        }

        private void Designer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!e.Handled)
            {
                DragManager.Drop();
                e.Handled = true;
            }
        }

        private void UpdateXaml()
        {
            Xaml.Display(Root.WriteXaml());
        }

        public void ShowProperties(object instance)
        {
            PropertyGrid.Show(instance, ActionManager);
        }
    }
}
