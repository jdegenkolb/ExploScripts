using ExploScripts.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExploScripts
{
    /// <summary>
    /// Interaktionslogik für InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public ContextMenuHandler Cmh { get; set; }

        public InfoWindow()
        {
            InitializeComponent();
        }

        private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }

        private void btnUninstall_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you really want to uninstall ExploScripts? This will just remove the context menu entry. The files must be taken care of yourself.", "Uninstall", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Cmh.Clear();
                Environment.Exit(0);
            }
        }
    }
}
