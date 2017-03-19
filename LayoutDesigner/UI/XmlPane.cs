using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Linq;
using DynamicGeometry;

namespace GuiLabs.LayoutDesigner
{
    [PropertyGridName("XAML")]
    public class XmlPane : StackPanel
    {
        public readonly TextBlock TextBlock = new TextBlock();
        private ScrollViewer scrollViewer = new ScrollViewer();
        private CheckBox showXaml = new CheckBox();

        public XmlPane()
        {
            Background = Brushes.DesignerBackground;
            IsHitTestVisible = true;

            TextBlock.FontSize = 16;
            TextBlock.FontFamily = new FontFamily("Consolas, Courier New");

            showXaml.Content = "Show XAML";
            showXaml.IsChecked = true;
            showXaml.VerticalAlignment = VerticalAlignment.Center;
            showXaml.Margin = new Thickness(0, 0, 16, 0);

            var image = Visuals.GetImage("copyHS.png");
            image.HorizontalAlignment = HorizontalAlignment.Center;
            var buttonContent = new StackPanel();
            buttonContent.Orientation = Orientation.Horizontal;
            buttonContent.Children.Add(image);
            buttonContent.Children.Add(new TextBlock() 
            { 
                Text = "Copy", 
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(4)
            });
            buttonContent.Margin = new Thickness(2);
            var copyButton = new Button() { Content = buttonContent, MinWidth = 73 };
            copyButton.Click += copyButton_Click;

            scrollViewer.Content = TextBlock;
            scrollViewer.BorderBrush = null;
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            var toolBar = new StackPanel();
            toolBar.Orientation = Orientation.Horizontal;
            toolBar.Children.Add(showXaml);
            toolBar.Children.Add(copyButton);

            this.Children.Add(toolBar);
            this.Children.Add(scrollViewer);

            showXaml.Checked += showXaml_Checked;
            showXaml.Unchecked += showXaml_Checked;
        }

        void copyButton_Click(object sender, RoutedEventArgs e)
        {
            Copy();
        }

        void showXaml_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            scrollViewer.Visibility = showXaml.IsChecked == true ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        int indent = 0;

        [PropertyGridVisible]
        public string Text
        {
            get
            {
                return TextBlock.Text;
            }
        }

        string plainText;

        public void Display(XElement element)
        {
            TextBlock.Inlines.Clear();
            if (element != null)
            {
                Write(element);
                plainText = element.ToString();
            }
        }

        private void Write(XElement element)
        {
            WriteIndent();
            Write("<", Colors.Blue);
            Write(element.Name.ToString(), Color.FromArgb(255, 163, 21, 21));

            if (element.HasAttributes)
            {
                WriteAttributes(element);
            };

            if (element.HasElements)
            {
                Write(">", Colors.Blue);
                WriteLineBreak();
                indent += 2;
                Write(element.Elements());
                indent -= 2;
                WriteIndent();
                Write("</", Colors.Blue);
                Write(element.Name.ToString(), Color.FromArgb(255, 163, 21, 21));
                Write(">", Colors.Blue);
            }
            else
            {
                Write(" />", Colors.Blue);
            }

            WriteLineBreak();
        }

        private void WriteIndent()
        {
            Write(new string(' ', indent));
        }

        private void WriteLineBreak()
        {
            TextBlock.Inlines.Add(new LineBreak());
        }

        private void Write(IEnumerable<XElement> elements)
        {
            foreach (var element in elements)
            {
                Write(element);
            }
        }

        private void WriteAttributes(XElement parent)
        {
            if (parent.HasAttributes)
            {
                var array = parent.Attributes().OrderBy(a => a.Name.LocalName == "Name" ? "_" : a.Name.LocalName).ToArray();
                parent.RemoveAttributes();

                foreach (var attribute in array)
                {
                    if (attribute.Name.LocalName == "Name" && string.IsNullOrEmpty(attribute.Value))
                    {
                        continue;
                    }
                    Write(" ");
                    Write(attribute);
                    parent.Add(attribute);
                }
            }
        }

        private void Write(XAttribute attribute)
        {
            if (attribute.Name == "Name")
            {
                Write("x:", Colors.Red);
            }
            Write(attribute.Name.ToString(), Colors.Red);
            Write("=", Colors.Blue);
            Write("\"");
            Write(attribute.Value, Colors.Blue);
            Write("\"");
        }

        private void Write(string text)
        {
            Write(text, Colors.Black);
        }

        private void Write(string text, Color color)
        {
            TextBlock.Inlines.Add(new Run()
            {
                Text = text,
                Foreground = new SolidColorBrush(color)
            });
        }

        public void Copy()
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return;
            }
            Clipboard.SetText(plainText);
        }
    }
}
