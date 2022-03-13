using System.Linq;
using TaskManager.Models;

namespace TaskManager.Repositories
{
    public interface ITaskRepository
    {
        TaskModel Get(int TaskID);
        IQueryable<TaskModel> GetAllActive();
        void Add(TaskModel task);
        void Update(int taskID, TaskModel task);
        void Delete(int taskID);

    }
}
