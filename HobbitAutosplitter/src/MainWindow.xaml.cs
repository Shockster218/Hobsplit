using System.Diagnostics;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace HobbitAutosplitter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static MainWindow instance;
        private DispatcherTimer dispatcher = new DispatcherTimer();
        private Stopwatch segmentStopwatch = new Stopwatch();
        private ProcessManager processManager = new ProcessManager();

        public MainWindow()
        {
            InitializeComponent();
            instance = this;
        }
    }
}
