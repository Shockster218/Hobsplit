using System.Linq;
using System.Windows.Forms;
using System.IO;
using System;

namespace HobbitAutosplitter
{
    public partial class ChooseFolderComponent : System.Windows.Controls.UserControl
    {
        public ChooseFolderComponent()
        {
            InitializeComponent();
        }

        private void Browse_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string message = "This will start an automated process to find and match all images to the corresponding splits in the selected " +
                "folder. Please make sure the images are named similar to the split they represent (check split names in previous screen) " +
                "or are numbered in the order in which they appear during the run STARTING WITH 0. Accepted image types are png, jpg, and png. " +
                "If you have done this already, you may click the OK button and continue. You can always manually set them after " +
                "if they are incorrectly matched.";
            if (MessageBox.Show(message, "PLEASE READ BEFORE CONTINUING", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK) ShowFolderDialog();
        }

        private void ShowFolderDialog()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = false;
            dialog.Description = "Select the folder with your split images. Then click OK.";

            if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                string path = dialog.SelectedPath;
                string[] allowedExtensions = new string[] { ".bmp", ".jpg", ".png" };
                string[] images = Directory
                                      .GetFiles(path)
                                      .Where(file => allowedExtensions.Any(file.ToLower().EndsWith)).ToArray();
            }
        }
    }
}
