using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.Messages.Project
{
    public class SaveProjectFile
    {
        public string FilePath { get; set; }

        private SaveProjectFile(string filepath)
        {
            this.FilePath = filepath;
        }

        public static void Send(string filepath)
        {
            Messenger.Default.Send<SaveProjectFile>(new SaveProjectFile(filepath));
        }
    }
}
