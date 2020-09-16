using System;

namespace Domain.DataModels
{
    public class Task
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Subject { get; set; }
        public string IsComplete { get; set; }
        public string AssignedToId { get; set; }
    }
}