using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Core.Entities
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public ICollection<TodoList> TodoLists { get; set; } = new List<TodoList>();
    }
}
