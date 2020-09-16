
namespace Domain.Commands
{
    public class CreateTaskCommand
    {
        public string Subject { get; set; }
        public string IsComplete { get; set; }
        public string AssignedToId { get; set; }
    }
}
