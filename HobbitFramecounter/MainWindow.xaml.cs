using System.Windows;
using Microsoft.Win32;
using System.Windows.Media.Imaging;


namespace HobbitFramecounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow instance;
        public MainWindow()
        {
            InitializeComponent();
            instance = this;
        }

        private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            //dialog.Filter = "All Media Files| *.wav; *.aac; *.wma; *.wmv; *.avi; *.mpg; *.mpeg; *.m1v; *.mp2; *.mp3; *.mpa; *.mpe; *.m3u; *.mp4; *.mov; *.3g2; *.3gp2; *.3gp; *.3gpp; *.m4a; *.cda; *.aif; *.aifc; *.aiff; *.mid; *.midi; *.rmi; *.mkv; *.WAV; *.AAC; *.WMA; *.WMV; *.AVI; *.MPG; *.MPEG; *.M1V; *.MP2; *.MP3; *.MPA; *.MPE; *.M3U; *.MP4; *.MOV; *.3G2; *.3GP2; *.3GP; *.3GPP; *.M4A; *.CDA; *.AIF; *.AIFC; *.AIFF; *.MID; *.MIDI; *.RMI; *.MKV";

            if (dialog.ShowDialog() == true)
            {
                textblockPathOutput.Text = dialog.FileName;;
            }
        }

        private void radioButton60FPS_Checked(object sender, RoutedEventArgs e)
        {
            if (radiobutton30FPS != null) radiobutton30FPS.IsChecked = false;
        }

        private void radiobutton30FPS_Checked(object sender, RoutedEventArgs e)
        {
            if (radiobutton60FPS != null) radiobutton60FPS.IsChecked = false;
        }
    }
}
