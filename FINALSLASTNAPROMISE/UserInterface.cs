using System;
using System.Collections.Generic;
using System.Linq;

namespace FINALSLASTNAPROMISE
{
    internal class UserInterface
    {
        private List<TaskItem> tasks = new List<TaskItem>();
        private FileManager fileManager = new FileManager();

        public void Start()
        {
            tasks = fileManager.LoadTasks();

            bool exitProgram = false;

            while (!exitProgram)
            {
                Console.WriteLine("1. Create Task");
                Console.WriteLine("2. View Tasks");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option (1-3): ");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        CreateTask();
                        break;

                    case "2":
                        ViewTasks();
                        break;

                    case "3":
                        fileManager.SaveTasks(tasks);
                        exitProgram = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please choose again.");
                        break;
                }

                Console.WriteLine();
            }

            Console.WriteLine("Program exited. Press any key to close.");
            Console.ReadKey();
        }

        private void CreateTask()
        {
            Console.Write("Enter the task name: ");
            string taskName = Console.ReadLine();

            Console.Write("Enter the task details: ");
            string taskDetails = Console.ReadLine();

            Console.Write("Enter the creation time (e.g., 2024-01-01 12:00:00): ");
            string creationTime = Console.ReadLine();

            Console.Write("Enter the deadline (e.g., 2024-12-31 23:59:59): ");
            string deadline = Console.ReadLine();

            Console.Write("Enter comments: ");
            string comments = Console.ReadLine();

            Console.Write("Enter the status (Open/Assigned/Completed): ");
            string status = Console.ReadLine();

            TaskItem task = new TaskItem(taskName, taskDetails, creationTime, deadline, comments, status);
            tasks.Add(task);
            fileManager.SaveTasks(tasks);

            Console.WriteLine("Task created successfully!");
        }

        private void ViewTasks()
        {
            Console.WriteLine("1. View All Tasks");
            Console.WriteLine("2. View Open Tasks");
            Console.WriteLine("3. View Assigned Tasks");
            Console.WriteLine("4. View Completed Tasks");
            Console.Write("Choose an option (1-4): ");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    DisplayTaskNames(tasks);
                    break;

                case "2":
                    DisplayTaskNamesByStatus("Open");
                    break;

                case "3":
                    DisplayTaskNamesByStatus("Assigned");
                    break;

                case "4":
                    DisplayTaskNamesByStatus("Completed");
                    break;

                default:
                    Console.WriteLine("Invalid option. Returning to the main menu.");
                    break;
            }
        }

        private void DisplayTaskNames(List<TaskItem> tasksToDisplay)
        {
            Console.WriteLine("Task Names:");
            for (int i = 0; i < tasksToDisplay.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasksToDisplay[i].TaskName}");
            }

            Console.Write("Enter the number of the task to view details (0 to go back): ");
            string userInput = Console.ReadLine();

            if (int.TryParse(userInput, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= tasksToDisplay.Count)
            {
                ViewTaskDetails(tasksToDisplay[selectedIndex - 1]);
            }
        }

        private void DisplayTaskNamesByStatus(string status)
        {
            List<TaskItem> filteredTasks = tasks.FindAll(task => task.Status == status);
            Console.WriteLine($"{status} Task Names:");
            DisplayTaskNames(filteredTasks);
        }

        private void ViewTaskDetails(TaskItem task)
        {
            Console.WriteLine($"Task Details for {task.TaskName}:");
            Console.WriteLine($"Task Details: {task.TaskDetails}");
            Console.WriteLine($"Creation Time: {task.CreationTime}");
            Console.WriteLine($"Deadline: {task.Deadline}");
            Console.WriteLine($"Status: {task.Status}");
            Console.WriteLine($"Comments: {task.Comments}");

            if (task.Status == "Open")
            {
                Console.Write("Do you want to assign this task to yourself? (yes/no): ");
                string assignInput = Console.ReadLine().ToLower();

                if (assignInput == "yes")
                {
                    AssignTask(task);
                }
            }
            else if (task.Status == "Assigned")
            {
                Console.Write("Is the task now completed? (yes/no): ");
                string completeInput = Console.ReadLine().ToLower();

                if (completeInput == "yes")
                {
                    CompleteTask(task);
                }
            }
        }

        private void DisplayTasks(List<TaskItem> tasksToDisplay)
        {
            Console.WriteLine("Tasks:");
            for (int i = 0; i < tasksToDisplay.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasksToDisplay[i]}");
            }
        }

        private void DisplayTasksByStatus(string status)
        {
            List<TaskItem> filteredTasks = tasks.FindAll(task => task.Status == status);
            Console.WriteLine($"{status} Tasks:");
            DisplayTasks(filteredTasks);
        }

        private void AssignTask(TaskItem task)
        {
            Console.Write("Enter your name: ");
            string userName = Console.ReadLine();

            Console.Write("Enter the estimated date of completion (e.g., 2024-12-31 23:59:59): ");
            string completionDate = Console.ReadLine();

            // Update task details
            task.Status = "Assigned";
            task.Comments += $"\nAssigned to: {userName}\nEstimated Completion Date: {completionDate}";

            Console.WriteLine("Task assigned successfully!");
        }

        private void CompleteTask(TaskItem task)
        {
            // Update task details
            task.Status = "Completed";
            task.Comments += $"\nCompleted on: {DateTime.Now}";

            Console.WriteLine("Task marked as completed successfully!");
        }
    }
}