using AegirCore.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore
{
    public class ApplicationContext
    {
        public ProjectContext Project { get; set; }

        public ApplicationContext()
        {
            Project = new ProjectContext();
            
        }
    }
}
