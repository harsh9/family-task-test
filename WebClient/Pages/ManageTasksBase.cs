namespace WebClient.Pages
{
    using Domain.ViewModel;
    using Microsoft.AspNetCore.Components;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using WebClient.Abstractions;

    public class ManageTasksBase : ComponentBase
    {
        protected List<FamilyMember> members = new List<FamilyMember>();
        protected List<MenuItem> leftMenuItem = new List<MenuItem>();
        protected TaskModel[] tasksToShow;
        protected TaskModel[] allTasks;
        protected bool isLoaded;
        protected bool showLister;
        protected bool showCreator;

        [Inject]
        public IMemberDataService MemberDataService { get; set; }

        [Inject]
        public ITaskDataService TaskDataService { get; set; }

        [Inject]
        public HttpClient Http { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var result = (await MemberDataService.GetAllMembers()).Payload.ToList();
            var allTaskresults = (await TaskDataService.GetAllTasks()).TasksList.ToList();

            LoadTasks(result, allTaskresults);
            
            leftMenuItem.Add(new MenuItem
            {
                Label = "All Tasks",
                ReferenceId = Guid.Empty,
                IsActive = true
            });
            leftMenuItem[0].ClickCallback += showAllTasks;
            for (int i = 0; i < result.Count; i++)
            {
                leftMenuItem.Add(new MenuItem
                {
                    IconColor = result[i].Avatar,
                    Label = result[i].FirstName,
                    ReferenceId = result[i].Id,
                    IsActive = false
                });
                leftMenuItem[i + 1].ClickCallback += onItemClick;
            }
            showAllTasks(null, leftMenuItem[0]);
            isLoaded = true;
        }

        private void LoadTasks(List<MemberVm> result, List<TaskVm> allTaskresults)
        {
            List<TaskModel> list = new List<TaskModel>();

            for (int i = 0; i < allTaskresults.Count; i++)
            {
                var member = new FamilyMember();
                for (int j = 0; j < result.Count; j++)
                {
                    if (result[j].Id == allTaskresults[i].AssignedToId)
                    {
                        member.Id = result[j].Id;
                        member.Firstname = result[j].FirstName;
                        member.Lastname = result[j].LastName;
                        member.Email = result[j].Email;
                        member.Role = result[j].Roles;
                        member.Avatar = result[j].Avatar;
                    }
                }
                var task = new TaskModel
                {
                    Id = allTaskresults[i].Id,
                    Text = allTaskresults[i].Subject,
                    IsDone = allTaskresults[i].IsComplete,
                    Member = member
                };
                list.Add(task);
            }

            allTasks = list.ToArray();
        }

        protected void OnAddItem()
        {
            showLister = false;
            showCreator = true;
            makeMenuItemActive(null);
            StateHasChanged();
        }

        private void onItemClick(object sender, object e)
        {
            Guid val = (Guid)e.GetType().GetProperty("ReferenceId").GetValue(e);
            makeMenuItemActive(e);
            if (allTasks != null && allTasks.Length > 0)
            {
                tasksToShow = allTasks.Where(item =>
                {
                    if (item.Member != null)
                    {
                        return item.Member.Id == val;
                    }
                    else
                    {
                        return false;
                    }
                }).ToArray();
            }
            showLister = true;
            showCreator = false;
            StateHasChanged();
        }
        private void showAllTasks(object sender, object e)
        {
            tasksToShow = allTasks;
            showLister = true;
            showCreator = false;
            makeMenuItemActive(e);
            StateHasChanged();
        }

        private void makeMenuItemActive(object e)
        {
            foreach (var item in leftMenuItem)
            {
                item.IsActive = false;
            }
            if (e != null)
            {
                e.GetType().GetProperty("IsActive").SetValue(e, true);
            }
        }

        protected async Task onMemberAdd(FamilyMember familyMember)
        {
            var result = await MemberDataService.Create(new Domain.Commands.CreateMemberCommand()
            {
                Avatar = familyMember.Avatar,
                FirstName = familyMember.Firstname,
                LastName = familyMember.Lastname,
                Email = familyMember.Email,
                Roles = familyMember.Role
            });

            if (result != null && result.Payload != null && result.Payload.Id != Guid.Empty)
            {
                members.Add(new FamilyMember()
                {
                    Avatar = result.Payload.Avatar,
                    Email = result.Payload.Email,
                    Firstname = result.Payload.FirstName,
                    Lastname = result.Payload.LastName,
                    Role = result.Payload.Roles,
                    Id = result.Payload.Id
                });

                leftMenuItem.Add(new MenuItem
                {
                    IconColor = result.Payload.Avatar,
                    Label = result.Payload.FirstName,
                    ReferenceId = result.Payload.Id
                });

                showCreator = false;
                StateHasChanged();
            }
        }
        }
    }
}
}
