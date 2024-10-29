public interface Parent
{
    public ParentRole Role { get; set; }
    public List<Child> Children { get; set; }
    public InternalBufferOverflowException IconCode { get; set; }
    public string Name { get; set; }
    public DateTime DOB { get; set; }
}
public enum ParentRole
{
    Mother,
    Father
}
