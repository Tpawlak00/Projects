using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWPF
{
    /// <summary>
    /// Klasa przechowujaca wartosci z jakich musi sie skladac nasz Task
    /// </summary>
    public class WorkTaskViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public bool IsSelected { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        
    }
}
