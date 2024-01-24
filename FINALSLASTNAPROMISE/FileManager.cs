using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FINALSLASTNAPROMISE
{
    internal class FileManager
    {
        private const string TaskFilesDirectory = "TaskFiles";

        public void SaveTasks(List<TaskItem> tasks)
        {
            try
            {
                List<string> lines = new List<string>();

                foreach (TaskItem task in tasks)
                {
                    string statusDetails = task.Status.ToLower() == "assigned"
                        ? $"{task.StatusDetails},{task.StatusDate}"
                        : "";

                    string line = $"{task.TaskName},{task.TaskDetails},{task.CreationTime},{task.Deadline},{statusDetails},{task.Status},{task.Comments}";
                    lines.Add(line);
                }

                File.WriteAllLines("tasks.csv", lines);
                Console.WriteLine("Tasks saved to file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving tasks to file: {ex.Message}");
            }
        }

        public List<TaskItem> LoadTasks()
        {
            List<TaskItem> loadedTasks = new List<TaskItem>();

            try
            {
                if (File.Exists("tasks.csv"))
                {
                    string[] lines = File.ReadAllLines("tasks.csv");

                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');

                        if (parts.Length == 7)
                        {
                            string taskName = parts[0];
                            string taskDetails = parts[1];
                            if (DateTime.TryParse(parts[2], out DateTime creationTime) &&
                                DateTime.TryParse(parts[3], out DateTime deadline))
                            {
                                string details = parts[4];
                                string taskStatus = parts[5];
                                string comments = parts[6];

                                TaskItem loadedTask;

                                if (taskStatus.ToLower() == "assigned" || taskStatus.ToLower() == "completed")
                                {
                                    string[] assignedDetails = details.Split(',');
                                    string statusDetails = assignedDetails.Length > 0 ? assignedDetails[0] : "";
                                    DateTime statusDate = assignedDetails.Length > 1 ? DateTime.Parse(assignedDetails[1]) : DateTime.MinValue;

                                    loadedTask = new TaskItem(taskName, taskDetails, creationTime, deadline, comments, taskStatus)
                                    {
                                        StatusDetails = statusDetails,
                                        StatusDate = statusDate
                                    };
                                }
                                else
                                {
                                    loadedTask = new TaskItem(taskName, taskDetails, creationTime, deadline, comments, taskStatus);
                                }

                                loadedTasks.Add(loadedTask);
                            }
                        }
                    }

                    Console.WriteLine("Tasks loaded from file.");
                }
                else
                {
                    Console.WriteLine("No existing tasks file found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tasks from file: {ex.Message}");
            }

            return loadedTasks;
        }

        private TaskItem CreateTaskFromParts(string[] parts)
        {
            if (DateTime.TryParse(parts[2], out DateTime creationTime) &&
                DateTime.TryParse(parts[3], out DateTime deadline))
            {
                string details = parts[4];
                string taskStatus = parts[5];
                string comments = parts[6];

                if (taskStatus.ToLower() == "assigned" || taskStatus.ToLower() == "completed")
                {
                    string[] assignedDetails = details.Split(',');
                    string statusDetails = assignedDetails.Length > 0 ? assignedDetails[0] : "";
                    DateTime statusDate = assignedDetails.Length > 1 ? DateTime.Parse(assignedDetails[1]) : DateTime.MinValue;

                    return new TaskItem(parts[0], parts[1], creationTime, deadline, comments, taskStatus)
                    {
                        StatusDetails = statusDetails,
                        StatusDate = statusDate
                    };
                }
                else
                {
                    return new TaskItem(parts[0], parts[1], creationTime, deadline, comments, taskStatus);
                }
            }

            return null;
        }

        private string GetFilePath(string status)
        {
            string fileName = $"{status.ToLower()}_tasks.csv";
            return Path.Combine(TaskFilesDirectory, fileName);
        }
    }
}
