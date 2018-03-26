using ExploScriptsWPF.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace ExploScriptsWPF
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ExploScriptDatabase db;
        private ContextMenuHandler cmh;

        public MainWindow()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            if (Environment.GetCommandLineArgs().Length >= 2)
            {
                Directory.SetCurrentDirectory(Environment.GetCommandLineArgs()[1]);
            }

            db = new ExploScriptDatabase();
            dataGrid.ItemsSource = db.Scripts;
            // listView.ItemsSource = db.Scripts;
            // dataGrid.ItemsSource = db.Scripts;
            cmh = new ContextMenuHandler(db.Scripts);
        }

        private void DG_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            string dir = @"Scripts\" + link.NavigateUri.ToString();

            if (Directory.Exists(dir))
            {
                Process.Start(dir);
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            NewWindow newWin = new NewWindow();
            newWin.Owner = this;
            newWin.Scripts = db.Scripts;
            if (newWin.ShowDialog() == true)
            {
                db.Scripts.Add(newWin.NewScript);
                db.SaveDatabase();
                if (newWin.chkOpenDir.IsChecked == true)
                {
                    Process.Start(@"Scripts\" + newWin.NewScript.Name);
                }
            }
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            InfoWindow infoWin = new InfoWindow();
            infoWin.Owner = this;
            infoWin.Show();
        }

        private void dataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var item = (ExploScript) dataGrid.SelectedItem;
                if (MessageBox.Show("Do you really want to delete " + item.Name + "?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (Directory.Exists(@"Scripts\" + item.Name))
                        Directory.Delete(@"Scripts\" + item.Name, true);
                    db.Scripts.Remove(item);
                    db.SaveDatabase();
                }
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            dataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            db.SaveDatabase();

            // Upgrade Context Menu
            cmh.UpdateRegistry();
        }

        private void btnOpenDir_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Directory.GetCurrentDirectory());
        }
    }
}
