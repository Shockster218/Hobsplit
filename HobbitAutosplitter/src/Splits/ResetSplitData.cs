namespace HobbitAutosplitter
{
    /// <summary>
    /// Split data inheritance class for the split that resets the game. Also in charge of thief split (since the class has no special cropping)
    /// </summary>
    class ResetSplitData : SplitData
    {
        public ResetSplitData(string name, string imagePath) : base(name, imagePath)
        {
        }
        public override float GetIncomingFrameSimilarity()
        {
            return 0f;
        }
    }
}
