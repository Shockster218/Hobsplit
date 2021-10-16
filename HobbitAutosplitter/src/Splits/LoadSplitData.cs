namespace HobbitAutosplitter
{
    /// <summary>
    /// Split data inheritance class for the split that controls the loading screens during the run.
    /// </summary>
    public class LoadSplitData : SplitData
    {
        public LoadSplitData(string name, string imagePath) : base(name, imagePath)
        {
            crop = new RECT(0, 0, 0, 0);
        }

        public override float GetIncomingFrameSimilarity()
        {
            return 0f;
        }
    }
}
