using System;

public class TaskModel
{
    public Guid Id { get; set; }
    public FamilyMember Member { get; set; }
    public string Subject { get; set; }
    public bool IsComplete { get; set; }
    public DateTime LastChangedOn { get; set; }
}
