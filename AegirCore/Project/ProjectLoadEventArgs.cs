using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegirCore.Project
{
    public enum ProjectLoadStatus
    {
        NOTEXISTING,
        SUCCESS
    }
    public class ProjectLoadEventArgs : EventArgs
    {
        public ProjectLoadStatus LoadStatus { get; private set; }
        public ProjectFile Project { get; private set; }
        public string FilePath { get; private set; }

        public ProjectLoadEventArgs(string filepath)
        {
            FilePath = filepath;
            LoadStatus = ProjectLoadStatus.NOTEXISTING;
        }
        public ProjectLoadEventArgs(string filepath, ProjectFile project)
        {
            FilePath = filepath;
            Project = project;
            LoadStatus = ProjectLoadStatus.SUCCESS;
        }
    }
}
