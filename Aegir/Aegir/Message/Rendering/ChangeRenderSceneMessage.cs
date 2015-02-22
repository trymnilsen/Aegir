using Aegir.Rendering;
using GalaSoft.MvvmLight.Messaging;
using System;

namespace Aegir.Message.Rendering
{
    internal class ChangeRenderSceneMessage
    {
        public IRenderScene Item { get; private set; }
        public String SceneId { get; private set; }

        private ChangeRenderSceneMessage(IRenderScene item)
        {
            this.Item = item;
        }
        private ChangeRenderSceneMessage(String sceneId)
        {
            this.SceneId = sceneId;
        }
        public static void Send(IRenderScene newItem)
        {
            Messenger.Default.Send<ChangeRenderSceneMessage>(new ChangeRenderSceneMessage(newItem));
        }
        public static void Send(String sceneId)
        {
            Messenger.Default.Send<ChangeRenderSceneMessage>(new ChangeRenderSceneMessage(sceneId));
        }
    }
}
