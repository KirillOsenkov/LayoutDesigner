using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using DynamicGeometry;

namespace GuiLabs.LayoutDesigner
{
    public class XamlWriter
    {
        private static Dictionary<string, string> defaultValues = new Dictionary<string, string>()
        {
            { "HorizontalAlignment", "Stretch" },
            { "VerticalAlignment", "Stretch" },
            { "Orientation", "Vertical" },
        };

        public static XElement WriteCore(DependencyObject element)
        {
            XElement result = new XElement(element.GetType().Name);

            AddProperties(element, result);
            WriteGridRowColumn(element, result);

            var children = GetChildren(element);

            if (children != null)
            {
                foreach (DependencyObject child in children)
                {
                    XElement childXaml = Write(child);
                    result.Add(childXaml);
                }
            }

            return result;
        }

        public static void WriteGridRowColumn(DependencyObject element, XElement result)
        {
            int column = Grid.GetColumn((FrameworkElement)element);
            int row = Grid.GetRow((FrameworkElement)element);

            if (column > 0)
            {
                result.Add(new XAttribute("Grid.Column", column.ToString()));
            }
            if (row > 0)
            {
                result.Add(new XAttribute("Grid.Row", row.ToString()));
            }
        }

        public static XElement Write(DependencyObject child)
        {
            XElement childXaml = null;
            var knowsHowToWriteItself = child as IXamlWriter;
            if (knowsHowToWriteItself != null)
            {
                childXaml = knowsHowToWriteItself.WriteXaml();
            }
            else
            {
                childXaml = WriteCore(child);
            }
            return childXaml;
        }

        public static void AddProperties(DependencyObject element, XElement result)
        {
            var proxy = ProxyManager.GetProxy(element);
            if (proxy != element)
            {
                ValueDiscoveryStrategy valueProvider = new ExcludeByDefaultValueDiscoveryStrategy();
                var values = valueProvider.GetValues(proxy);
                foreach (var value in values)
                {
                    string propertyName = value.Name;
                    string propertyValue = value.GetValue<object>().ToString();
                    string defaultValue = null;
                    if (defaultValues.TryGetValue(propertyName, out defaultValue) && defaultValue == propertyValue)
                    {
                        continue;
                    }
                    result.Add(new XAttribute(propertyName, propertyValue));
                }
            }
        }

        private static IEnumerable GetChildren(DependencyObject element)
        {
            var panel = element as Panel;
            if (panel != null)
            {
                return panel.Children;
            }

            return null;
        }
    }
}
