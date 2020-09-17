namespace WebClient.Pages
{
    using Domain.ViewModel;
    using Microsoft.AspNetCore.Components;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
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
        protected Guid? memberId;

        [Inject]
        public IMemberDataService MemberDataService { get; set; }

        [Inject]
        public ITaskDataService TaskDataService { get; set; }

        [Inject]
        public HttpClient Http { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var result = (await MemberDataService.GetAllMembers()).Payload.ToList();
            var allTaskResults = (await TaskDataService.GetAllTasks()).TasksList.ToList();

            LoadTasks(result, allTaskResults);

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

        /// <summary>
        /// Loads the tasks.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="allTaskResults">All task results.</param>
        private void LoadTasks(List<MemberVm> result, List<TaskVm> allTaskResults)
        {
            for (int m = 0; m < result.Count; m++)
            {
                members.Add(new FamilyMember
                {
                    Avatar = result[m].Avatar,
                    Email = result[m].Email,
                    Firstname = result[m].FirstName,
                    Lastname = result[m].LastName,
                    Role = result[m].Roles,
                    Id = result[m].Id
                });

            }
            List<TaskModel> list = new List<TaskModel>();

            for (int i = 0; i < allTaskResults.Count; i++)
            {
                var member = new FamilyMember();
                for (int j = 0; j < result.Count; j++)
                {
                    if (result[j].Id == allTaskResults[i].AssignedToId)
                    {
                        member = members.First(x => x.Id == allTaskResults[i].AssignedToId);
                    }
                }
                var task = new TaskModel
                {
                    Id = allTaskResults[i].Id,
                    Text = allTaskResults[i].Subject,
                    IsDone = allTaskResults[i].IsComplete,
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
            memberId = val;
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
            memberId = null;
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

        public async Task onTaskAdd(TaskModel task)
        {
            var result = await TaskDataService.Create(new Domain.Commands.CreateTaskCommand()
            {
                Subject = task.Text,
                AssignedToId = memberId,
                IsComplete = task.IsDone
            });

            if (result != null && result.TasksList != null && result.TasksList.Id != Guid.Empty)
            {
                var familyMember = memberId == null ? null : members.First(x => x.Id == memberId);
                var taskModel = new TaskModel
                {
                    Id = result.TasksList.Id,
                    Text = result.TasksList.Subject,
                    IsDone = result.TasksList.IsComplete,
                    Member = familyMember
                };

                allTasks = allTasks.Prepend(taskModel).ToArray();

                if (memberId == null)
                {
                    showAllTasks(null, leftMenuItem[0]);
                }
                else
                {
                    tasksToShow = allTasks.Where(item =>
                    {
                        if (item.Member != null)
                        {
                            return item.Member.Id == memberId;
                        }
                        else
                        {
                            return false;
                        }
                    }).ToArray();
                }
                showCreator = false;
                task.Text = string.Empty;
                StateHasChanged();
            }
        }
    }
}