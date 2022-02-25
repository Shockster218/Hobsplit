using System.Windows.Controls;
using System.Windows;
using Microsoft.Win32;
using System.Drawing;

namespace HobbitAutosplitter
{
    public partial class ChangeSplitsControl : UserControl
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

        public ChangeSplitsControl()
        {
            InitializeComponent();
            LoadSplitImagePaths();
            SetSplitImages();
        }

        private void LoadSplitImagePaths()
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

        private void SetSplitImages()
        {
            try
            {
                Split_Image.Source = new Bitmap(menuPath).ToBitmapImage();
                Split_Image1.Source = new Bitmap(dwPath).ToBitmapImage();
                Split_Image2.Source = new Bitmap(aupPath).ToBitmapImage();
                Split_Image3.Source = new Bitmap(rmPath).ToBitmapImage();
                Split_Image4.Source = new Bitmap(thPath).ToBitmapImage();
                Split_Image5.Source = new Bitmap(ohPath).ToBitmapImage();
                Split_Image6.Source = new Bitmap(riddlesPath).ToBitmapImage();
                Split_Image7.Source = new Bitmap(fasPath).ToBitmapImage();
                Split_Image8.Source = new Bitmap(boobPath).ToBitmapImage();
                Split_Image9.Source = new Bitmap(awwPath).ToBitmapImage();
                Split_Image10.Source = new Bitmap(thiefPath).ToBitmapImage();
                Split_Image11.Source = new Bitmap(iiPath).ToBitmapImage();
                Split_Image12.Source = new Bitmap(gotcPath).ToBitmapImage();
                Split_Image13.Source = new Bitmap(tcbPath).ToBitmapImage();
                Split_Image14.Source = new Bitmap(finalPath).ToBitmapImage();
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
