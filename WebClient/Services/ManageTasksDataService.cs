using Domain.Commands;
using Domain.Queries;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading.Tasks;
using WebClient.Abstractions;

namespace WebClient.Services
{
    public class ManageTasksDataService : ITaskDataService
    {
        private HttpClient _httpClient;
        public ManageTasksDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<CreateTaskCommandResult> Create(CreateTaskCommand command)
        {            
            return await _httpClient.PostJsonAsync<CreateTaskCommandResult>("tasks", command);
        }

        public async Task<GetAllTasksQueryResult> GetAllTasks()
        {
            return await _httpClient.GetJsonAsync<GetAllTasksQueryResult>("tasks");
        }

        public async Task<UpdateTaskCommandResult> Update(UpdateTaskCommand command)
        {
            return await _httpClient.PutJsonAsync<UpdateTaskCommandResult>($"tasks/{command.Id}", command);
        }

        public async Task<DeleteTaskCommandResult> Delete(DeleteTaskCommand command)
        {
            return await _httpClient.PutJsonAsync<DeleteTaskCommandResult>($"tasks/{command.Id}", command);
        }
    }
}
