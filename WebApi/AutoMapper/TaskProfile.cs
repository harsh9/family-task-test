using AutoMapper;
using Domain.Commands;
using Domain.DataModels;
using Domain.ViewModel;

namespace WebApi.AutoMapper
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<CreateTaskCommand, Task>();
            CreateMap<UpdateTaskCommand, Task>();
            CreateMap<DeleteTaskCommand, Task>();
            CreateMap<Task, TaskVm>();
        }
    }
}
