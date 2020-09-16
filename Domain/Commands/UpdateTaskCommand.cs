using System;

namespace Domain.Commands
{
    public class UpdateTaskCommand
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string IsComplete { get; set; }
        public string AssignedToId { get; set; }
    }
}
