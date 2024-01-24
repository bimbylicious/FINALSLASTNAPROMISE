namespace FINALSLASTNAPROMISE
{
    public class TaskItem
    {
        public string TaskName { get; }
        public string TaskDetails { get; }
        public string CreationTime { get; }
        public string AssignedTo { get; set; } // Make settable
        public string AssignmentTime { get; set; } // Make settable
        public string CompletionTime { get; set; } // Make settable
        public string Status { get; }
        public string Comments { get; set; }

        public TaskItem(string taskName, string taskDetails, string creationTime, string assignedTo, string assignmentTime, string completionTime, string status, string comments)
        {
            TaskName = taskName;
            TaskDetails = taskDetails;
            CreationTime = creationTime;
            AssignedTo = assignedTo;
            AssignmentTime = assignmentTime;
            CompletionTime = completionTime;
            Status = status;
            Comments = comments;
        }

        public override string ToString()
        {
            return $"{TaskName},{TaskDetails},{CreationTime},{AssignedTo},{AssignmentTime},{CompletionTime},{Status},{Comments}";
        }
    }
}