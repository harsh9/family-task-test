using System;

public class TaskModel
{
    public Guid Id { get; set; }
    public FamilyMember Member { get; set; }
    public string Text { get; set; }
    public bool IsDone { get; set; }
}
