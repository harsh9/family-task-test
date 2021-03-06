﻿using System.Threading.Tasks;
using Domain.Commands;
using Domain.Queries;

namespace WebClient.Abstractions
{
    public interface ITaskDataService
    {
        public Task<CreateTaskCommandResult> Create(CreateTaskCommand command);
        public Task<UpdateTaskCommandResult> Update(UpdateTaskCommand command);
        public Task<DeleteTaskCommandResult> Delete(DeleteTaskCommand command);
        public Task<GetAllTasksQueryResult> GetAllTasks();
    }
}
