namespace ServiceManager.Service.Models
{
    public class WorkerState
    {
        public string State { get; set; }
        public IEnumerable<WorkerInfo> Info { get; internal set; }
        public int Count { get => Info.Count(); }
    }
}