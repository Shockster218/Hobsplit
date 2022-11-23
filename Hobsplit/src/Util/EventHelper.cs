namespace Hobsplit
{
    public delegate void SmartEventHandler();
    public delegate void LivesplitActionEventHandler(LivesplitAction action = LivesplitAction.NONE);
    public delegate void PreviewFrameEventHandler(byte[] frameData);
    public delegate void AdvancedSplitInformationEventHandler(AdvancedSplitInfoArgs args);

    public class AdvancedSplitInfoArgs
    {
        public int splitIndex;
        public float similarity;
        public SplitState state;

        public AdvancedSplitInfoArgs(int splitIndex, float similarity, SplitState state)
        {
            this.splitIndex = splitIndex;
            this.similarity = similarity;
            this.state = state;
        }
    }
}
