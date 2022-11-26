namespace Hobsplit
{
    public delegate void SmartEventHandler();
    public delegate void LivesplitActionEventHandler(LivesplitAction action = LivesplitAction.NONE);
    public delegate void PreviewFrameEventHandler(byte[] frameData);
    public delegate void AdvancedSplitInformationEventHandler(AdvancedSplitInfoArgs args);

    public class AdvancedSplitInfoArgs
    {
        public int splitIndex;
        public string splitName;
        public float currentSim;
        public float resetSim;
        public SplitState state;

        public AdvancedSplitInfoArgs(int splitIndex, string splitName, float currentSim, float resetSim, SplitState state)
        {
            this.splitIndex = splitIndex;
            this.splitName = splitName;
            this.currentSim = currentSim;
            this.resetSim = resetSim;
            this.state = state;
        }
    }
}
