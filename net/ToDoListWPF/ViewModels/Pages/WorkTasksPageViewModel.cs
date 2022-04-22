using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ToDoList.Database;
using System.Windows;
using System.Windows.Controls;
namespace ToDoListWPF
{
        /// <summary>
        /// Klasa zawierająca w sobie logike funkcjonalści znajdujących się w aplikacji okienkowej 
        /// </summary>
        /// 
    public class WorkTasksPageViewModel : BaseViewModel
    {


        /// <summary>
        /// Utworzenie Listy wyświetlajacej nasz taski
        /// </summary>
        public ObservableCollection<WorkTaskViewModel> WorkTaskList { get; set; } = new ObservableCollection<WorkTaskViewModel>();

        public string NewWorkTaskTitle { get; set; }

        public string NewWorkTaskDescription { get; set; }
        public ICommand AddNewTaskCommend { get; set; }
        public ICommand DeleteSelectedTasksCommand { get; set; }
        public ICommand EditSelectedTasksCommand { get; set; }
        public static DateTime SelectDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Załadowanie do listy gdzei wyświetlane są nasze taski tasków z bazy danych
        /// </summary>
        public WorkTasksPageViewModel()
        {
            AddNewTaskCommend = new RelayCommand(AddNewTask);
            DeleteSelectedTasksCommand = new RelayCommand(DeleteSelectedTasks);
            EditSelectedTasksCommand = new RelayCommand(EditSelectedTasks);

            TaskListDBEntities db = new TaskListDBEntities();


            foreach (var task in db.Tasks.ToList())
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

        /// <summary>
        /// Funkcja pozwalająca dodawać nowe taski do wyswietlanej listy i bazy danych
        /// </summary>
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

        /// <summary>
        /// Funkcja pozwalająca edytowac wybrane taski z listy i bazy danych
        /// </summary>
        private void EditSelectedTasks()
        {
            TaskListDBEntities db = new TaskListDBEntities();

            var selectedTasks = WorkTaskList.Where(x => x.IsSelected).ToList();

            string tmpTitle;
            string tmpDesc;

            foreach (var task in selectedTasks)
            {

               tmpTitle = task.Title;
               tmpDesc = task.Description;
               WorkTaskList.Remove(task);

                var foundEntity = db.Tasks.FirstOrDefault(x => x.Id == task.Id);
                if (foundEntity != null)
                {
                    if (NewWorkTaskTitle == string.Empty)
                    {
                        task.Title = tmpTitle;
                    }
                    else { 
                        task.Title = NewWorkTaskTitle; 
                    }
                
                    task.Description = NewWorkTaskDescription;
                    task.CreatedDate = SelectDate;
                    db.Tasks.Remove(foundEntity);

                }

                

                Task taskObject = new Task()
                {
                    Title = NewWorkTaskTitle,
                    Description = NewWorkTaskDescription,
                    CreatedDate = SelectDate
                };
                db.Tasks.Add(taskObject);
                WorkTaskList.Add(task);

            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie wszystkie pola uzupełnione");
            }

            /*
            var newTask = new WorkTaskViewModel
            {
                Title = tmpTitle,
                Description = tmpDesc,
                CreatedDate = SelectDate
            };
            if (tmpTitle == string.Empty)
            {
                newTask.Title = NewWorkTaskTitle;
            }
            if (tmpDesc == string.Empty)
            {
                newTask.Description = NewWorkTaskDescription;
            }
            WorkTaskList.Add(newTask);

            Task taskObject = new Task()
            {
                Title = tmpTitle,
                Description = tmpDesc,
                CreatedDate = SelectDate
            };
            if (tmpTitle == string.Empty)
            {
                taskObject.Title = NewWorkTaskTitle;
            }
            if (tmpDesc == string.Empty)
            {
                taskObject.Description = NewWorkTaskDescription;
            }

            db.Tasks.Add(taskObject);
            */
            //db.SaveChanges();
        }




        /// <summary>
        /// Funkcja pozwalająca usunąć wybrane taski z listy i bazy danych
        /// </summary>
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
