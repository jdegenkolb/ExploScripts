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
using System.IO;
using ExploScripts.Data;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace ExploScripts
{
    /// <summary>
    /// Interaktionslogik für NewWindow.xaml
    /// </summary>
    public partial class NewWindow : Window
    {
        public ObservableCollection<ExploScript> Scripts { get; set; }

        public ExploScript NewScript { get; private set; }

        public NewWindow()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            txtName.Focus();

            if (Directory.Exists("Templates"))
            {
                string[] files = Directory.GetFiles("Templates");
                foreach (var file in files)
                {
                    if (file.EndsWith(".py"))
                    {
                        cmbTemplates.Items.Add(Path.GetFileName(file));
                    }
                }
            }
            else
            {
                InstallTemplates();
                Initialize();
            }
        }

        private void InstallTemplates()
        {
            Directory.CreateDirectory("Templates");
            File.WriteAllBytes("Templates\\Blank.py", ExploScripts.Properties.Resources.Blank);
            File.WriteAllBytes("Templates\\CopyFiles.py", ExploScripts.Properties.Resources.CopyFiles);
            File.WriteAllBytes("Templates\\CopyFilesAndExecute.py", ExploScripts.Properties.Resources.CopyFilesAndExecute);
            File.WriteAllBytes("Templates\\CreateConfigFile.py", ExploScripts.Properties.Resources.CreateConfigFile);
            File.WriteAllBytes("Templates\\CreateConfigFileFromTemplate.py", ExploScripts.Properties.Resources.CreateConfigFileFromTemplate);
            File.WriteAllBytes("Templates\\ProcessFile.py", ExploScripts.Properties.Resources.ProcessFile);
        }

        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            ExploScript es = new ExploScript(true, txtName.Text, txtCaption.Text);

            if (string.IsNullOrWhiteSpace(es.Name))
            {
                MessageBox.Show("The name must not be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (var script in Scripts)
            {
                if (es.Name == script.Name)
                {
                    MessageBox.Show("The name must be unique.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            Directory.CreateDirectory("Scripts\\" + es.Name);
            File.Copy("Templates\\" + cmbTemplates.SelectedItem.ToString(), "Scripts\\" + es.Name + "\\" + es.Name + ".py");

            NewScript = es;
            DialogResult = true;
        }

        private void txtName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }

        private void txtName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "[a-zA-Z0-9]");
        }
    }
}
