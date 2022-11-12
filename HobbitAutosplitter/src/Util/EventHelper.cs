using System.Windows.Media.Imaging;

namespace HobbitAutosplitter
{
    public delegate void SmartEventHandler();
    public delegate void LivesplitActionEventHandler(LivesplitAction action = LivesplitAction.NONE);
    public delegate void PreviewFrameEventHandler(BitmapImage frame);
}
