namespace OnaxTools.Dto
{
    public class QueueSettings
    {
        public QueueConfig QueueConfig { get; set; }
    }
    public class QueueConfig
    {
        public virtual string QueueName { get; set; }
        public virtual string QueueConString { get; set; }
    }
}
