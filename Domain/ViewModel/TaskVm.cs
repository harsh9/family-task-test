using System;

namespace Domain.ViewModel
{
    public class TaskVm
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string IsComplete { get; set; }
        public string AssignedToId { get; set; }
    }
}