using System.IO;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Win32;
using System.Drawing;

namespace Hobsplit
{
    public partial class SplitsControl : UserControl
    {
        public SplitsControl()
        {
            InitializeComponent();
        }

        public void SetSplitImages()
        {
            System.Windows.Controls.Image[] images = PopulateImagesArray();
            SplitData[] splits = SplitManager.GetSplitDataArray();
            for(int i = 0; i < images.Length; i++)
            {
                int index = i >= 1 ? i >= 11 ? i + 2 : i + 1 : i;
                if (splits[index].IsImageInitialized()) images[i].Source = splits[index].GetImageOriginalWorkable().ToBitmapImage();
            }
        }

        private System.Windows.Controls.Image[] PopulateImagesArray()
        {
            return new System.Windows.Controls.Image[]
            {
                Split_Image,
                Split_Image1,
                Split_Image2,
                Split_Image3,
                Split_Image4,
                Split_Image5,
                Split_Image6,
                Split_Image7,
                Split_Image8,
                Split_Image9,
                Split_Image10,
                Split_Image11,
                Split_Image12,
                Split_Image13,
                Split_Image14
            };
        }

        private void Split_Item_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            canvas.Children[1].Visibility = Visibility.Visible;
        }

        private void Split_Item_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            canvas.Children[1].Visibility = Visibility.Hidden;
        }

        private void Split_Item_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Image files |*.png;*.jpeg;*.bmp;*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                Canvas canvas = sender as Canvas;
                switch (canvas.Name)
                {
                    case "Menu":
                        Settings.Default.menuPath = path;
                        SplitManager.UpdateSplit(0, path);
                        SplitManager.UpdateSplit(1, path);
                        break;
                    case "DW":
                        Settings.Default.dwPath = path;
                        SplitManager.UpdateSplit(2, path);
                        break;
                    case "AUP":
                        Settings.Default.aupPath = path;
                        SplitManager.UpdateSplit(3, path);
                        break;
                    case "RM":
                        Settings.Default.rmPath = path;
                        SplitManager.UpdateSplit(4, path);
                        break;
                    case "TH":
                        Settings.Default.thPath = path;
                        SplitManager.UpdateSplit(5, path);
                        break;
                    case "OH":
                        Settings.Default.ohPath = path;
                        SplitManager.UpdateSplit(6, path);
                        break;
                    case "Riddles":
                        Settings.Default.riddlesPath = path;
                        SplitManager.UpdateSplit(7, path);
                        break;
                    case "FAS":
                        Settings.Default.fasPath = path;
                        SplitManager.UpdateSplit(8, path);
                        break;
                    case "Barrels":
                        Settings.Default.boobPath = path;
                        SplitManager.UpdateSplit(9, path);
                        break;
                    case "AWW":
                        Settings.Default.awwPath = path;
                        SplitManager.UpdateSplit(10, path);
                        SplitManager.UpdateSplit(12, path);
                        break;
                    case "Thief":
                        Settings.Default.thiefPath = path;
                        SplitManager.UpdateSplit(11, path);
                        break;
                    case "InsideInfo":
                        Settings.Default.iiPath = path;
                        SplitManager.UpdateSplit(13, path);
                        break;
                    case "GOTC":
                        Settings.Default.gotcPath = path;
                        SplitManager.UpdateSplit(14, path);
                        break;
                    case "TCB":
                        Settings.Default.tcbPath = path;
                        SplitManager.UpdateSplit(15, path);
                        break;
                    case "Final":
                        Settings.Default.finalPath = path;
                        SplitManager.UpdateSplit(16, path);
                        break;
                }

                SetSplitImages();
            }
        }
    }
}
