namespace HobbitAutosplitter
{
    /// <summary>
    /// Split data inheritance class for the split that starts the game (crops the new game, options, load game, and exit game sections).
    /// </summary>
    class ThiefSplitData : SplitData
    {
        public ThiefSplitData(string name, string imagePath) : base(name, imagePath)
        {
            crop = new RECT(0, 0, 0, 0);
        }
        public override float GetIncomingFrameSimilarity()
        {
            return 0f;
        }
    }
}
