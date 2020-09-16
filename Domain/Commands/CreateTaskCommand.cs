
namespace Domain.Commands
{
    public class CreateTaskCommand
    {
        public string Subject { get; set; }
        public bool IsComplete { get; set; }
        public string AssignedToId { get; set; }
    }
}
