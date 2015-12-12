using System.ComponentModel;

namespace AegirCore.Scene
{
    public class SceneNode : Node
    {
        [Browsable(false)]
        public string ModelPath { get; set; }

        public SceneNode()
        {
        }

        /// <summary>
        /// Recalculates the absolute position of the node when a parent has been moved
        /// </summary>
        /// <param name="parent"></param>
        public void InvalidateAbsolutePosition(SceneNode parent)
        {
        }
    }
}