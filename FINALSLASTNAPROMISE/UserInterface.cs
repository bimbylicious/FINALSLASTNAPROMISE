using System.Collections.Generic;
using System;

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

            string creationTime = DateTime.Now.ToString(); // Timestamp when the task was created

            Console.Write("Enter who the task is assigned to: ");
            string assignedTo = Console.ReadLine();

            string assignmentTime = ""; // Initialize as an empty string
            string completionTime = ""; // Initialize as an empty string

            // If the task is assigned, capture the assignment time
            if (assignedTo.Length > 0)
            {
                assignmentTime = DateTime.Now.ToString();
            }

            // If the task is completed, capture the completion time
            Console.Write("Is the task completed? (yes/no): ");
            string isCompleted = Console.ReadLine().ToLower();

            if (isCompleted == "yes")
            {
                completionTime = DateTime.Now.ToString();
            }

            Console.Write("Enter the task status (Open/Assigned/For Verification/For Revision/Closed): ");
            string status = Console.ReadLine();

            Console.Write("Enter comments: ");
            string comments = Console.ReadLine();

            TaskItem task = new TaskItem(taskName, taskDetails, creationTime, assignedTo, assignmentTime, completionTime, status, comments);
            tasks.Add(task);
            fileManager.Writer(tasks);
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
                    DisplayTaskDetails(tasks);
                    break;

                case "2":
                    DisplayTaskDetailsByStatus("Open");
                    break;

                case "3":
                    DisplayTaskDetailsByStatus("Assigned");
                    break;

                case "4":
                    DisplayTaskDetailsByStatus("Closed");
                    break;

                default:
                    Console.WriteLine("Invalid option. Returning to the main menu.");
                    break;
            }
        }

        private void DisplayTaskDetails(List<TaskItem> tasksToDisplay)
        {
            Console.WriteLine("Task Details:");
            for (int i = 0; i < tasksToDisplay.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasksToDisplay[i]}");
            }

            Console.Write("Enter the number of the task to view details (0 to go back): ");
            string userInput = Console.ReadLine();

            if (int.TryParse(userInput, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= tasksToDisplay.Count)
            {
                ViewTaskDetails(tasksToDisplay[selectedIndex - 1]);
            }
        }

        private void DisplayTaskDetailsByStatus(string status)
        {
            List<TaskItem> filteredTasks = tasks.FindAll(task => task.Status == status);
            Console.WriteLine($"{status} Task Details:");
            DisplayTaskDetails(filteredTasks);
        }

        private void ViewTaskDetails(TaskItem task)
        {
            Console.WriteLine($"Task Details for {task.TaskName}:");
            Console.WriteLine($"Task Details: {task.TaskDetails}");
            Console.WriteLine($"Creation Time: {task.CreationTime}");
            Console.WriteLine($"Assigned To: {task.AssignedTo}");
            Console.WriteLine($"Assignment Time: {task.AssignmentTime}");
            Console.WriteLine($"Completion Time: {task.CompletionTime}");
            Console.WriteLine($"Status: {task.Status}");
            Console.WriteLine($"Comments: {task.Comments}");

            if (task.Status == "Open")
            {
                Console.Write("Do you want to assign this task to someone? (yes/no): ");
                string assignInput = Console.ReadLine().ToLower();

                if (assignInput == "yes")
                {
                    AssignTask(task);
                }
            }
        }

        private void AssignTask(TaskItem task)
        {
            Console.Write("Enter the name of the person or team to whom you want to assign the task: ");
            string assignedTo = Console.ReadLine();

            // Update task details
            task.AssignedTo = assignedTo;
            task.AssignmentTime = DateTime.Now.ToString();

            Console.WriteLine("Task assigned successfully!");
        }
    }
}