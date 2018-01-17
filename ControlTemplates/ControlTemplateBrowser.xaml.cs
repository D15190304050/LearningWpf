using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Reflection;
using System.Xml;
using System.Windows.Markup;

namespace ControlTemplates
{
    /// <summary>
    /// ControlTemplateBrowser.xaml 的交互逻辑
    /// </summary>
    public partial class ControlTemplateBrowser : Window
    {
        public ControlTemplateBrowser()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            Type controlType = typeof(Control);
            List<Type> derivedTypes = new List<Type>();

            // Search all the types in the assembly where the Control class is defined.
            Assembly assembly = Assembly.GetAssembly(typeof(Control));
            foreach (Type type in assembly.GetTypes())
            {
                // Only add a type of the list if it's a control, a concrete class, and public.
                if (type.IsSubclassOf(controlType) && (!type.IsAbstract) && type.IsPublic)
                    derivedTypes.Add(type);
            }

            // Sort the types by name.
            derivedTypes.Sort(new TypeComparer());

            // Show the list of types.
            lstTypes.ItemsSource = derivedTypes;
        }

        private void lstTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Get the selected type.
                Type type = (Type)lstTypes.SelectedItem;

                // Instantiate the type.
                ConstructorInfo info = type.GetConstructor(System.Type.EmptyTypes);
                Control control = (Control)info.Invoke(null);

                Window window = control as Window;
                if (window != null)
                {
                    // Create the window but minimized.
                    window.WindowState = System.Windows.WindowState.Minimized;
                    window.ShowInTaskbar = false;
                    window.Show();
                }
                else
                {
                    // Add it to the grid (but keep it hidden).
                    control.Visibility = Visibility.Collapsed;
                    grid.Children.Add(control);
                }

                // Get the template.
                ControlTemplate template = control.Template;

                // Get the XAML for the template.
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                StringBuilder sb = new StringBuilder();
                XmlWriter writer = XmlWriter.Create(sb, settings);
                XamlWriter.Save(template, writer);

                // Display the template.
                txtTemplate.Text = sb.ToString();

                // Remove the control from the grid.
                if (window != null)
                    window.Close();
                else
                    grid.Children.Remove(control);
            }
            catch (Exception err)
            {
                txtTemplate.Text = "<< Error generating template: " + err.Message + " >>";
            }
        }
    }

    public class TypeComparer : IComparer<Type>
    {
        public int Compare(Type x, Type y) => x.Name.CompareTo(y.Name);
    }
}
