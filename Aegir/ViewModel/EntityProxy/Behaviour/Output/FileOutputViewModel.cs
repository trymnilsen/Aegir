using AegirLib.Behaviour.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ViewPropertyGrid.PropertyGrid.Component;

namespace Aegir.ViewModel.EntityProxy.Behaviour.Output
{
    [ViewModelForBehaviour(typeof(FileOutput))]
    [DisplayName("File output")]
    [ComponentDescriptor("Write output to file", "Output", true, false)]
    public class FileOutputViewModel : TypedBehaviourViewModel<FileOutput>
    {
        [DisplayName("File Path")]
        public string FilePath { get; set; }
        public FileOutputViewModel(FileOutput component) : base(component)
        {

        }

        internal override void Invalidate()
        {

        }
    }
}
