public interface TaskEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int IconCode { get; set; }
    public int Points { get; set; }
    public TimeSpan? RepeatAfter { get; set; }
    public TimeSpan? DueDate { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime UpdatedDateUtc { get; set; }
    public DateTime? CompletedDateUtc { get; set; }
}
