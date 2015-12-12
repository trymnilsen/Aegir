using GalaSoft.MvvmLight.Messaging;

namespace Aegir.Messages.Simulation
{
    public class InvalidateEntities
    {
        private InvalidateEntities()
        {
        }

        public static void Send()
        {
            Messenger.Default.Send<InvalidateEntities>(new InvalidateEntities());
        }
    }
}