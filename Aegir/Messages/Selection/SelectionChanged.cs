using TinyMessenger;

namespace Aegir.Messages.Selection
{
    /// <summary>
    /// Holds data from messages sent when the selection of an editable item in the scene or simulation changes
    /// </summary>
    public class SelectionChanged : GenericTinyMessage<object>
    {
        public SelectionChanged(object sender, object content) : base(sender, content)
        {
        }
    }
}