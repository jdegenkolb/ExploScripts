using ExploScripts.Data;
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

namespace ExploScripts
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
            // When the program is run by the context menu entry, the working directory is not correct.
            // So the correct working directory (where ExploScripts is installed) is passed as a command line arg.           
            if (Environment.GetCommandLineArgs().Length >= 2)
            {
                Directory.SetCurrentDirectory(Environment.GetCommandLineArgs()[1]);
            }

            // Initialize the script database and setup the data binding to the view
            db = new ExploScriptDatabase();

            if (db.NewInstall)
            {
                string msg = "Hello! It seems like this is your first execution of ExploScripts. I will now install " +
                    "ExploScripts in this folder. That means all your scripts and templates will be stored here. " +
                    "It is recommended to install ExploScripts in an own folder inside your Documents folder.\nDo you want to continue?";

                MessageBoxResult res = MessageBox.Show(msg, "Installation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (res == MessageBoxResult.No)
                {
                    Environment.Exit(0);
                }
            }

            db.Initialize();
            dataGrid.ItemsSource = db.Scripts;
            cmh = new ContextMenuHandler(db.Scripts);
        }

        private void DG_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            // Open the script containing folder on hyperlink click
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
            infoWin.Cmh = cmh;
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
            // Finish the current entry (to make sure all changes are commited)
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
