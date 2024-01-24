using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FINALSLASTNAPROMISE
{
    public class TaskItem
    {
        public string TaskName { get; }
        public string TaskDetails { get; }
        public DateTime CreationTime { get; }
        public DateTime Deadline { get; }
        public string Comments { get; set; } // Make 'Comments' property settable
        public string Status { get; set; }
        public string StatusDetails { get; set; }
        public DateTime StatusDate { get; set; }

        public TaskItem(string taskName, string taskDetails, DateTime creationTime, DateTime deadline, string comments, string status)
        {
            TaskName = taskName;
            TaskDetails = taskDetails;
            CreationTime = creationTime;
            Deadline = deadline;
            Comments = comments;
            Status = status;
        }

        public override string ToString()
        {
            return $"{TaskName} - Created: {CreationTime}, Deadline: {Deadline}, Status: {Status}, Comments: {Comments}";
        }
    }
}
