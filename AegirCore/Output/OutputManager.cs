using System.Collections.Generic;

namespace AegirCore.Output
{
    public class OutputManager
    {
        public List<OutputData> Outputs { get; set; }

        public OutputManager()
        {
            //Add some test outputs
            Outputs = new List<OutputData>();
            Outputs.Add(new OutputData("Foo", 1, 2));
        }
    }
}