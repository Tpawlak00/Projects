using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ToDoList.Database;

namespace ToDoList.Core
{
    public class WorkTasksPageViewModel : BaseViewModel
    {

        public ObservableCollection<WorkTaskViewModel>  WorkTaskList { get; set; } = new ObservableCollection<WorkTaskViewModel>();

        public string NewWorkTaskTitle { get; set; }

        public string NewWorkTaskDescription { get; set; }
        public DateTime NewWorkTaskStartDate { get; set; }

        public ICommand AddNewTaskCommend { get; set; }
        public ICommand DeleteSelectedTasksCommand { get; set; }
        public WorkTasksPageViewModel()
        {
            AddNewTaskCommend = new RelayCommand(AddNewTask);
            DeleteSelectedTasksCommand = new RelayCommand(DeleteSelectedTasks);

            TaskToDoListDBEntities db = new TaskToDoListDBEntities();

            
            foreach ( var task in db.Task1.ToList())
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
            var newTask = new WorkTaskViewModel
            {
                Title = NewWorkTaskTitle,
                Description = NewWorkTaskDescription,
                CreatedDate = NewWorkTaskStartDate  //DateTime.Now
            };
            WorkTaskList.Add(newTask);

            //TaskToDoListDBEntities db = new TaskToDoListDBEntities();

           /* Task1 taskObject = new Task1()
            {
                Title = "kolacja",
                Description = "costam",
                CreatedDate = DateTime.Now
            };*/

           // db.Task1.Add(taskObject);
           // db.SaveChanges();
            /*
            DatabaseLocation.Database.WorkTasks.Add(new WorkTask
            {
                Title = newTask.Title,
                Description = newTask.Description,
                CreatedDate = newTask.CreatedDate
            });

            DatabaseLocation.Database.SaveChanges();*/

            NewWorkTaskTitle = string.Empty;
            NewWorkTaskDescription = string.Empty;

            
         
        }

        private void DeleteSelectedTasks()
        {
            var selectedTasks = WorkTaskList.Where(x => x.IsSelected).ToList();

            foreach (var task in selectedTasks)
            {
                WorkTaskList.Remove(task);

                /*var foundEntity =  DatabaseLocation.Database.WorkTasks.FirstOrDefault(x=>x.Id == task.Id);
                
                if (foundEntity != null)
                {
                    DatabaseLocation.Database.WorkTasks.Remove(foundEntity);
                }*/
            
            
            }
            //DatabaseLocation.Database.SaveChanges();
        }
    }
}
