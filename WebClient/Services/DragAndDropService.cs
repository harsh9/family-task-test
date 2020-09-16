using System;

namespace WebClient.Services
{
    public class DragAndDropService
    {
        public TaskModel Data { get; set; }

        public void StartDrag(TaskModel data)
        {
            Data = data;
        }

        public bool Accepts(MenuItem zone)
        {
            var output = (zone.ReferenceId != Guid.Empty && Data.Member == null)
                || (zone.ReferenceId == Guid.Empty && Data.Member != null)
                || (zone.ReferenceId != Guid.Empty && Data.Member != null && Data.Member.Id != zone.ReferenceId);

            return output;
        }
    }
}
