using System.Windows.Controls;

namespace Hobsplit
{

    public partial class SplitControlComponent : UserControl
    {
        public SplitControlComponent()
        {
            InitializeComponent();
        }

        private void Next_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(SplitManager.GetSplitIndex() < 16) SplitManager.IncrementSplitIndex();
        }

        private void Previous_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(SplitManager.GetSplitIndex() >= 2) SplitManager.DeincrementSplitIndex();
        }
    }
}
