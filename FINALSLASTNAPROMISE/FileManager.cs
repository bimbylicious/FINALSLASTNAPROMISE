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

                // Header line
                lines.Add("TaskName,TaskDetails,CreationTime,AssignedTo,AssignmentTime,CompletionTime,TaskStatus,Comments");

                foreach (TaskItem task in tasks)
                {
                    string assignedDetails = task.Status.ToLower() == "assigned"
                        ? $"{task.AssignedTo},{task.AssignmentTime}"
                        : "";

                    string completionTime = task.CompletionTime == null ? "" : task.CompletionTime;

                    string line = $"{task.TaskName},{task.TaskDetails},{task.CreationTime},{assignedDetails},{completionTime},{task.Status},{task.Comments}";
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

                    // Skip the first line (header)
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string line = lines[i];
                        string[] parts = line.Split(',');

                        if (parts.Length == 8) // Adjust the length based on the number of columns
                        {
                            string taskName = parts[0];
                            string taskDetails = parts[1];
                            string creationTime = parts[2];
                            string assignedTo = parts[3];
                            string assignmentTime = parts[4];
                            string completionTime = parts[5];
                            string taskStatus = parts[6];
                            string comments = parts[7];

                            TaskItem loadedTask = new TaskItem(taskName, taskDetails, creationTime, assignedTo, assignmentTime, completionTime, comments, taskStatus);
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

        public void Writer(List<TaskItem> Task)
        {
            using (StreamWriter sr = new StreamWriter(TaskFilesDirectory))
            {
                for (int x = 0; x < Task.Count; x++)
                {
                    sr.Write(Task[x]);
                }
            }
        }
    }
}