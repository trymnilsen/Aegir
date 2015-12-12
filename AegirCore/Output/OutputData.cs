namespace AegirCore.Output
{
    public class OutputData
    {
        public string Name { get; set; }
        public int Port { get; set; }
        public int Listeners { get; set; }

        public OutputData()
        {
        }

        public OutputData(string name, int port, int listeners)
        {
            Name = name;
            Port = port;
            Listeners = listeners;
        }
    }
}