using Domain.Commands;
using Domain.Queries;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading.Tasks;
using WebClient.Abstractions;

namespace WebClient.Services
{
    // Need to change model and Command after creating it change here

    public class ManageTasksDataService : IMemberDataService
    {
        private HttpClient _httpClient;
        public ManageTasksDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CreateMemberCommandResult> Create(CreateMemberCommand command)
        {            
            return await _httpClient.PostJsonAsync<CreateMemberCommandResult>("members", command);
        }

        public async Task<GetAllMembersQueryResult> GetAllMembers()
        {
            return await _httpClient.GetJsonAsync<GetAllMembersQueryResult>("members");
        }

        public async Task<UpdateMemberCommandResult> Update(UpdateMemberCommand command)
        {
            return await _httpClient.PutJsonAsync<UpdateMemberCommandResult>($"members/{command.Id}", command);
        }
    }
}
