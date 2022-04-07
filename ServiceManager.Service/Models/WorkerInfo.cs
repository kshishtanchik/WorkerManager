namespace ServiceManager.Service.Models
{
    public class WorkerInfo
    {
        public TimeSpan TotalProcessorTime { get; internal set; }
        public string Name { get; set; }
        public int Id { get; set; }
    }
}