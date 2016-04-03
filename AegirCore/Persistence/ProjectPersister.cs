using AegirCore.Project;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Persistence
{
    public class ProjectPersister
    {
        private readonly ScenePersister ScenePersistance;
        private readonly TimelinePersister TimelinePersistance;

        public ProjectPersister()
        {
            ScenePersistance = new ScenePersister();
            TimelinePersistance = new TimelinePersister();
        }

        public void SaveProject(ProjectData data, StreamWriter saveStream)
        {

        }
        public ProjectData LoadProject(StreamReader loadStream)
        {
            return null;
        }
    }
}
