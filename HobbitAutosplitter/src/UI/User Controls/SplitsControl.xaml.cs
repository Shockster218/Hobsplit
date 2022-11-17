using System.Windows.Controls;
using System.Windows;
using Microsoft.Win32;
using System.Drawing;

namespace HobbitAutosplitter
{
    public partial class SplitsControl : UserControl
    {
        public string menuPath { get; private set; }
        public string dwPath { get; private set; }
        public string aupPath { get; private set; }
        public string rmPath { get; private set; }
        public string thPath { get; private set; }
        public string ohPath { get; private set; }
        public string riddlesPath { get; private set; }
        public string fasPath { get; private set; }
        public string boobPath { get; private set; }
        public string awwPath { get; private set; }
        public string thiefPath { get; private set; }
        public string iiPath { get; private set; }
        public string gotcPath { get; private set; }
        public string tcbPath { get; private set; }
        public string finalPath { get; private set; }

        public SplitsControl()
        {
            InitializeComponent();
        }

        public void LoadSplitImagePaths()
        {
            menuPath = Settings.Default.menuPath;
            dwPath = Settings.Default.dwPath;
            aupPath = Settings.Default.aupPath;
            rmPath = Settings.Default.rmPath;
            thPath = Settings.Default.thPath;
            ohPath = Settings.Default.ohPath;
            riddlesPath = Settings.Default.riddlesPath;
            fasPath = Settings.Default.fasPath;
            boobPath = Settings.Default.boobPath;
            awwPath = Settings.Default.awwPath;
            thiefPath = Settings.Default.thiefPath;
            iiPath = Settings.Default.iiPath;
            gotcPath = Settings.Default.gotcPath;
            tcbPath = Settings.Default.tcbPath;
            finalPath = Settings.Default.finalPath;
        }

        public void SetSplitImages(bool overrideStartup = false)
        {
            if (((SplitImagesWindow)Window.GetWindow(this)).fromStartup && !overrideStartup) return;
            try
            {
                SplitData[] splits = SplitManager.GetSplitDataArray();
                Split_Image.Source = splits[0].GetImageOriginalWorkable().ToBitmapImage();
                Split_Image1.Source = splits[2].GetImageOriginalWorkable().ToBitmapImage();
                Split_Image2.Source = splits[3].GetImageOriginalWorkable().ToBitmapImage();
                Split_Image3.Source = splits[4].GetImageOriginalWorkable().ToBitmapImage();
                Split_Image4.Source = splits[5].GetImageOriginalWorkable().ToBitmapImage();
                Split_Image5.Source = splits[6].GetImageOriginalWorkable().ToBitmapImage();
                Split_Image6.Source = splits[7].GetImageOriginalWorkable().ToBitmapImage();
                Split_Image7.Source = splits[8].GetImageOriginalWorkable().ToBitmapImage();
                Split_Image8.Source = splits[9].GetImageOriginalWorkable().ToBitmapImage();
                Split_Image9.Source = splits[10].GetImageOriginalWorkable().ToBitmapImage();
                Split_Image10.Source = splits[11].GetImageOriginalWorkable().ToBitmapImage();
                Split_Image11.Source = splits[13].GetImageOriginalWorkable().ToBitmapImage();
                Split_Image12.Source = splits[14].GetImageOriginalWorkable().ToBitmapImage();
                Split_Image13.Source = splits[15].GetImageOriginalWorkable().ToBitmapImage();
                Split_Image14.Source = splits[16].GetImageOriginalWorkable().ToBitmapImage();
            }
            catch{ }
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
                Canvas canvas = sender as Canvas;
                switch (canvas.Name)
                {
                    case "Menu":
                        menuPath = openFileDialog.FileName;
                        break;
                    case "DW":
                        dwPath = openFileDialog.FileName;
                        break;
                    case "AUP":
                        aupPath = openFileDialog.FileName;
                        break;
                    case "RM":
                        rmPath = openFileDialog.FileName;
                        break;
                    case "TH":
                        thPath = openFileDialog.FileName;
                        break;
                    case "OH":
                        ohPath = openFileDialog.FileName;
                        break;
                    case "Riddles":
                        riddlesPath = openFileDialog.FileName;
                        break;
                    case "FAS":
                        fasPath = openFileDialog.FileName;
                        break;
                    case "Barrels":
                        boobPath = openFileDialog.FileName;
                        break;
                    case "AWW":
                        awwPath = openFileDialog.FileName;
                        break;
                    case "Thief":
                        thiefPath = openFileDialog.FileName;
                        break;
                    case "InsideInfo":
                        iiPath = openFileDialog.FileName;
                        break;
                    case "GOTC":
                        gotcPath = openFileDialog.FileName;
                        break;
                    case "TCB":
                        tcbPath = openFileDialog.FileName;
                        break;
                    case "Final":
                        finalPath = openFileDialog.FileName;
                        break;
                }
                Bitmap bitmap = new Bitmap(openFileDialog.FileName);
                ((System.Windows.Controls.Image)((Border)canvas.Children[0]).Child).Source = bitmap.ToBitmapImage();
            }
        }
    }
}
