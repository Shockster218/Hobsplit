namespace HobbitAutosplitter
{
    /// <summary>
    /// Split data inheritance class for the split that completes the run.
    /// </summary>
    class EndSplitData : SplitData
    {
        public EndSplitData(string name, string imagePath) : base(name, imagePath)
        {
            crop = new RECT(0, 0, 0, 0);
        }
        public override float GetIncomingFrameSimilarity()
        {
            return 0f;
        }
    }
}
