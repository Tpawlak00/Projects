using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Database
{
    public  class ToDoListDbContext : DbContext
    {
        public DbSet<WorkTask> WorkTasks { get; set; }

        
        
    }
}
