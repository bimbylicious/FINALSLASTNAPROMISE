using System;
using System.Collections.Generic;
using System.IO;

namespace FINALSLASTNAPROMISE
{
    public class TaskItem
    {
        public string TaskName { get; }
        public string TaskDetails { get; }
        public string CreationTime { get; }
        public string Deadline { get; }
        public string Comments { get; set; } // Make 'Comments' property settable
        public string Status { get; set; }
        public string StatusDetails { get; set; }
        public string StatusDate { get; set; }

        public TaskItem(string taskName, string taskDetails, string creationTime, string deadline, string comments, string status)
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
            return $"{TaskName},{TaskDetails},{CreationTime},{Deadline},{StatusDetails},{Status},{Comments}";
        }
    }
}