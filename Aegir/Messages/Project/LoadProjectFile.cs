using GalaSoft.MvvmLight.Messaging;

namespace Aegir.Messages.Project
{
    public class LoadProjectFile
    {
        public string FilePath { get; set; }

        private LoadProjectFile(string filepath)
        {
            this.FilePath = filepath;
        }

        public static void Send(string filepath)
        {
            Messenger.Default.Send<LoadProjectFile>(new LoadProjectFile(filepath));
        }
    }
}