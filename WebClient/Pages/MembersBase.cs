using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebClient.Abstractions;

namespace WebClient.Pages
{
    public class MembersBase: ComponentBase
    {       
        protected List<FamilyMember> members = new List<FamilyMember>();
        protected List<MenuItem> leftMenuItem = new List<MenuItem>();

        protected bool showCreator;
        protected bool isLoaded;

        [Inject]
        public IMemberDataService MemberDataService { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            var result = await MemberDataService.GetAllMembers();

            if (result != null && result.Payload != null && result.Payload.Any())
            {                
                foreach (var item in result.Payload)
                {                    
                    members.Add(new FamilyMember()
                    {
                        Avatar = item.Avatar,
                        Email = item.Email,
                        Firstname = item.FirstName,
                        Lastname = item.LastName,
                        Role = item.Roles,
                        Id = item.Id
                    });
                }
            }
         
            for (int i = 0; i < members.Count; i++)
            {
                leftMenuItem.Add(new MenuItem
                {
                    IconColor = members[i].Avatar,
                    Label = members[i].Firstname,
                    ReferenceId = members[i].Id
                });
            }
            showCreator = true;
            isLoaded = true;
        }
       
        protected void onAddItem()
        {
            showCreator = true;
            StateHasChanged();
        }

        protected async Task onMemberAdd(FamilyMember familyMember)
        {
           var result = await  MemberDataService.Create(new Domain.Commands.CreateMemberCommand()
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
