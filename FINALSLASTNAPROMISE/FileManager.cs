using System;
using System.Collections.Generic;
using System.IO;

namespace FINALSLASTNAPROMISE
{
    internal class FileManager
    {
        private const string TaskFilesDirectory = "TaskFiles.csv";

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

                File.WriteAllLines(TaskFilesDirectory, lines);
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
                if (File.Exists(TaskFilesDirectory))
                {
                    string[] lines = File.ReadAllLines(TaskFilesDirectory);

                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');

                        if (parts.Length == 7)
                        {
                            string taskName = parts[0];
                            string taskDetails = parts[1];
                            string creationTime = parts[2];
                            string deadline = parts[3];
                            string details = parts[4];
                            string taskStatus = parts[5];
                            string comments = parts[6];

                            TaskItem loadedTask;

                            if (taskStatus.ToLower() == "assigned" || taskStatus.ToLower() == "completed")
                            {
                                string[] assignedDetails = details.Split(',');
                                string statusDetails = assignedDetails.Length > 0 ? assignedDetails[0] : "";
                                string statusDate = assignedDetails.Length > 1 ? assignedDetails[1] : "";

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
    }
}