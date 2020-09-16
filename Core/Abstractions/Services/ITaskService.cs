using Domain.Commands;
using Domain.Queries;
using System.Threading.Tasks;

namespace Core.Abstractions.Services
{
    public interface ITaskService
    {
        Task<CreateTaskCommandResult> CreateTaskCommandHandler(CreateTaskCommand command);
        Task<UpdateTaskCommandResult> UpdateTaskCommandHandler(UpdateTaskCommand command);
        Task<DeleteTaskCommandResult> DeleteTaskCommandHandler(DeleteTaskCommand command);
        Task<GetAllTasksQueryResult> GetAllTasksQueryHandler();
    }
}