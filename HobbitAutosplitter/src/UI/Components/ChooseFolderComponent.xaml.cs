using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Windows;
using System.Text.RegularExpressions;

namespace HobbitAutosplitter
{
    public partial class ChooseFolderComponent : System.Windows.Controls.UserControl
    {
        private string[][] levelKeyWords = new string[][]
        {
            new string[]{"0", "main", "menu", "start"},
            new string[]{"1", "dream", "world"},
            new string[]{"2", "unexpected", "party"},
            new string[]{"3", "roast", "mutton"},
            new string[]{"4", "troll", "hole"},
            new string[]{"5", "overhill", "underhill",},
            new string[]{"6", "riddles"},
            new string[]{"7", "flies", "spiders"},
            new string[]{"8", "barrels", "bond"},
            new string[]{"9", "warm", "welcome"},
            new string[]{"10", "thief"},
            new string[]{"11", "inside", "info"},
            new string[]{"12", "gathering", "clouds"},
            new string[]{"13", "clouds", "burst"},
            new string[]{"14", "barrel", "end", "final", "gg"},
        };

        public ChooseFolderComponent()
        {
            InitializeComponent();
        }

        private void Browse_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string message = "This will start an automated process to find and match all images to the corresponding splits in the selected " +
                "folder. Please make sure the images are named similar to the split they represent (refer to names on split window if unsure. " +
                "Do not use acronyms or shortened names!) or are numbered in the order in which they appear during the run STARTING WITH 0. " +
                "Accepted image types are png, jpg, and png. If you have done this already, you may click the OK button and continue. " +
                "You can always manually set them after  if they are incorrectly matched.";
            if (System.Windows.Forms.MessageBox.Show(message, "PLEASE READ BEFORE CONTINUING", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK) ShowFolderDialog();
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
                string[] imagesPath = Directory
                                      .GetFiles(path)
                                      .Where(file => allowedExtensions.Any(file.ToLower().EndsWith)).ToArray();


                for(int i = 0; i < imagesPath.Length; i++)
                {
                    bool imageFound = false;
                    string fileName = Path.GetFileNameWithoutExtension(imagesPath[i]);
                    SplitData[] splits = SplitManager.GetSplitDataArray();

                    for (int j = 0; j < levelKeyWords.Length; j++)
                    {
                        for (int k = 0; k < levelKeyWords[j].Length; k++)
                        {
                            if(int.TryParse(fileName, out int value))
                            {
                                for (int l = 0; l < splits.Length; l++)
                                {
                                    if (splits[l].GetSplitNumber() == value)
                                    {
                                        SplitManager.UpdateSplit(l, imagesPath[i]);
                                        imageFound = true;
                                    }
                                    if (imageFound) break;
                                }
                            }
                            else if (fileName.ToLower().Contains(levelKeyWords[j][k]))
                            {
                                string match = levelKeyWords[j][k];
                                for (int l = 0; l < splits.Length; l++)
                                {
                                    if (splits[l].GetSplitName().Contains(match))
                                    {
                                        SplitManager.UpdateSplit(l, imagesPath[i]);
                                        imageFound = true;
                                    }
                                    if (imageFound) break;
                                }
                            }
                            if (imageFound) break;
                        }
                        if (imageFound) break;
                    }
                }

                SplitManager.UpdateImagesPathSetting();
                ((ComparisonSettingsWindow)Window.GetWindow(this)).UpdateSplitImages();
            }
        }
    }
}
