namespace TestTask.DAL.Models
{
    public abstract class BaseDbEntity
    {
        public DateTime Created { get; internal set; } = DateTime.UtcNow;
        public DateTime Modified { get; internal set; } = DateTime.UtcNow;
    }
}
