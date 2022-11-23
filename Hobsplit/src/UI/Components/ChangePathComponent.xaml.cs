using System;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Linq;

namespace Hobsplit
{
    public partial class ChangePathComponent : UserControl
    {
        public ChangePathComponent()
        {
            InitializeComponent();
            Toggle(Settings.Default.autoOBS);
            Path_TextBlock.Text = TrimPath(Settings.Default.obsPath);
        }

        private void Set_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Path.GetPathRoot(Environment.SystemDirectory);
            openFileDialog.Filter = "Exe Files (*.exe)|*.exe";
            bool? result = openFileDialog.ShowDialog();
            
            if (result == true)
            {
                string path = openFileDialog.FileName;
                Path_TextBlock.Text = TrimPath(path);
                Settings.Default.obsPath = openFileDialog.FileName;
                Settings.Default.Save();
            }
        }

        private string TrimPath(string path)
        {
            string[] directories = path.Split('\\');
            return Path.GetPathRoot(path) + "..." + string.Join(@"\", directories.Skip(directories.Length - 3));
        }

        public void Toggle(bool state)
        {
            if (state) Visibility = Visibility.Visible;
            else Visibility = Visibility.Collapsed;
        }
    }
}
