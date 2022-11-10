using System.Windows;


namespace HobbitAutosplitter
{
    public partial class ComparisonSettingsWindow : Window
    {
        public ComparisonSettingsWindow() => InitializeComponent();

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            SaveSplitImagePaths();
            SaveSplitSimilarity();
            Close();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e) => Close();

        private void SaveSplitImagePaths()
        {
            //Settings.Default.menuPath = ChangeSplits.menuPath;
            //Settings.Default.dwPath = ChangeSplits.dwPath;
            //Settings.Default.aupPath = ChangeSplits.aupPath;
            //Settings.Default.rmPath = ChangeSplits.rmPath;
            //Settings.Default.thPath = ChangeSplits.thPath;
            //Settings.Default.ohPath = ChangeSplits.ohPath;
            //Settings.Default.riddlesPath = ChangeSplits.riddlesPath;
            //Settings.Default.fasPath = ChangeSplits.fasPath;
            //Settings.Default.boobPath = ChangeSplits.boobPath;
            //Settings.Default.awwPath = ChangeSplits.awwPath;
            //Settings.Default.thiefPath = ChangeSplits.thiefPath;
            //Settings.Default.iiPath = ChangeSplits.iiPath;
            //Settings.Default.gotcPath = ChangeSplits.gotcPath;
            //Settings.Default.tcbPath = ChangeSplits.tcbPath;
            //Settings.Default.finalPath = ChangeSplits.finalPath;
        }

        private void SaveSplitSimilarity()
        {
            Settings.Default.resetSimilarity = ChangeSimilarity.valueReset;
            Settings.Default.startSimilarity = ChangeSimilarity.valueStart;
            Settings.Default.loadsSimilarity = ChangeSimilarity.valueLoads;
            Settings.Default.thiefSimilarity = ChangeSimilarity.valueThief;
            Settings.Default.finalSimilarity = ChangeSimilarity.valueFinal;
            SplitManager.UpdateSplitSimilarity();
        }
    }
}
