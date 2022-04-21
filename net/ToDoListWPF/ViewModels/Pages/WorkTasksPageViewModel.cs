using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ToDoList.Database;
using System.Windows;
using System.Windows.Controls;
namespace ToDoListWPF
{
    public class WorkTasksPageViewModel : BaseViewModel
    {

        public ObservableCollection<WorkTaskViewModel>  WorkTaskList { get; set; } = new ObservableCollection<WorkTaskViewModel>();

        public string NewWorkTaskTitle { get; set; }

        public string NewWorkTaskDescription { get; set; }
        public ICommand AddNewTaskCommend { get; set; }
        public ICommand DeleteSelectedTasksCommand { get; set; }
        public static DateTime SelectDate { get; set; } = DateTime.Now;

        public WorkTasksPageViewModel()
        {
            AddNewTaskCommend = new RelayCommand(AddNewTask);
            DeleteSelectedTasksCommand = new RelayCommand(DeleteSelectedTasks);


            TaskListDBEntities db = new TaskListDBEntities();


            foreach ( var task in db.Tasks.ToList())
            {
                WorkTaskList.Add(new WorkTaskViewModel
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    CreatedDate = task.CreatedDate
                });
            }


        }

        private void AddNewTask()
        {
            TaskListDBEntities db = new TaskListDBEntities();

            var newTask = new WorkTaskViewModel
            {
                Title = NewWorkTaskTitle,
                Description = NewWorkTaskDescription,
                CreatedDate = SelectDate
            };
            if (newTask.Title == string.Empty)
            {
                MessageBox.Show("Please name your task");
            }
            else
            {
                WorkTaskList.Add(newTask);

                Task taskObject = new Task()
                {
                    Title = NewWorkTaskTitle,
                    Description = NewWorkTaskDescription,
                    CreatedDate = SelectDate
                };

                db.Tasks.Add(taskObject);

            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }


            NewWorkTaskTitle = string.Empty;
            NewWorkTaskDescription = string.Empty;
            



        }

        private void DeleteSelectedTasks()
        {
            TaskListDBEntities db = new TaskListDBEntities();

            var selectedTasks = WorkTaskList.Where(x => x.IsSelected).ToList();

            foreach (var task in selectedTasks)
            {
                WorkTaskList.Remove(task);

                var foundEntity =  db.Tasks.FirstOrDefault(x=>x.Id == task.Id);
                
                if (foundEntity != null)
                {
                    db.Tasks.Remove(foundEntity);
                }
            
            
            }
            db.SaveChanges();
        }
    }
}
