using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FINALSLASTNAPROMISE
{
    internal class FileManager
    {
        private string TaskFilesDirectory => AppDomain.CurrentDomain.BaseDirectory;

        private string GetTaskFilePath(string taskStatus)
        {
            // Combine the base directory with the task status
            return Path.Combine(TaskFilesDirectory, $"{taskStatus.ToLower()}_tasks.csv");
        }

        public void SaveTasks(List<TaskItem> tasks)
        {
            try
            {
                // Get distinct statuses from the tasks
                IEnumerable<string> distinctStatuses = tasks.Select(task => task.Status).Distinct();

                foreach (string status in distinctStatuses)
                {
                    List<string> lines = new List<string>();

                    // Header line
                    lines.Add("TaskName,TaskDetails,CreationTime,AssignedTo,AssignmentTime,CompletionTime,TaskStatus,Comments");

                    IEnumerable<TaskItem> tasksByStatus = tasks.Where(task => task.Status == status);

                    foreach (TaskItem task in tasksByStatus)
                    {
                        string assignedDetails = task.Status.ToLower() == "assigned"
                            ? $"{task.AssignedTo},{task.AssignmentTime}"
                            : ",,"; // Empty placeholders for AssignedTo and AssignmentTime

                        string completionTime = task.CompletionTime ?? ""; // Use null-coalescing operator

                        // Ensure all values are properly formatted
                        string taskName = FormatCsvValue(task.TaskName);
                        string taskDetails = FormatCsvValue(task.TaskDetails);
                        string creationTime = FormatCsvValue(task.CreationTime);
                        string taskStatus = FormatCsvValue(task.Status);
                        string comments = FormatCsvValue(task.Comments);

                        string line = $"{taskName},{taskDetails},{creationTime},{assignedDetails},{completionTime},{taskStatus},{comments}";
                        lines.Add(line);
                    }

                    // Use the GetTaskFilePath method to get the file path based on status
                    string filePath = GetTaskFilePath(status);

                    // Check if the directory exists, create if not
                    string directoryPath = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    File.WriteAllLines(filePath, lines);
                    Console.WriteLine($"Tasks with status '{status}' saved to file.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving tasks to file: {ex.Message}");
            }
        }

        // Helper method to format CSV values
        private string FormatCsvValue(string value)
        {
            // If the value contains commas, wrap it in double quotes
            if (value.Contains(","))
            {
                return $"\"{value}\"";
            }
            return value;
        }

        public List<TaskItem> LoadTasks(string status = null)
        {
            List<TaskItem> loadedTasks = new List<TaskItem>();

            try
            {
                if (status != null)
                {
                    string filePath = GetTaskFilePath(status);

                    // Check if the directory exists, create if not
                    string directoryPath = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    if (File.Exists(filePath))
                    {
                        string[] lines = File.ReadAllLines(filePath);

                        // Skip the first line (header)
                        for (int i = 1; i < lines.Length; i++)
                        {
                            string line = lines[i];
                            string[] parts = line.Split(',');

                            if (parts.Length == 8)
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

                        Console.WriteLine($"Tasks with status '{status}' loaded from file.");
                    }
                    else
                    {
                        Console.WriteLine($"No existing tasks file found for status '{status}'.");
                    }
                }
                else
                {
                    // Load tasks from all CSV files
                    var allStatuses = Enum.GetValues(typeof(TaskStatus)).Cast<TaskStatus>().Select(s => s.ToString().ToLower());

                    foreach (var file in Directory.GetFiles(TaskFilesDirectory, "*.csv"))
                    {
                        string currentStatus = allStatuses.FirstOrDefault(s => file.EndsWith($"{s}_tasks.csv"));
                        if (currentStatus != null)
                        {
                            loadedTasks.AddRange(LoadTasks(currentStatus));
                        }
                    }

                    Console.WriteLine("All tasks loaded from files.");
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
            foreach (var status in Task.Select(task => task.Status).Distinct())
            {
                var tasksByStatus = Task.Where(task => task.Status == status);

                // Check if the directory exists, create if not
                string directoryPath = Path.GetDirectoryName(GetTaskFilePath(status));
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (StreamWriter sr = new StreamWriter(GetTaskFilePath(status)))
                {
                    foreach (TaskItem task in tasksByStatus)
                    {
                        sr.Write(task);
                    }
                }
            }
        }
    }
}