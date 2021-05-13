using System.Windows;
using Microsoft.Win32;
using System.Windows.Media.Imaging;


namespace HobbitAutoSplitter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow instance;
        public MainWindow()
        {
            instance = this;
            InitializeComponent();
        }
    }
}
